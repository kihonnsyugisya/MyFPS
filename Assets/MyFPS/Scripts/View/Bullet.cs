using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    [HideInInspector] public float power;
    [HideInInspector] public int playerID;
    public BulletType bulletType;
    public Rigidbody rigid;
    [SerializeField] private GameObject bulletHoleEffect;
    public BulletPool bulletPool;

    private void OnEnable()
    {
        Invoke(nameof(StoryEnd),5f);
    }

    private void StoryEnd()
    {
        bulletPool.ReleaseBullet(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.name == AvatarManager.avatarName) return;
        //DispBulletHole(transform.position);
        var h = bulletPool.holes.Get();
        h.transform.SetLocalPositionAndRotation(transform.position,Quaternion.identity);
        Debug.Log(collision.gameObject.name + " にぶつかった by 弾");
        CancelInvoke();
        bulletPool.ReleaseBullet(this);
    }

}

public enum BulletType
{
    Short,Long,Shot
}