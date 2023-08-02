using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GunItem : Item
{
    public IntReactiveProperty magazineSize = new();
    public GameObject bulletObj;
    public Transform gunPoint;
    public GameObject gunEffect;
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }
}
