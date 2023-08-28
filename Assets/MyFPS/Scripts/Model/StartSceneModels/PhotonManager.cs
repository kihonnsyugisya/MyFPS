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

    [SerializeField] private int roomMaxPlayer;

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

        // ルームが満員になったら、以降そのルームへの参加を不許可にする
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            Debug.Log("満員(" + PhotonNetwork.CurrentRoom.MaxPlayers + ")になったので締め切りました");
        }
    }



    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("on join room field");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = roomMaxPlayer;
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
        Debug.Log("1");
        foreach (var roomPlayer in GameSystemModel.playerList)
        {
            if (roomPlayer.Value.userID == player.UserId)
            {
                Debug.Log("2");
                //GameSystemModel.playerList[roomPlayer.Key].killerID = 0;
                //GameSystemModel.playerList.Remove(roomPlayer.Key);
                photonView.RPC(nameof(ShareRemovePlayerList),RpcTarget.All,roomPlayer.Key);
                Debug.Log(roomPlayer.Key + "　をリストから削除");
                return;
            }
        }
        Debug.Log(player.NickName + " が退出しました");
    }

    [PunRPC]
    private void ShareRemovePlayerList(int playerID)
    {
        Debug.Log("3");
        GameSystemModel.RemovePlayerList(in playerID);
    }


}

public enum ConnectionStatus
{
    
}
