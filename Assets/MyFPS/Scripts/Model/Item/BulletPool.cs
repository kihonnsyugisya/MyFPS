using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public ObjectPool<Bullet> shortBullets;
    public ObjectPool<Bullet> longBullets;
    public ObjectPool<CFX_AutoDestructShuriken> holes;

    [SerializeField] private Bullet shortBullet;
    [SerializeField] private Bullet longBullet;
    [SerializeField] private CFX_AutoDestructShuriken hole;

    // Start is called before the first frame update
    void Start()
    {
        //if (shortBullets == null) return;
        shortBullets = new ObjectPool<Bullet>
        (
            createFunc: () =>
            {
                var d = Instantiate(shortBullet);
                d.bulletPool = this;
                return d;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 90,
            maxSize: 270           
        );

        longBullets = new ObjectPool<Bullet>
        (
            createFunc: () =>
            {
                var d = Instantiate(longBullet);
                d.bulletPool = this;
                return d;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 24,
            maxSize: 72           
        );

        holes = new ObjectPool<CFX_AutoDestructShuriken>
        (
            createFunc: () =>
            {
                var d = Instantiate(hole);
                d.bulletPool = this;
                return d;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 50,
            maxSize: 100
        );

    }

    public ObjectPool<Bullet> GetPool(int bulletType)
    {
        switch ((BulletType)bulletType)
        {
            case BulletType.Short:
                return shortBullets;
            case BulletType.Long:
                return longBullets;
            case BulletType.Shot:
                break;
        }
        return shortBullets;
    }

    public void ReleaseBullet(Bullet o)
    {
        o.rigid.velocity = Vector3.zero;
        GetPool((int)o.bulletType).Release(o);
    }

    public void RereaseHole(CFX_AutoDestructShuriken c)
    {
        holes.Release(c);
        Debug.Log("owata");
    }
}
