using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;

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
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        isConnectedRandomRoom.Value = true;
    }



    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("on join room field");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsOpen = roomOptions.PublishUserId = roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(null,roomOptions,null);
        Debug.Log("make room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("on created room field"); 
    }

    


}

public enum ConnectionStatus
{
    
}
