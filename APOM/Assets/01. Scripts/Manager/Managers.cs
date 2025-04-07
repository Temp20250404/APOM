
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public interface IManager
{
    void Init();
    void Clear();
}

public class Managers : Singleton<Managers>
{
    [field: SerializeField] private GameManager gameManager = new GameManager();
    [field: SerializeField] private DataManager data;
    [field: SerializeField] private InputManager input = new InputManager();
    [field: SerializeField] private ResourceManager resource = new ResourceManager();
    [field: SerializeField] private EventManager @event = new EventManager();
    [field: SerializeField] private SoundManager sound = new SoundManager();
    [field: SerializeField] private UIManager ui = new UIManager();
    [field: SerializeField] private PoolManager pool = new PoolManager();
    [field: SerializeField] private PlayerManager @player = new PlayerManager();

    public static GameManager GameManager => Instance.gameManager;
    public static DataManager Data => Instance.data;
    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;
    public static EventManager Event => Instance.@event;
    public static SoundManager Sound => Instance.sound;
    public static UIManager UI => Instance.ui;
    public static PoolManager Pool => Instance.pool;
    public static PlayerManager Player => Instance.@player;

    protected override void Awake()
    {
        base.Awake();

        if (gameManager == null)
        {
            gameManager = new GameManager();
        }
        //  Player = FindFirstObjectByType<PlayerController>().gameObject;
        data = new DataManager();
        Init();

    }
    private void Start()
    {
        UI.ShowSceneUI<UI_SceneTest>(); // ShowSceneUI<UI_SceneTest>("여기에 class의 명이 아닌 Prefab의 이름을 넣을 수 있음")
        UI.ShowSceneUI<UI_SceneTest>("UI_SceneTest 1");
        UI.ShowPopupUI<UI_PopupTest>(); // ShowPopupUI<UI_PopupTest>("여기에 class의 명이 아닌 Prefab의 이름을 넣을 수 있음")
        //UI.ShowPopupUI<SkillPopupUI>(); 

    }

    private void Update()
    {
        //_input.OnUpdate();
    }

    private static void Init()
    {
        Input.Init();
        Resource.Init();
        Sound.Init();
        UI.Init();
        Pool.Init();
        Player.Init();
    }

    public static void Clear()
    {

    }

    public static void SetTimer(Action action, float time)
    {
        Instance.StartCoroutine(Instance.SetTimerCoroutine(action, time));
    }

    private IEnumerator SetTimerCoroutine(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

}
