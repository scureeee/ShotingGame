using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerLife;

    [SerializeField] private Transform shotPoint;

    [SerializeField] private PoolManager poolManager;

    // 移動範囲の制限
    private float minX = -8.5f;

    private float maxX = 8.5f;

    private float minY = -4.7f;

    private float maxY = 4.5f;

    //移動スピードと点滅の間隔
    [SerializeField] float flashInterval;

    //点滅させるときのループカウント
    [SerializeField] int loopCount;

    private float CoolTime;

    private float speed;

    //発射する角度間隔
    private float shotAngleIncrement;

    private float shotHalfAngleIncrement;

    public int playerHp;

    private int invincibilityCount;

    //点滅させるためのSpriteRenderer
    SpriteRenderer sp;

    //コライダーをオンオフするためのBoxCollider2D
    BoxCollider2D bc2d;

    //接触判定
    bool isHit;

    //プレイヤーの状態（ノーマル、ダメージ、無敵の3種類）
    enum STATE
    {
        NOMAL,
        DAMAGE,
        INVINCIBILITY
    }

    //STATE型の変数
    STATE state;

    // Start is called before the first frame update
    void Start()
    {
        //STATE.INVINCIBILITYに移行するtime
        invincibilityCount = 1;

        CoolTime = 0f;

        playerHp = 6;

        speed = 4f;

        //SpriteRenderer格納
        sp = GetComponent<SpriteRenderer>();

        //BoxCollider2D格納
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Updateは毎フレーム呼び出される
    void Update()
    {
        //ダメージならreturnする
        if(state == STATE.DAMAGE)
        {
            return;
        }

        //Debug.Log(playerHp);

        //時間経過
        CoolTime += Time.deltaTime;

        // 横軸（X）の入力を取得
        float x = Input.GetAxis("Horizontal");

        // 縦軸（Y）の入力を取得
        float y = Input.GetAxis("Vertical");

        // プレイヤーの位置を更新する
        transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

        // 移動範囲の制限を適用
        var pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;

        playerLife.text = "Life:" + playerHp;

        if (CoolTime >= 0.3f)
        {
            PlayerBullet();
            CoolTime = 0f;
        }

        if(playerHp <= 0)
        {
            SceneManager.LoadScene("Title");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Enemy")
        {
            playerHp = playerHp - 1;
            
            //弾が当たっていた場合STATEを移行しない
            if(isHit)
            {
                return;
            }

            state = STATE.DAMAGE;
            StartCoroutine(hit());
        }
    }

    //点滅させる処理
    IEnumerator hit()
    {
        //当たりフラグをtrueにする
        isHit = true;

        //当たり判定off
        bc2d.enabled = false;

        sp.color = Color.black;

        //点滅ループ開始
        for(int i = 0; i < loopCount; i++)
        {
            //flashIntervalの待機
            yield return new WaitForSeconds(flashInterval);

            //spriteRendererをoff
            sp.enabled = false;

            //flashIntervalの待機
            yield return new WaitForSeconds(flashInterval);

            //spriteRendererをon
            sp.enabled = true;

            //ループが20回した時の処理
            if(i > invincibilityCount)
            {
                //stateをINVINCIBILITYにする
                state = STATE.INVINCIBILITY;

                sp.color = Color.green;
            }

        }

        //ループが終了時にstateをNOMALにする
        state = STATE.NOMAL;

        //当たり判定をonにする
        bc2d.enabled = true;

        sp.color = Color.white;

        //点滅ループを抜けたらhitをfalseにする
        isHit = false;
    }


    void PlayerBullet()
    {
        // 横3列の弾を生成
        int columnCount = 3;
        float spacing = 0.2f; // 横の間隔

        for (int i = 0; i < columnCount; i++)
        {
            // 弾をオブジェクトプールから取得
            GameObject shotRed = poolManager.GetShot(PoolType.PlayerPool);

            //発射位置を中央から左右にずらす
            float offsetX = (i - 1) * spacing;  // -1, 0, 1 の3つの値を取得

            // 弾の位置を設定 (shotpointの位置から横にずらす)
            Vector3 spawnPosition = shotPoint.transform.position + new Vector3(offsetX, 0, 0);
            shotRed.transform.position = spawnPosition;
        }
    }
}