using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//enumでプールの種類を定義
public enum PoolType
{
    OmnidirectionalPool,
    FanPool,
    TrackingPool,
    ScrewPool,
    HorizontalPool,
    PlayerPool
}

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject HorizontalBullet;

    [SerializeField] private GameObject FanBullet;

    [SerializeField] private GameObject OmnidirectionalBullet;

    [SerializeField] private GameObject screwBullet;

    [SerializeField] private GameObject TrackingBullet;

    [SerializeField] private GameObject PlayerBullet;
    //オブジェクトプール
    ObjectPool<GameObject> omnidirectionalPool;

    ObjectPool<GameObject> fanPool;

    ObjectPool<GameObject> screwPool;

    ObjectPool<GameObject> trackingPool;

    ObjectPool<GameObject> horizontalPool;

    ObjectPool<GameObject> playerPool;

    private void Start()
    {
        omnidirectionalPool = new ObjectPool<GameObject>(
            () => Instantiate(OmnidirectionalBullet),
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true
            );

        fanPool = new ObjectPool<GameObject>(
            () => Instantiate(FanBullet),
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true
            );

        screwPool = new ObjectPool<GameObject>(
             () => Instantiate(screwBullet),
             OnTakeFromPool,
             OnReturnedToPool,
             OnDestroyPoolObject,
             true
             );
        trackingPool = new ObjectPool<GameObject>(
            () => Instantiate(TrackingBullet),
             OnTakeFromPool,
             OnReturnedToPool,
             OnDestroyPoolObject,
             true
             );
        horizontalPool = new ObjectPool<GameObject>(
            () => Instantiate(HorizontalBullet),
             OnTakeFromPool,
             OnReturnedToPool,
             OnDestroyPoolObject,
             true
             );
        playerPool =new ObjectPool<GameObject>(
            () => Instantiate(PlayerBullet),
             OnTakeFromPool,
             OnReturnedToPool,
             OnDestroyPoolObject,
             true
             );
    }

    //プールに空きがあった時の処理
    void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    //プールに返却する時の処理
    void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    //Maxサイズより多くなった時に自動で破棄する
    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    //指定したプールかオブジェクトを取得
    //プレイヤーから呼び出して画面に弾を発生させる
    public GameObject GetShot(PoolType poolType)
    {
        switch(poolType)
        {
            case PoolType.OmnidirectionalPool:
                return omnidirectionalPool.Get();
            case PoolType.FanPool:
                return fanPool.Get();
                default:
                return null;
            case PoolType.ScrewPool:
                return screwPool.Get();
            case PoolType.TrackingPool:
                return trackingPool.Get();
            case PoolType.HorizontalPool:
                return horizontalPool.Get();
            case PoolType.PlayerPool:
                return playerPool.Get();
        }
    }

    //オブジェクトプールに戻す
    //弾から呼び出して画面から弾を消滅させる
    public void DelShot(GameObject obj)
    {
        omnidirectionalPool.Release(obj);
    }
}
