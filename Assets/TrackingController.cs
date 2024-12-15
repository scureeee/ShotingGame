using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TrackingController : MonoBehaviour
{
    private PoolManager poolManager;

    // �^�[�Q�b�g���C���X�y�N�^�[����ݒ�\��
    private GameObject target;

    private float DeleteTime;

    // �ړ����x���C���X�y�N�^�[�Œ����\��
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

        // �^�[�Q�b�g�����ݒ�̏ꍇ�͏������X�L�b�v
        if (target == null) return;

        // �^�[�Q�b�g�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position, // target �� position ���g�p
            speed * Time.deltaTime);

        if (DeleteTime >= 2.0f)
        {
            poolManager.DelShot(gameObject);

            DeleteTime = 0f;
        }
    }
}