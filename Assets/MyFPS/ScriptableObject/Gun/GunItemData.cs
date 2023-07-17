using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemDataBase")]
public class GunItemData : ItemSourceData
{
    public float atkPoint;
    public int magazineSize;
    public float rate;
    public GunType gunType;
    public BulletType bulletType;
}

public enum GunType 
{ 
    AR,SMG,SG,SR,HG,RPG
}