using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerView : MonoBehaviour
{
    public Transform eye;
    public Transform Aim;
    public Transform shoulderWeaponPoint;
    public Transform handWeaponPoint;
    public string userID;

    /// <Summary>
    /// 殺された相手のID 
    /// </Summary>
    [HideInInspector] public int killerID;

    /// <Summary>
    /// 殺された相手の名前 
    /// </Summary>
    [HideInInspector] public string killerName;

    /// <Summary>
    ///  殺したプレーヤ達の情報が格納される
    /// </Summary>
    [HideInInspector] public ReactiveDictionary<int,string> killedInfo = new(); 
}
