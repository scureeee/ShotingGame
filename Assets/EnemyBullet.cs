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
    //�S���ʂ̒e�̐�
    private int shotCount = 12;

    private int shotCount2 = 10;

    private float angle = 0;

    private float CoolTime;

    //���˂���p�x�Ԋu
    private float shotAngleIncrement;

    private float shotHalfAngleIncrement;

    private float vecX;

    private float vecY;

    private float spawnX;

    private float spawnY;

    private float Maxspawn;

    // �֎~�G���A�̔��a
    private float spawnRadius;

    // �����ʒu�����s����ő��
    public int maxAttempts = 10;

    public float minX = -8f; // �Œ�x���W
    public float maxX = 8f; // �ō�x���W

    public float minY = -3f; // �Œ�x���W
    public float maxY = 3f; // �ō�x���W

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

    // Update�͖��t���[���Ăяo�����
    void Update()
    {
        // ���t���[���p�x�𑝉�������
        angle += 15 * Time.deltaTime; // ���b45�x��]

        //���Ԍo��
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
        isPhaseChanging = true; // �t�F�[�Y�ύX���t���O�𗧂Ă�

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

            // �֎~�G���A�O�Ȃ�L��
            if (Vector3.Distance(spawnPosition, target.transform.position) > spawnRadius)
            {
                if (spawnPosition.x >= minX && spawnPosition.x <= maxX && spawnPosition.y >= minY && spawnPosition.y <= maxY)
                {
                    validPositionFound = true;
                    break;
                }
            }
        }


        // �L���Ȉʒu�����������ꍇ�̂ݐ���
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
        // �����֎~�G���A��Gizmo�ŕ`��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, spawnRadius);
    }

    void ScrewBullet()
    {
        // �L���[�u���v�[������擾
        GameObject screw = poolManager.GetShot(PoolType.ScrewPool);

        // �ʒu�Ɖ�]��ݒ�
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
            //�w�肵���v�[������I�u�W�F�N�g���擾
            GameObject odm = poolManager.GetShot(PoolType.OmnidirectionalPool);

            //�e���ƂɊp�x�𑝉�
            float angle = i * shotAngleIncrement;

            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            //���˂���ꏊ��shotpoint�Ŏw�肷��
            odm.transform.position = shotPoint3.transform.position;

            //���˕�����ݒ�
            odm.GetComponent<Shot>().SetDirectionUp(direction);
        }
    }

    void FanBullet()
    {
        for (int i = 0; i < shotCount2; i++)
        {
            //�w�肵���v�[������I�u�W�F�N�g���擾
            GameObject fan = poolManager.GetShot(PoolType.FanPool);

            //�e���ƂɊp�x�𑝉�
            float angle = i * shotHalfAngleIncrement;

            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            //���˂���ꏊ��shotpoint�Ŏw�肷��
            fan.transform.position = shotPoint.transform.position;

            //���˕�����ݒ�
            fan.GetComponent<Shot>().SetDirectionUp(direction);
        }
    }

    void HorizontalBullet()
    {
        // ��3��̒e�𐶐�
        int columnCount = 18;
        float spacing = 2.0f; // ���̊Ԋu

        for (int i = 0; i < columnCount; i++)
        {
            // �e���I�u�W�F�N�g�v�[������擾
            GameObject horizontal = poolManager.GetShot(PoolType.HorizontalPool);

            // ���ˈʒu�𒆉����獶�E�ɂ��炷
            float offsetX = (i) * spacing;  // -1, 0, 1 ��3�̒l���擾

            // �e�̈ʒu��ݒ� (shotpoint�̈ʒu���牡�ɂ��炷)
            Vector3 spawnPosition = shotPoint2.transform.position + new Vector3(offsetX, 0, 0);
            horizontal.transform.position = spawnPosition;

            horizontal.GetComponent<Shot>().SetDirectionDown(Vector3.down);
        }
    }
}