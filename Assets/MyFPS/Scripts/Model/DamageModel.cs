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
    [HideInInspector] public IntReactiveProperty hp = new(100);

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
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var d = collision.gameObject.GetComponent<Bullet>();
            photonView.RPC(nameof(ShowDamageText), RpcTarget.Others, d.power);
            hp.Value -= (int)d.power;
        }
        else {
            Debug.Log(collision.gameObject.tag + " これです。");
        }
    }  
    
}
