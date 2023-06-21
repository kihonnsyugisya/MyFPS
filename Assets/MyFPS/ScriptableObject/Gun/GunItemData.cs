using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObject/GunItemData")]
public class GunItemData : ItemSourceData
{
    public float atkPoint;
    public int magazineSize;
    public float rate;
    public GunType gunType;
}

public enum GunType 
{ 
    AR,SMG,SG,SR,HG,RPG
}