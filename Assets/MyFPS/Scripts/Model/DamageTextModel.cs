using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Photon.Pun;

public class DamageTextModel :MonoBehaviourPunCallbacks
{
    [HideInInspector] public MMFeedbacks hitFeedBack;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("これを動的にアバター（じぶんだけ）にアタッチし、ダメージを受けたときに全員にそれを同期する");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void ShowDamageText(Vector3 impactPosition,float damage)
    {
        hitFeedBack.PlayFeedbacks(impactPosition,damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "がぶつかってきた");
    }
}
