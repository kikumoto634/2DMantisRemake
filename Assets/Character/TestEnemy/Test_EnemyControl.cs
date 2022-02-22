using UnityEngine;

public class Test_EnemyControl : MonoBehaviour
{
    [SerializeField] private enum EnemyType
    { 
        Wandering,  //���Q(�U����)
        Tracking,   //�ǐ�(�ߋ���)
        Firing      //����(������)
    };
    [SerializeField] private EnemyType CurrentEnemyType = EnemyType.Wandering;


    [Header("��b�X�e�[�^�X")]
    [SerializeField] private float _NormalSpeed = 5f;   //���Q�A�ǐ�
    [SerializeField]private float _SlowSpeed = 2.5f;    //����

    //���x
    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;

    //�ړ�����
    [Range (-1f, 1f)] private float XDirection = 0;
    [Range (-1f, 1f)] private float YDirection = 0;


    [Header("Tracking�X�e�[�^�X")]
    [SerializeField] private float _TracSpeed = 8f;

    //�͈͋���
    private Vector2 distance = default;


    [Header("�R���|�[�l���g")]
    [SerializeField] private Rigidbody2D _rb = null;
    [SerializeField] private GameObject _player = null;

    [Header("�L���b�V��")]
    private Transform thisTransform = default;
    private Transform playerTransform = default;

    private void Start()
    {
        switch (CurrentEnemyType)
        {
            case EnemyType.Firing:
                Speed = _SlowSpeed;

                break;

            default:
                Speed = _NormalSpeed;

                break;
        }

        thisTransform = this.transform;
        playerTransform = _player.transform;

        //�����ړ��ݒ�
        XDirection = Random.Range(-1f, 1f);
        YDirection = Random.Range(-1f, 1f);
        XSpeed = XDirection * Speed;
        YSpeed = YDirection * Speed;

        _rb.velocity = new Vector2(XSpeed, YSpeed);
    }

    private void LateUpdate()
    {
        //��������
        distance = playerTransform.position - thisTransform.position;

        switch (CurrentEnemyType)
        { 
            case EnemyType.Tracking:

                if (distance.magnitude <= Mathf.Abs(8f))
                {
                    //�ǐՃ��[�h�ڍs
                    Speed = _TracSpeed;
                    _rb.velocity = distance.normalized * Speed;
                }
                else if (distance.magnitude > Mathf.Abs(8f))
                { 
                    //���x�̍Đݒ�
                    Speed = _NormalSpeed;
                    _rb.velocity = _rb.velocity.normalized * Speed;
                }

                break;

            case EnemyType.Firing:

                if(distance.magnitude <= Mathf.Abs(10f))
                {
                    //���˃��[�h
                    Vector3 diff = (playerTransform.position - thisTransform.position).normalized;
                    thisTransform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
                    Speed = 0f;
                    _rb.velocity = -(_rb.velocity.normalized) * Speed;
                }
                else if (distance.magnitude > Mathf.Abs(10f))
                { 
                    //���x�̍Đݒ�
                    Speed = _SlowSpeed;
                    _rb.velocity = thisTransform.up * Speed;
                }

                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���ˎ��ɑ��x�̍Đݒ�
        _rb.velocity = _rb.velocity.normalized * Speed;
    }

}
