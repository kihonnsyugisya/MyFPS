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
        DontDestroyOnLoad(this);
    }

    public void ConnectionMastarServer()
    {
        if (PhotonNetwork.IsConnected) return;
        Debug.Log("connection master server");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        isConnectedMaster.Value = true;
        Debug.Log("on connected to master !!!");
    }

    // Photonのサーバーから切断された時に呼ばれるコールバック
    public override void OnDisconnected(DisconnectCause cause)
    {     
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
    }


    public void GoToRandomMatchRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        isConnectedRandomRoom.Value = true;
    }

    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    Debug.Log("on join room field");
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = roomMaxPlayer;
    //    roomOptions.IsOpen = roomOptions.PublishUserId = roomOptions.IsVisible = true;
    //    PhotonNetwork.CreateRoom(null,roomOptions,null);
    //    Debug.Log("make room");
    //}


    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
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
                GameSystemModel.RemovePlayerList(roomPlayer.Key);
                Debug.Log(roomPlayer.Key + "　をリストから削除");
                return;
            }
        }
        Debug.Log(player.NickName + " が退出しました");
    }

    public static bool isRoomMaxPlayer()
    {
        if (GameSystemModel.playerList.Count >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            return true;
        }
        else return false;
    }

}

public enum ConnectionStatus
{
    
}
