using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Photon.Pun;

public class DamageTextModel :MonoBehaviourPunCallbacks
{
    //[HideInInspector] public MMFeedbacks hitFeedBack;
    public MMFeedbacks hitFeedBack;
    [HideInInspector] public Transform feedBackLocation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("これを動的にアバター（じぶんだけ）にアタッチし、ダメージを受けたときに全員にそれを同期する");
    }

    [PunRPC]
    public void ShowDamageText(Vector3 impactPosition,float damage)
    {
        hitFeedBack.PlayFeedbacks(impactPosition,damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + "がぶつかってきた");
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var d = collision.gameObject.GetComponent<Bullet>();
            photonView.RPC(nameof(ShowDamageText),RpcTarget.All,feedBackLocation.position,d.power);
        }
    }  
    
}
