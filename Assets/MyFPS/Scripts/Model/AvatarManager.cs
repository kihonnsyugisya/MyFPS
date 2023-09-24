using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using Photon.Realtime;
using MoreMountains.Feedbacks;

public class AvatarManager : MonoBehaviourPunCallbacks
{
    public Transform initSpawnPointsObj;
    public Transform nextSpawnPointsObj;
    [HideInInspector] public List<Transform> initSpawnPoints = new();
    [HideInInspector] public List<Transform> nextSpawnPoints = new();
    [HideInInspector] public GameObject myAvatar;
    [HideInInspector] public PlayerView playerView;
    public string avatarName;
    public static int myViewID;

    public TMPro.TextMeshProUGUI debugtext;

    private void Awake()
    {
        foreach (Transform point in initSpawnPointsObj) initSpawnPoints.Add(point);
        foreach (Transform point in nextSpawnPointsObj) nextSpawnPoints.Add(point);
        //avatarName = FireStoreModel.userDataCash.Avatar;

        avatarName = ResourceModel.avatars[Random.Range(0,ResourceModel.avatars.Count)].name;

        myAvatar = PhotonNetwork.Instantiate(avatarName, initSpawnPoints[Random.Range(0, initSpawnPoints.Count)].position, Quaternion.identity);
        myViewID = myAvatar.GetPhotonView().ViewID;
        playerView = myAvatar.GetComponent<PlayerView>();
        myAvatar.name = FireStoreModel.userDataCash.NickName;
        photonView.RPC(nameof(SetPlayerList), RpcTarget.AllBuffered, myViewID,PhotonNetwork.LocalPlayer.UserId,PhotonNetwork.NickName);
        photonView.RPC(nameof(SetGunModel),RpcTarget.AllBuffered,myViewID);
        photonView.RPC(nameof(SetDamegeTextModel),RpcTarget.AllBuffered,myViewID);
        SetItemManager();
        SetPlayerModel();
        SetCamera();
        SetAnimatorSyncSetting();

        Debug.Log(PhotonNetwork.CloudRegion + " " + PhotonNetwork.CurrentRoom.Name);
        debugtext.text = "region " + PhotonNetwork.CloudRegion + " roomName: " + PhotonNetwork.CurrentRoom.Name;
    }

    [HideInInspector] public PlayerModel playerModel;
    public AudioClip orenoVoice;
    public Joystick moveJoystick;
    public Joystick rotateJoystick;
    private void SetPlayerModel()
    {
        playerModel = myAvatar.AddComponent<PlayerModel>();
        playerModel.audioSource = myAvatar.AddComponent<AudioSource>();

        playerModel.audioSource.mute = true;

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
    public ObjectPools objectPool;
    public RectTransform AimPoint;
    [PunRPC]
    private void SetGunModel(int viewID)
    {
        GameObject target = GameSystemModel.playerList[viewID].gameObject;
        PlayerView pv = target.GetComponent<PlayerView>();
        gunModel = target.AddComponent<GunModel>();
        gunModel.objectPool = objectPool;
        gunModel.AimPoint = AimPoint;
        gunModel.shoulderWeaponPoint = pv.shoulderWeaponPoint;
        gunModel.handWeaponPoint = pv.handWeaponPoint;
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
    private void SetPlayerList(int viewID, string userID, string nickName )
    {
        GameSystemModel.SetPlayerList(in viewID,in userID, in nickName);
    }

    public MMFeedbacks hitFeedBack;
    public DamageModel damageModel;
    //[HideInInspector] public DamageModel damageModel;

    [PunRPC]
    private void SetDamegeTextModel(int viewID)
    {
        GameObject target = GameSystemModel.playerList[viewID].gameObject;
        var d = target.AddComponent<DamageModel>();
        d.hitFeedBack = hitFeedBack;
        d.feedBackLocation = GameSystemModel.playerList[viewID].eye;
        if (viewID == myViewID)
        {
            damageModel = d;
        }
    }

}
