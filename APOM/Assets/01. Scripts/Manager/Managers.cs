
using Game;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UGS;
using UnityEngine;

public interface IManager
{
    void Init();
    void Clear();
}

public class Managers : Singleton<Managers>
{
    [field: SerializeField] private GameManager gameManager = new GameManager();
    [field: SerializeField] private DataManager data = new DataManager();
    [field: SerializeField] private InputManager input = new InputManager();
    [field: SerializeField] private ResourceManager resource = new ResourceManager();
    [field: SerializeField] private EventManager @event = new EventManager();
    [field: SerializeField] private SoundManager sound = new SoundManager();
    [field: SerializeField] private UIManager ui = new UIManager();
    [field: SerializeField] private PoolManager pool = new PoolManager();
    [field: SerializeField] private PlayerManager @player = new PlayerManager();
    [field: SerializeField] private NetworkManager network = new NetworkManager();
    [field: SerializeField] private PacketManager packet = new PacketManager();
    [field: SerializeField] private BossManager bossManager = new BossManager();

    public static GameManager GameManager => Instance.gameManager;
    public static DataManager Data => Instance.data;
    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;
    public static EventManager Event => Instance.@event;
    public static SoundManager Sound => Instance.sound;
    public static UIManager UI => Instance.ui;
    public static PoolManager Pool => Instance.pool;
    public static PlayerManager Player => Instance.@player;
    public static NetworkManager Network => Instance.network;
    public static PacketManager Packet => Instance.packet;
    public static BossManager BossManager => Instance.bossManager;

    protected override void Awake()
    {
        base.Awake();

        if (gameManager == null)
        {
            gameManager = new GameManager();
        }

        Screen.SetResolution(1920, 1080, false); // 해상도 설정    
        Application.targetFrameRate = 60;   // 최대 프레임 60으로 조정
        JobQueue.Push(() => { });           // 잡큐 enq 시작

        Init();
    }
    private void Start()
    {
        UI.ShowPopupUI<UI_Login>();
        //UI.ShowPopupUI<UI_IDFind>();
        //UI.ShowPopupUI<UI_SignUp>();

        //UI.ShowPopupUI<UI_PWFind>();

        //UI.ShowPopupUI<UI_Inventory>(); // ShowPopupUI<UI_PopupTest>("여기에 class의 명이 아닌 Prefab의 이름을 넣을 수 있음")
                                        //UI.ShowPopupUI<UI_PopupTest>(); // ShowPopupUI<UI_PopupTest>("여기에 class의 명이 아닌 Prefab의 이름을 넣을 수 있음")
                                        //UI.ShowPopupUI<SkillPopupUI>(); 

        // 서버에 4초를 주기로 생존 여부를 알리는 패킷을 보내는 기능
        StartCoroutine(SendTimeoutPackt()); 
    }

    private void Update()
    {
        Network.Update();
    }

    private static void Init()
    {
        Data.Init();
        Input.Init();
        Resource.Init();
        Sound.Init();
        UI.Init();
        Pool.Init();
        Player.Init();
        Network.Init();
        Packet.Init();
        BossManager.Init();
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

    IEnumerator SendTimeoutPackt()
    {
        while (true)
        {
            CS_CHECK_TIMEOUT ptk = new CS_CHECK_TIMEOUT();
            ptk.BCheck = true;

            Managers.Network.Send(ptk);
            yield return new WaitForSeconds(4f);
        }
    }

    public void PacketTestMethed(SC_POSITION_SYNC _packet)
    {
        Debug.Log($"ID: {_packet.PlayerID} - {_packet.PosX}, {_packet.PosY}, {_packet.CameraYaw}");
    }
}
