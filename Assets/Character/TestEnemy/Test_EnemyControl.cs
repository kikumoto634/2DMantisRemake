using System.Collections;
using UnityEngine;


public class Test_EnemyControl : MonoBehaviour
{
    public enum EnemyType
    { 
        Wandering,  //���Q(�U����)
        Tracking,   //�ǐ�(�ߋ���)
        Firing      //����(������)
    };
    public EnemyType CurrentEnemyType = EnemyType.Wandering;


    [Header("��b�X�e�[�^�X")]
    [SerializeField]private float _NormalSpeed = 5f;   //���Q�A�ǐ�
    [SerializeField]private float _SlowSpeed = 2.5f;    //����

    //���x
    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;

    //�ړ�����
    [Range (-1f, 1f)] private float XDirection = 0;
    [Range (-1f, 1f)] private float YDirection = 0;


    [Header("Tracking�X�e�[�^�X")]
    [SerializeField]private float _TracSpeed = 8f;

    //�͈͋���
    private Vector2 distance = default;


    [Header("�R���|�[�l���g")]
    [SerializeField]private Rigidbody2D _rb = null;
    [SerializeField]private GameObject _player = null;



    //bool IsDamage = false;
    //Vector2 SaveDirection = default;
    //float SaveSpeed = 0f;

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
        distance = _player.transform.position - this.transform.position;
        //if (!IsDamage)
        //{
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

                    if (distance.magnitude <= Mathf.Abs(10f))
                    {
                        //���˃��[�h
                        Vector3 diff = (_player.transform.position - this.transform.position).normalized;
                        this.transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
                        Speed = 0f;
                        _rb.velocity = -(_rb.velocity.normalized) * Speed;
                    }
                    else if (distance.magnitude > Mathf.Abs(10f))
                    {
                        //���x�̍Đݒ�
                        Speed = _SlowSpeed;
                        _rb.velocity = this.transform.up * Speed;
                    }

                    break;
            }
        //}
        //else if (IsDamage)
        //{ 
        //    StartCoroutine(ResetVelocity());
        //}
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���ˎ��ɑ��x�̍Đݒ�
        _rb.velocity = _rb.velocity.normalized * Speed;
    }


//private IEnumerator ResetVelocity()
//{
//    Debug.Log("reset");

//    Speed = 0f;

//    yield return new WaitForSeconds(1.5f);

//    Speed = SaveSpeed;
//    _rb.velocity = SaveDirection * Speed;
//    IsDamage = false;
//}


    //�����蔻��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slash"))
        { 
            Debug.Log(this.gameObject.name+":�_���[�W");

            //SaveDirection = _rb.velocity.normalized;
            //SaveSpeed = Speed;
            //IsDamage = true;

            _rb.AddForce(collision.gameObject.transform.up * 100f, ForceMode2D.Force);
            Debug.Log(collision.gameObject.name);
        }
    }
}
