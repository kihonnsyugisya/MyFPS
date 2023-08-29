using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Photon.Pun;
using System.Linq;

public class GameSystemModel 
{
    public static ReactiveDictionary<int, PlayerView> playerList = new();
    public static BoolReactiveProperty isRoomMaxPlayer = new(false);

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
        if (playerList.TryGetValue(playerID,out _))
        {
            playerList.Remove(playerID);
        }
    }

    public static void RemovePlayerList(in int playerID)
    {
        playerList[playerID].killerID = 0;
        if (playerList.TryGetValue(playerID, out _))
        {
            playerList.Remove(playerID);
        }   
    }

    public static void PlaceAllPlayer(in List<Transform> spawnPoints)
    {
        int count = playerList.Count();
        foreach (var pair in playerList.OrderBy(player => player.Key))
        {
            pair.Value.transform.position = spawnPoints[count].position;
            count--;
        }
    }



}
