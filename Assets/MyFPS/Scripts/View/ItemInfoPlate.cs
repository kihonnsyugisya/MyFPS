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
    [HideInInspector] public int uniqueID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
 
}
