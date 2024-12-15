using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TrackingController : MonoBehaviour
{
    private PoolManager poolManager;

    // ターゲットをインスペクターから設定可能に
    private GameObject target;

    private float DeleteTime;

    // 移動速度をインスペクターで調整可能に
    [SerializeField] private float speed = 3.0f;

    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();

        DeleteTime = 0f;

        target = GameObject.Find("ship1");
    }

    private void Update()
    {
        DeleteTime += Time.deltaTime;

        // ターゲットが未設定の場合は処理をスキップ
        if (target == null) return;

        // ターゲットに向かって移動
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position, // target の position を使用
            speed * Time.deltaTime);

        if (DeleteTime >= 2.0f)
        {
            poolManager.DelShot(gameObject);

            DeleteTime = 0f;
        }
    }
}