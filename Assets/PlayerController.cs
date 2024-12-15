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

    // �ړ��͈͂̐���
    private float minX = -8.5f;

    private float maxX = 8.5f;

    private float minY = -4.7f;

    private float maxY = 4.5f;

    //�ړ��X�s�[�h�Ɠ_�ł̊Ԋu
    [SerializeField] float flashInterval;

    //�_�ł�����Ƃ��̃��[�v�J�E���g
    [SerializeField] int loopCount;

    private float CoolTime;

    private float speed;

    //���˂���p�x�Ԋu
    private float shotAngleIncrement;

    private float shotHalfAngleIncrement;

    public int playerHp;

    private int invincibilityCount;

    //�_�ł����邽�߂�SpriteRenderer
    SpriteRenderer sp;

    //�R���C�_�[���I���I�t���邽�߂�BoxCollider2D
    BoxCollider2D bc2d;

    //�ڐG����
    bool isHit;

    //�v���C���[�̏�ԁi�m�[�}���A�_���[�W�A���G��3��ށj
    enum STATE
    {
        NOMAL,
        DAMAGE,
        INVINCIBILITY
    }

    //STATE�^�̕ϐ�
    STATE state;

    // Start is called before the first frame update
    void Start()
    {
        //STATE.INVINCIBILITY�Ɉڍs����time
        invincibilityCount = 1;

        CoolTime = 0f;

        playerHp = 6;

        speed = 4f;

        //SpriteRenderer�i�[
        sp = GetComponent<SpriteRenderer>();

        //BoxCollider2D�i�[
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update�͖��t���[���Ăяo�����
    void Update()
    {
        //�_���[�W�Ȃ�return����
        if(state == STATE.DAMAGE)
        {
            return;
        }

        //Debug.Log(playerHp);

        //���Ԍo��
        CoolTime += Time.deltaTime;

        // �����iX�j�̓��͂��擾
        float x = Input.GetAxis("Horizontal");

        // �c���iY�j�̓��͂��擾
        float y = Input.GetAxis("Vertical");

        // �v���C���[�̈ʒu���X�V����
        transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

        // �ړ��͈͂̐�����K�p
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
            
            //�e���������Ă����ꍇSTATE���ڍs���Ȃ�
            if(isHit)
            {
                return;
            }

            state = STATE.DAMAGE;
            StartCoroutine(hit());
        }
    }

    //�_�ł����鏈��
    IEnumerator hit()
    {
        //������t���O��true�ɂ���
        isHit = true;

        //�����蔻��off
        bc2d.enabled = false;

        sp.color = Color.black;

        //�_�Ń��[�v�J�n
        for(int i = 0; i < loopCount; i++)
        {
            //flashInterval�̑ҋ@
            yield return new WaitForSeconds(flashInterval);

            //spriteRenderer��off
            sp.enabled = false;

            //flashInterval�̑ҋ@
            yield return new WaitForSeconds(flashInterval);

            //spriteRenderer��on
            sp.enabled = true;

            //���[�v��20�񂵂����̏���
            if(i > invincibilityCount)
            {
                //state��INVINCIBILITY�ɂ���
                state = STATE.INVINCIBILITY;

                sp.color = Color.green;
            }

        }

        //���[�v���I������state��NOMAL�ɂ���
        state = STATE.NOMAL;

        //�����蔻���on�ɂ���
        bc2d.enabled = true;

        sp.color = Color.white;

        //�_�Ń��[�v�𔲂�����hit��false�ɂ���
        isHit = false;
    }


    void PlayerBullet()
    {
        // ��3��̒e�𐶐�
        int columnCount = 3;
        float spacing = 0.2f; // ���̊Ԋu

        for (int i = 0; i < columnCount; i++)
        {
            // �e���I�u�W�F�N�g�v�[������擾
            GameObject shotRed = poolManager.GetShot(PoolType.PlayerPool);

            //���ˈʒu�𒆉����獶�E�ɂ��炷
            float offsetX = (i - 1) * spacing;  // -1, 0, 1 ��3�̒l���擾

            // �e�̈ʒu��ݒ� (shotpoint�̈ʒu���牡�ɂ��炷)
            Vector3 spawnPosition = shotPoint.transform.position + new Vector3(offsetX, 0, 0);
            shotRed.transform.position = spawnPosition;
        }
    }
}