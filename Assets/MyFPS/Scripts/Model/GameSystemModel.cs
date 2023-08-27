using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Photon.Pun;

public class GameSystemModel 
{
    public static ReactiveDictionary<int, PlayerView> playerList = new();
    // Start is called before the first frame update


    public static void SetPlayerList(in int viewID, in string userID)
    {
        PlayerView pv = PhotonView.Find(viewID).GetComponent<PlayerView>();
        pv.userID = userID;
        playerList.Add(viewID, pv);
        Debug.Log("set list");
    }

    public static void RemovePlayerList(in int playerID, in string killerName, in int killerID)
    {
        playerList[playerID].killerName = killerName;
        playerList[playerID].killerID = killerID;
        playerList.Remove(playerID);

    }
}
