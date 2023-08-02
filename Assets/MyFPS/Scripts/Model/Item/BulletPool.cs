using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public ObjectPool<Bullet> shortBullets;
    public ObjectPool<Bullet> longBullets;

    [SerializeField] private Bullet shortBullet;
    [SerializeField] private Bullet longBullet;
    // Start is called before the first frame update
    void Start()
    {
        //if (shortBullets == null) return;
        shortBullets = new ObjectPool<Bullet>
        (
            createFunc: ()=> Instantiate(shortBullet),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 90,
            maxSize: 270           
        );

        longBullets = new ObjectPool<Bullet>
        (
            createFunc: ()=> Instantiate(longBullet),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 24,
            maxSize: 72           
        );

    }

    public ObjectPool<Bullet> GetPool(BulletType bulletType)
    {
        switch (bulletType)
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
}
