using UnityEngine;

public class Test_EnemyControl : MonoBehaviour
{
    [SerializeField] private enum EnemyType
    { 
        Wandering,  //���Q
        Tracking    //�ǐ�
    };
    private EnemyType CurrentEnemyType = 0;


    [Header("�e��X�e�[�^�X")]
    [SerializeField] private float _NormalSpeed = 5f;


    //���x
    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;


    //�ړ�����
    [Range (-1f, 1f)] private float XDirection = 0;
    [Range (-1f, 1f)] private float YDirection = 0;


    [Header("�R���|�[�l���g")]
    [SerializeField] private Rigidbody2D _rb = null;

    private void Start()
    {
        Speed = _NormalSpeed;

        switch (CurrentEnemyType)
        { 
            case EnemyType.Wandering:

                XDirection = Random.Range(-1f, 1f);
                YDirection = Random.Range(-1f, 1f);

                XSpeed = XDirection * Speed;
                YSpeed = YDirection * Speed;

                _rb.velocity = new Vector2(XSpeed, YSpeed);

                break;

            case EnemyType.Tracking:

                break;
        }
    }

    private void LateUpdate()
    {
        switch (CurrentEnemyType)
        { 
            case EnemyType.Wandering:

                

                break;

            case EnemyType.Tracking:

                break;
        }
    }

    private void FixedUpdate()
    {
        //_rb.velocity = new Vector2(XSpeed, YSpeed);
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("DamageArea"))
    //    { 
    //        XDirection *= -1f;
    //        YDirection *= -1f;
    //    }
    //}

}
