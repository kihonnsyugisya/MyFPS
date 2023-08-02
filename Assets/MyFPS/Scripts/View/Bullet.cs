using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    [HideInInspector] public float power;
    [HideInInspector] public int playerID;
    [HideInInspector] public BulletPool bulletPool;
    public BulletType bulletType;
    public Rigidbody rigid;
    [SerializeField] private GameObject bulletHoleEffect;

    private void OnEnable()
    {
        Invoke(nameof(StoryEnd),5f);
    }

    private void StoryEnd()
    {
        BulletPool.Release(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.name == AvatarManager.avatarName) return;
        DispBulletHole(transform.position);
        Debug.Log(collision.gameObject.name + " にぶつかった by 弾");
        CancelInvoke();
        BulletPool.Release(this);
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