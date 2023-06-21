using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPlate : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image itemIcon;
    public Button pickUpButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CustumLookAt();
    }
    [SerializeField] Vector3 f;
    private void CustumLookAt()
    {
        // ターゲットへの向きベクトル計算
        var dir = Camera.main.transform.position - transform.position;

        // ターゲットの方向への回転
        var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
        // 回転補正
        var offsetRotation = Quaternion.FromToRotation(f, Vector3.forward);

        // 回転補正→ターゲット方向への回転の順に、自身の向きを操作する
        transform.rotation = lookAtRotation * offsetRotation;
    }
    
}
