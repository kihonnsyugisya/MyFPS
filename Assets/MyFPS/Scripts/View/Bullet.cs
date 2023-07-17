using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    [HideInInspector] public float power;
    [HideInInspector] public int playerID;
    public BulletType bulletType;
    [SerializeField] private GameObject bulletHoleEffect;
    private float lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.fixedDeltaTime;
        if (lifeTime > 7)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.name == "3RD Person") return;
        DispBulletHole(transform.position);
        Debug.Log(collision.gameObject.name);
        Destroy(gameObject);
    }

    private void DispBulletHole(Vector3 dispPoint)
    {
        if(bulletHoleEffect) Instantiate(bulletHoleEffect,dispPoint,Quaternion.identity);
    }


}

public enum BulletType
{
    Short,Long,Shot
}