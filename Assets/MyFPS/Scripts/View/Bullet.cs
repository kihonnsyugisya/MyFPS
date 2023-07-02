using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    [HideInInspector] public float power;
    [HideInInspector] public int playerID;
    [SerializeField] private GameObject bulletHoleEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        DispBulletHole(collision.transform.position);
        Debug.Log(collision.gameObject.name);
        Destroy(gameObject);
    }

    private void DispBulletHole(Vector3 dispPoint)
    {
        if(bulletHoleEffect) Instantiate(bulletHoleEffect,dispPoint,Quaternion.identity);
    }
}
    