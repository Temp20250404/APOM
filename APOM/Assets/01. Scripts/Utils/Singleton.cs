using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    private static bool isInit = false;
    private static bool useDontDestroyOnLoad = true;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 오브젝트 탐색
                instance = FindObjectOfType<T>();

                // 씬에 없다면 새로 생성
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }

                Init();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 부모 오브젝트가 있을 경우 부모 해제
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        if (useDontDestroyOnLoad)
        {
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    private static void Init()
    {
        if (instance != null && isInit == false)
        {
            if (useDontDestroyOnLoad)
            {
                DontDestroyOnLoad(instance.gameObject);
            }

            isInit = true;
        }
    }

    /// <summary> 
    /// 싱글톤 객체 초기 설정(반드시 Instance 생성 전에 호출)
    /// </summary>
    public static void SetInitSettings(bool useDontDestroy)
    {
        useDontDestroyOnLoad = useDontDestroy;
    }
}
