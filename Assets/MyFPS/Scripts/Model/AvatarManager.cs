using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using Photon.Realtime;

public class AvatarManager : MonoBehaviourPunCallbacks
{   
    public List<Transform> spawnPoint = new();
    [HideInInspector] public GameObject myAvatar;
    private PlayerView playerView;
    public static string avatarName = "3RD Person";
    public static int myViewID;
    public static Dictionary<int, PlayerView> playerList = new();

    public TMPro.TextMeshProUGUI debugtext;

    private void Awake()
    {
        myAvatar = PhotonNetwork.Instantiate(avatarName, spawnPoint[Random.Range(0, spawnPoint.Count)].position, Quaternion.identity);
        myViewID = myAvatar.GetPhotonView().ViewID;
        playerView = myAvatar.GetComponent<PlayerView>();
        playerView.player = PhotonNetwork.LocalPlayer;
        myAvatar.name = avatarName;
        photonView.RPC(nameof(SetPlayerList), RpcTarget.AllBuffered, myViewID);

        SetGunModel();
        SetItemManager();
        SetPlayerModel();
        SetCamera();
        SetAnimatorSyncSetting();

        Debug.Log(PhotonNetwork.CloudRegion + " " + PhotonNetwork.CurrentRoom.Name);
        debugtext.text = "region " + PhotonNetwork.CloudRegion + " roomName: " + PhotonNetwork.CurrentRoom.Name;
    }

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    [HideInInspector] public PlayerModel playerModel;
    public AudioClip orenoVoice;
    public Joystick moveJoystick;
    public Joystick rotateJoystick;
    private void SetPlayerModel()
    {
        playerModel = myAvatar.AddComponent<PlayerModel>();
        playerModel.audioSource = myAvatar.AddComponent<AudioSource>();
        playerModel.moveJoystick = moveJoystick;
        playerModel.rotateJoystick = rotateJoystick;
        playerModel.audioClips.Add(orenoVoice);
        playerModel.itemManager = itemManager;
        playerModel.eye = playerView.eye;
        playerModel.Aim = playerView.Aim;
    }

    [HideInInspector] public ItemManager itemManager;
    public GameObject itemInfoPlateObj;
    private void SetItemManager()
    {
        itemManager = myAvatar.AddComponent<ItemManager>();
        itemManager.itemInfoPlateObj = itemInfoPlateObj;
        itemManager.gunModel = gunModel;
        itemManager.itemDataBase = GetComponent<ItemDataBase>();
    }

    [HideInInspector] public GunModel gunModel;
    public RectTransform AimPoint;
    private void SetGunModel()
    {
        gunModel = myAvatar.AddComponent<GunModel>();
        gunModel.AimPoint = AimPoint;
        gunModel.shoulderWeaponPoint = playerView.shoulderWeaponPoint;
        gunModel.handWeaponPoint = playerView.handWeaponPoint;
    }

    public CinemachineStateDrivenCamera stateDrivenCamera;
    private void SetCamera()
    {
        stateDrivenCamera.Follow = myAvatar.transform;
        stateDrivenCamera.LookAt = playerModel.eye;
        stateDrivenCamera.m_AnimatedTarget = myAvatar.GetComponent<Animator>();
        stateDrivenCamera.ChildCameras[1].LookAt = playerModel.Aim;
        //stateDrivenCamera.
    }

    private void SetAnimatorSyncSetting()
    {
        Animator animator = myAvatar.GetComponent<Animator>();
        PhotonAnimatorView photonAnimatorView = myAvatar.GetComponent<PhotonAnimatorView>();

        for (var count = 0; count < animator.layerCount; count++)
        {
            photonAnimatorView.SetLayerSynchronized(count,PhotonAnimatorView.SynchronizeType.Discrete);
        }

        foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
        {
            photonAnimatorView.SetParameterSynchronized(animatorControllerParameter.name,(PhotonAnimatorView.ParameterType)animatorControllerParameter.type,PhotonAnimatorView.SynchronizeType.Discrete);
        }
    }

    [PunRPC]
    private void SetPlayerList(int viewID)
    {
        PlayerView pv = PhotonView.Find(viewID).GetComponent<PlayerView>();

        playerList.Add(viewID, pv);      
        
    }
    //PhotonNetwork.UseRpcMonoBehaviourCache = true;


    public override void OnPlayerLeftRoom(Player player)
    {
        foreach (var roomPlayer in playerList)
        {
            if (roomPlayer.Value.player.UserId == player.UserId)
            {
                playerList.Remove(roomPlayer.Key);
                Debug.Log(roomPlayer.Key + "　をリストから削除");
                return;
            }
        }
        Debug.Log(player.NickName + " が退出しました");
    }



}
