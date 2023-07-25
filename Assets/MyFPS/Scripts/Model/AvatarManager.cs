using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class AvatarManager : MonoBehaviourPunCallbacks
{   
    public List<Transform> spawnPoint = new();
    [HideInInspector] public GameObject myAvatar;
    private PlayerView playerView;

    private void Awake()
    {
        myAvatar = PhotonNetwork.Instantiate("3RD Person", spawnPoint[Random.Range(0, spawnPoint.Count)].position, Quaternion.identity);
        playerView = myAvatar.GetComponent<PlayerView>();
        SetGunModel();
        SetItemManager();
        SetPlayerModel();
        SetCamera();
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



}
