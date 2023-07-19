using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemId;
    public ItemType itemType;
    [HideInInspector] public ItemInfoPlate itemInfoPlate;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        gameObject.tag = "Item";
        gameObject.layer = 2;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum ItemType 
{ 
    GUN,BULLET
}