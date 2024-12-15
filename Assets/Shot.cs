using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]private PoolManager poolManager;

    [SerializeField] private float speed;

    // �e�̐i�s�������������i�f�t�H���g�͏�����j
    private Vector3 direction = Vector3.up;

    //new
    // �e�̉�]���x��������
    private float angularVelocity = 0f;

    // �e�̔��˂���̌o�ߎ��Ԃ�������
    private float lifeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //new
    //���˕����Ɖ�]���x��ݒ肷�郁�\�b�h
    public void SetDirectionDown(Vector3 newDirection,float angularVelocity = 0f)
    {

        // newDirection ���ꎞ�I�� Down �ɕύX
        newDirection = Vector3.down;

        // ���˕�����ݒ�
        direction = newDirection.normalized;


        //�o�ߎ��Ԃ����Z�b�g
        lifeTime = 0f;

        //��]���x��ݒ�
        this.angularVelocity = angularVelocity;
    }

    public void SetDirectionUp(Vector3 newDirection, float angularVelocity = 0f)
    {
        //���˕�����ݒ�
        direction = newDirection.normalized;

        //�o�ߎ��Ԃ����Z�b�g
        lifeTime = 0f;

        //��]���x��ݒ�
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
        // �t���[�����ƂɌo�ߎ��Ԃ����Z
        lifeTime += Time.deltaTime;

        // ��]����p�x���v�Z�i�p���x �~ �o�ߎ��ԁj
        var angle = angularVelocity * lifeTime;
        
        // Z������ɉ�]����N�H�[�^�j�I�����쐬
        var rot = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

        // ��]��K�p�����i�s�������v�Z
        var dir = rot * direction;

        //�e���ړ����鑬��
        transform.Translate(dir * speed * Time.deltaTime);
    }
}