using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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
    }

    public void GoToRandomMatchRoom()
    {
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

    public static void GoToLoby()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync("StartScene");
        //PhotonNetwork.LoadLevel("StartScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + ": on player enter");
    }


    public override void OnPlayerLeftRoom(Player player)
    {

        foreach (var roomPlayer in GameSystemModel.playerList)
        {
            if (roomPlayer.Value.userID == player.UserId)
            {
                GameSystemModel.playerList[roomPlayer.Key].killerID = 0;
                GameSystemModel.playerList.Remove(roomPlayer.Key);
                Debug.Log(roomPlayer.Key + "　をリストから削除");
                return;
            }
        }
        Debug.Log(player.NickName + " が退出しました");
    }


}

public enum ConnectionStatus
{
    
}
