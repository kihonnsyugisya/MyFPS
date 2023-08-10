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
    public ObjectPools objectPool;

    private void OnEnable()
    {
        Invoke(nameof(StoryEnd),5f);
    }

    private void StoryEnd()
    {
        objectPool.ReleaseBullet(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.name == AvatarManager.avatarName) return;
        //DispBulletHole(transform.position);
        var h = objectPool.holes.Get();
        h.transform.SetLocalPositionAndRotation(transform.position,Quaternion.identity);
        Debug.Log(collision.gameObject.name + " にぶつかった by 弾");
        CancelInvoke();
        objectPool.ReleaseBullet(this);
    }

}

public enum BulletType
{
    Short,Long,Shot
}