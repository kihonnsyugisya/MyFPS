using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [HideInInspector] public BoolReactiveProperty isConnectedMaster = new(false);
    [HideInInspector] public BoolReactiveProperty isConnectedRandomRoom = new(false);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectionMastarServer()
    {
        if (PhotonNetwork.IsConnected) return;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        isConnectedMaster.Value = true;
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        isConnectedRandomRoom.Value = true;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("room field");
    }



}

public enum ConnectionStatus
{
    
}
