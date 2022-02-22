using UnityEngine;

public class Test_EnemyControl : MonoBehaviour
{
    [SerializeField] private enum EnemyType
    { 
        Wandering,  //放浪(攻撃無)
        Tracking,   //追跡(近距離)
        Firing      //発射(遠距離)
    };
    [SerializeField] private EnemyType CurrentEnemyType = EnemyType.Wandering;


    [Header("基礎ステータス")]
    [SerializeField] private float _NormalSpeed = 5f;   //放浪、追跡
    [SerializeField]private float _SlowSpeed = 2.5f;    //発射

    //速度
    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;

    //移動方向
    [Range (-1f, 1f)] private float XDirection = 0;
    [Range (-1f, 1f)] private float YDirection = 0;


    [Header("Trackingステータス")]
    [SerializeField] private float _TracSpeed = 8f;

    //範囲距離
    private Vector2 distance = default;


    [Header("コンポーネント")]
    [SerializeField] private Rigidbody2D _rb = null;
    [SerializeField] private GameObject _player = null;

    [Header("キャッシュ")]
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

        //初期移動設定
        XDirection = Random.Range(-1f, 1f);
        YDirection = Random.Range(-1f, 1f);
        XSpeed = XDirection * Speed;
        YSpeed = YDirection * Speed;

        _rb.velocity = new Vector2(XSpeed, YSpeed);
    }

    private void LateUpdate()
    {
        //距離測定
        distance = playerTransform.position - thisTransform.position;

        switch (CurrentEnemyType)
        { 
            case EnemyType.Tracking:

                if (distance.magnitude <= Mathf.Abs(8f))
                {
                    //追跡モード移行
                    Speed = _TracSpeed;
                    _rb.velocity = distance.normalized * Speed;
                }
                else if (distance.magnitude > Mathf.Abs(8f))
                { 
                    //速度の再設定
                    Speed = _NormalSpeed;
                    _rb.velocity = _rb.velocity.normalized * Speed;
                }

                break;

            case EnemyType.Firing:

                if(distance.magnitude <= Mathf.Abs(10f))
                {
                    //発射モード
                    Vector3 diff = (playerTransform.position - thisTransform.position).normalized;
                    thisTransform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
                    Speed = 0f;
                    _rb.velocity = -(_rb.velocity.normalized) * Speed;
                }
                else if (distance.magnitude > Mathf.Abs(10f))
                { 
                    //速度の再設定
                    Speed = _SlowSpeed;
                    _rb.velocity = thisTransform.up * Speed;
                }

                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //反射時に速度の再設定
        _rb.velocity = _rb.velocity.normalized * Speed;
    }

}
