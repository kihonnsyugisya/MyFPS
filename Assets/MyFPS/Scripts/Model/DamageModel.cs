using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Photon.Pun;
using UniRx;

public class DamageModel :MonoBehaviourPunCallbacks
{
    [HideInInspector] public MMFeedbacks hitFeedBack;
    [HideInInspector] public Transform feedBackLocation;
    [HideInInspector] public Subject<int> damageSubject = new();
    [HideInInspector] public IntReactiveProperty hp = new(100);
    [HideInInspector] public BoolReactiveProperty isDead = new(false);
    [HideInInspector] public int killerID;

    [PunRPC]
    public void ShowDamageText(float damage)
    {
        if (photonView.IsMine) return;
        Debug.Log("position " + feedBackLocation.parent.name + "ダメージ " + damage +" 実行者 " + gameObject.name);
        hitFeedBack.PlayFeedbacks(feedBackLocation.position,damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + "がぶつかってきた By " + gameObject.name);
        if (isDead.Value) return;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var d = collision.gameObject.GetComponent<Bullet>();
            photonView.RPC(nameof(ShowDamageText), RpcTarget.Others, d.power);
            hp.Value -= (int)d.power;
            damageSubject.OnNext((int)d.power);
            if (hp.Value <= 0) 
            {
                killerID = d.playerID;
                isDead.Value = true;
                photonView.RPC(nameof(SendPlayerListDead),RpcTarget.AllBuffered,AvatarManager.myViewID, AvatarManager.playerList[d.playerID].name);
                Debug.Log(AvatarManager.playerList[d.playerID].name + "に殺された");
            }
        }
        else {
            Debug.Log(collision.gameObject.tag + " これです。");
        }
    }

    [PunRPC]
    private void SendPlayerListDead(int myViewID,string killerName)
    {
        AvatarManager.playerList[myViewID].killerName = killerName;
        AvatarManager.playerList.Remove(myViewID);
    }
    
}
