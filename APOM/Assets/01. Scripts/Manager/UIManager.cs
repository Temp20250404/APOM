using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum ItemCategory // 아이템 카테고리
{
    Weapon,
    Expendable,
    Ingredient,
    Etc
}



[Serializable]
public class UIManager : IManager
{
    [SerializeField] private Transform _sceneUIParent;
    [SerializeField] private Transform _popupUIParent;
    [SerializeField] private Transform _followUIParent;

    public List<Item> inventoryItems = new List<Item>();


    private int _order = 10;

    private UI_Scene _sceneUI = null;
    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private List<UI_Follow> _followList = new List<UI_Follow>();

    public void Init()
    {
        if (_sceneUIParent == null)
        {
            GameObject sceneParent = new GameObject("SceneUIParent");
            _sceneUIParent = sceneParent.transform;
        }

        if (_popupUIParent == null)
        {
            GameObject popupParent = new GameObject("PopupUIParent");
            _popupUIParent = popupParent.transform;
        }

        if (_followUIParent == null)
        {
            GameObject followParent = new GameObject("FollowUIParent");
            _followUIParent = followParent.transform;
        }
    } 
  
    public void Clear()
    {
        
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

	public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
		if (parent != null)
			go.transform.SetParent(parent);

		return Util.GetOrAddComponent<T>(go);
	}

    public T ShowSceneChildUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        go.transform.SetParent(_sceneUIParent.transform, false);
 
        return Util.GetOrAddComponent<T>(go); 
    }

	public T ShowSceneUI<T>(string name = null) where T : UI_Scene
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
		T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI; 
  
		go.transform.SetParent(_sceneUIParent.transform, false);

		return sceneUI; 
	}

	public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        Debug.Log(name);
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        popup.Init();
        _popupStack.Push(popup);

        go.transform.SetParent(_popupUIParent.transform, false);


		return popup; 
    }

    public void ClosePopupUI(UI_Popup popup)
    {
		if (_popupStack.Count == 0)
			return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        GameObject.Destroy(popup.gameObject); 
        popup = null;
        _order--; 
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void CloseFollowUI(UI_Follow follow)
    {
        if (_followList.Contains(follow))
        {
            _followList.Remove(follow);
            GameObject.Destroy(follow.gameObject);
            _order--;
        }
    }
    public void CloseFollowUI()
    {
        if (_followList.Count == 0)
            return;

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("메인 카메라를 찾을 수 없습니다.");
            return;
        }

        for (int i = _followList.Count - 1; i >= 0; i--)
        {
            UI_Follow follow = _followList[i];

            // 뷰포트 좌표는 (0,0)에서 (1,1) 사이이면 화면 내에 있는 것으로 판단
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(follow.transform.position);
            bool isVisible = viewportPos.x >= 0 && viewportPos.x <= 1 &&
                             viewportPos.y >= 0 && viewportPos.y <= 1 &&
                             viewportPos.z > 0;

            // 카메라에 보이지 않는 경우 제거
            if (!isVisible)
            {
                _followList.RemoveAt(i);
                GameObject.Destroy(follow.gameObject);
                _order--;
            }
        }
    }

    private int GetSortPriority(ItemCategory category) // 정렬 우선순위 지정
    {
        switch (category)
        {
            case ItemCategory.Weapon: return 0;
            case ItemCategory.Expendable: return 1;
            case ItemCategory.Ingredient: return 2;
            case ItemCategory.Etc: return 3;
            default: return 4;
        }   
    }

    public void SortInventory(List<Item> inventory)
    {
        inventory.Sort(CompareItems);
        Debug.Log("인벤토리 정렬 끝");
        //UpdateInventoryUI();
    }

    private int CompareItems(Item a, Item b)
    {
        int orderA = GetSortPriority(a.category);
        int orderB = GetSortPriority(b.category);

        if (orderA != orderB)
            return orderA.CompareTo(orderB);

        return string.Compare(a.name, b.name);
    }
}