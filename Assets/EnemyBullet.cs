using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private Transform shotPoint;

    [SerializeField] private Transform shotPoint2;

    [SerializeField] private Transform shotPoint3;

    [SerializeField] private PoolManager poolManager;

    [SerializeField] private GameObject target;
    //全方位の弾の数
    private int shotCount = 12;

    private int shotCount2 = 10;

    private float angle = 0;

    private float CoolTime;

    //発射する角度間隔
    private float shotAngleIncrement;

    private float shotHalfAngleIncrement;

    private float vecX;

    private float vecY;

    private float spawnX;

    private float spawnY;

    private float Maxspawn;

    // 禁止エリアの半径
    private float spawnRadius;

    // 生成位置を試行する最大回数
    public int maxAttempts = 10;

    public float minX = -8f; // 最低x座標
    public float maxX = 8f; // 最高x座標

    public float minY = -3f; // 最低x座標
    public float maxY = 3f; // 最高x座標

    private int enemyHp;

    private int enemyPhase;

    private bool isPhaseChanging = false;

    // Start is called before the first frame update
    void Start()
    {
        shotAngleIncrement = 360f / shotCount;

        shotHalfAngleIncrement = -180f / shotCount2;

        CoolTime = 0f;

        spawnRadius = 2f;

        enemyHp = 100;

        enemyPhase = 1;
    }

    // Updateは毎フレーム呼び出される
    void Update()
    {
        // 毎フレーム角度を増加させる
        angle += 15 * Time.deltaTime; // 毎秒45度回転

        //時間経過
        CoolTime += Time.deltaTime;

        //Debug.Log(enemyPhase);

        //Debug.Log(enemyHp);

        if(enemyPhase == 1)
        {
            if (CoolTime >= 0.5f)
            {
                OmnidirectionalBullet();
                TrackingBullet();
                CoolTime = 0f;
            }
        }
        else if(enemyPhase == 2)
        {
            if (CoolTime >= 0.6f)
            {
                ScrewBullet();
                HorizontalBullet();
                CoolTime = 0f;
            }
        }else if(enemyPhase == 3)
        {
            if (CoolTime >= 0.6f)
            {
                TrackingBullet();
                FanBullet();
                CoolTime = 0f;
            }
        }else if (enemyPhase == 4)
        {
            if (CoolTime >= 0.6f)
            {
                ScrewBullet();
                FanBullet();
                CoolTime = 0f;
            }
        }

        if(enemyHp <= 0 && !isPhaseChanging)
        {
            StartCoroutine(EnemyPhaseChange());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            enemyHp = enemyHp - 1;
        }
    }

    IEnumerator EnemyPhaseChange()
    {
        isPhaseChanging = true; // フェーズ変更中フラグを立てる

        yield return new WaitForSeconds(2.0f);

        if(enemyPhase == 5)
        {
            SceneManager.LoadScene("ClearScene");
        }else
        {
            enemyPhase++;

            enemyHp = 100;

            isPhaseChanging = false;
        }
    }

    void TrackingBullet()
    {
        Vector3 spawnPosition = Vector3.zero;

        bool validPositionFound = false;


        for (int i = 0; i < maxAttempts; i++)
        {
            vecX = Random.Range(-8f, 8f);

            vecY = Random.Range(-3f, 3f);

            spawnPosition = new Vector3(vecX, vecY, 0);

            // 禁止エリア外なら有効
            if (Vector3.Distance(spawnPosition, target.transform.position) > spawnRadius)
            {
                if (spawnPosition.x >= minX && spawnPosition.x <= maxX && spawnPosition.y >= minY && spawnPosition.y <= maxY)
                {
                    validPositionFound = true;
                    break;
                }
            }
        }


        // 有効な位置が見つかった場合のみ生成
        if (validPositionFound)
        {
            GameObject tracking = poolManager.GetShot(PoolType.TrackingPool);
            tracking.transform.position = spawnPosition;
        }
        else
        {
            Debug.LogWarning("Valid spawn position not found within the max attempts.");
        }
    }

    private void OnDrawGizmos()
    {
        // 生成禁止エリアをGizmoで描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, spawnRadius);
    }

    void ScrewBullet()
    {
        // キューブをプールから取得
        GameObject screw = poolManager.GetShot(PoolType.ScrewPool);

        // 位置と回転を設定
        screw.transform.position = shotPoint.transform.position;
        screw.transform.rotation = Quaternion.Euler(0, 0, angle);

        GameObject screwDown = poolManager.GetShot(PoolType.ScrewPool);

        screwDown.transform.position = shotPoint.transform.position;
        screwDown.transform.rotation = Quaternion.Euler(0, 0, angle);

        screwDown.GetComponent<Shot>().SetDirectionDown(Vector3.down);
    }

    void OmnidirectionalBullet()
    {
        for (int i = 0; i < shotCount; i++)
        {
            //指定したプールからオブジェクトを取得
            GameObject odm = poolManager.GetShot(PoolType.OmnidirectionalPool);

            //弾ごとに角度を増加
            float angle = i * shotAngleIncrement;

            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            //発射する場所をshotpointで指定する
            odm.transform.position = shotPoint3.transform.position;

            //発射方向を設定
            odm.GetComponent<Shot>().SetDirectionUp(direction);
        }
    }

    void FanBullet()
    {
        for (int i = 0; i < shotCount2; i++)
        {
            //指定したプールからオブジェクトを取得
            GameObject fan = poolManager.GetShot(PoolType.FanPool);

            //弾ごとに角度を増加
            float angle = i * shotHalfAngleIncrement;

            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            //発射する場所をshotpointで指定する
            fan.transform.position = shotPoint.transform.position;

            //発射方向を設定
            fan.GetComponent<Shot>().SetDirectionUp(direction);
        }
    }

    void HorizontalBullet()
    {
        // 横3列の弾を生成
        int columnCount = 18;
        float spacing = 2.0f; // 横の間隔

        for (int i = 0; i < columnCount; i++)
        {
            // 弾をオブジェクトプールから取得
            GameObject horizontal = poolManager.GetShot(PoolType.HorizontalPool);

            // 発射位置を中央から左右にずらす
            float offsetX = (i) * spacing;  // -1, 0, 1 の3つの値を取得

            // 弾の位置を設定 (shotpointの位置から横にずらす)
            Vector3 spawnPosition = shotPoint2.transform.position + new Vector3(offsetX, 0, 0);
            horizontal.transform.position = spawnPosition;

            horizontal.GetComponent<Shot>().SetDirectionDown(Vector3.down);
        }
    }
}