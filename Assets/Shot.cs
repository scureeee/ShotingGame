using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]private PoolManager poolManager;

    [SerializeField] private float speed;

    // 弾の進行方向を初期化（デフォルトは上方向）
    private Vector3 direction = Vector3.up;

    //new
    // 弾の回転速度を初期化
    private float angularVelocity = 0f;

    // 弾の発射からの経過時間を初期化
    private float lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //new
    //発射方向と回転速度を設定するメソッド
    public void SetDirectionDown(Vector3 newDirection,float angularVelocity = 0f)
    {

        // newDirection を一時的に Down に変更
        newDirection = Vector3.down;

        // 発射方向を設定
        direction = newDirection.normalized;


        //経過時間をリセット
        lifeTime = 0f;

        //回転速度を設定
        this.angularVelocity = angularVelocity;
    }

    public void SetDirectionUp(Vector3 newDirection, float angularVelocity = 0f)
    {
        //発射方向を設定
        direction = newDirection.normalized;

        //経過時間をリセット
        lifeTime = 0f;

        //回転速度を設定
        this.angularVelocity = angularVelocity;
    }

    void AngleChange()
    {
        angularVelocity = 0f;
    }

    void AngleChange2()
    {
        angularVelocity = 32f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude >= 10)
        {
            poolManager.DelShot(gameObject);
        }
    }

    //new
    private void FixedUpdate()
    {
        // フレームごとに経過時間を加算
        lifeTime += Time.deltaTime;

        // 回転する角度を計算（角速度 × 経過時間）
        var angle = angularVelocity * lifeTime;
        
        // Z軸周りに回転するクォータニオンを作成
        var rot = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

        // 回転を適用した進行方向を計算
        var dir = rot * direction;

        //弾が移動する速さ
        transform.Translate(dir * speed * Time.deltaTime);
    }
}