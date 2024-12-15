using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//enum�Ńv�[���̎�ނ��`
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
    //�I�u�W�F�N�g�v�[��
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

    //�v�[���ɋ󂫂����������̏���
    void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    //�v�[���ɕԋp���鎞�̏���
    void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    //Max�T�C�Y��葽���Ȃ������Ɏ����Ŕj������
    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    //�w�肵���v�[�����I�u�W�F�N�g���擾
    //�v���C���[����Ăяo���ĉ�ʂɒe�𔭐�������
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

    //�I�u�W�F�N�g�v�[���ɖ߂�
    //�e����Ăяo���ĉ�ʂ���e�����ł�����
    public void DelShot(GameObject obj)
    {
        omnidirectionalPool.Release(obj);
    }
}
