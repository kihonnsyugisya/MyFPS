using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemId;
    public ItemType itemType;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        gameObject.tag = "Item";
        gameObject.layer = 6;
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