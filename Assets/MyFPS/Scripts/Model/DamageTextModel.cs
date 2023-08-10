using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class DamageTextModel : MonoBehaviour
{
    public MMFeedbacks hit;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("これを動的にアバター（じぶんだけ）にアタッチし、ダメージを受けたときに全員にそれを同期する");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
