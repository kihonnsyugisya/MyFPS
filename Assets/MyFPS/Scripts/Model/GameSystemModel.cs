using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using MoreMountains.Feedbacks;
using Photon.Pun;

public class GameSystemModel : MonoBehaviourPunCallbacks
{
    public static ReactiveDictionary<int, PlayerView> playerList = new();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetPlayerList(in int viewID, in string userID)
    {
        PlayerView pv = PhotonView.Find(viewID).GetComponent<PlayerView>();
        pv.userID = userID;
        playerList.Add(viewID, pv);
        Debug.Log("set list");
    }

    public static void SendPlayerListDead(in int myViewID, in string killerName,in int killerID)
    {
        playerList[myViewID].killerName = killerName;
        playerList[myViewID].killerID = killerID;
        playerList.Remove(myViewID);
    }
}
