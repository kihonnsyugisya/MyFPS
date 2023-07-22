using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [HideInInspector] public BoolReactiveProperty isConnectedMaster = new(false);
    [HideInInspector] public BoolReactiveProperty isConnectedLobby = new(false);


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
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        isConnectedLobby.Value = true;
    }



}

public enum ConnectionStatus
{
    
}
