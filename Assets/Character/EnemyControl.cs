using System.Collections;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //public

    [System.Serializable]
    public enum EnemyType
    {
        Wandering,  //放浪(攻撃無)
        Tracking,   //追跡(近距離)
        Firing,     //発射(遠距離)
    }
    public EnemyType type = EnemyType.Wandering;

    //Tyap
    public Wandering wandering = new Wandering(0f);
    public Tracking tracking  = new Tracking(0f, 0f, 0f);
    public Firing firing = new Firing(0f, 0f);

    //方向保存
    public float WaitTime = 1.5f;

    //コンポーネント
    public Rigidbody2D _rb = null;
    public GameObject _player = null;

    //private

    //各種速度
    private float AllSpeed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;

    //移動方向
    [Range(-1f, 1f)] private float XDirection = 0f;
    [Range(-1f, 1f)] private float YDirection = 0f;

    //フラグ
    bool IsDamage = false;

    //方向保存
    Vector2 SaveDir = default;

    private void Start()
    {

        //typeごとの設定
        switch(type)
        {
            case EnemyType.Wandering:

                AllSpeed = wandering._normalSpeed;

                break;

            case EnemyType.Tracking:

                AllSpeed = tracking._normalSpeed;

                break;

            case EnemyType.Firing:

                AllSpeed = firing._normalSpeed;

                break;
        }

        //初期移動設定
        XDirection = Random.Range(-1f, 1f);
        YDirection = Random.Range(-1f, 1f);
        XSpeed = XDirection * AllSpeed;
        YSpeed = YDirection * AllSpeed;

        _rb.velocity = new Vector2(XSpeed, YSpeed);
    }


    private void Update()
    {
        if(!IsDamage)
        {
            switch(type)
            {

                case EnemyType.Tracking:

                    tracking.Movement(_player.transform.position, this.transform.position, AllSpeed, _rb);

                    break;

                case EnemyType.Firing:

                    firing.Movement(_player.transform.position, this.transform, AllSpeed, _rb);

                    break;
            }
            //ダメージ判定後処理
            if(SaveDir != default){
                _rb.velocity = SaveDir * AllSpeed;
                SaveDir = default;
            }
        }
        //ダメージ
        else if(IsDamage)
        {
            StartCoroutine(ResetVelocity());
        }
    }

    //ダメージ(待機処理)
    private IEnumerator ResetVelocity()
    {
        _rb.velocity = default;

        yield return new WaitForSeconds(WaitTime);

        IsDamage = false;
        Debug.Log("Reset");
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //反射時の速度再設定
        _rb.velocity = _rb.velocity.normalized * AllSpeed;
    }

    //当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Slash"))
        {
            if(!IsDamage)
            {
                SaveDir = _rb.velocity.normalized;

                Debug.Log("Damage");
                Debug.Log(SaveDir);
            }
            IsDamage = true;
        }
    }
}



//放浪型
[System.Serializable]
public class Wandering
{
    [Header("基礎速度")]
    public float _normalSpeed = 0f;

    public Wandering(float normalSpeed)
    {
        this._normalSpeed = normalSpeed;
    }
}


//追跡型
[System.Serializable]
public class Tracking
{
    [Header("基礎速度")]
    public float _normalSpeed = 0f;

    [Header("追跡速度")]
    public float _trackingSpeed = 0f;

    [Header("追跡範囲")]
    public float _trackingRange = 0f;

    private Vector2 distance = default;

    public Tracking(float normalSpeed, float trackingSpeed, float trackingRange)
    {
        this._normalSpeed = normalSpeed;
        this._trackingSpeed = trackingSpeed;
        this._trackingRange = trackingRange;
    }


    public void Movement(Vector3 player, Vector3 thispos, float Speed, Rigidbody2D rigidbody2D)
    {
        distance = player - thispos;

        if(distance.magnitude <= Mathf.Abs(_trackingRange))
        {
            //追跡モード
            Speed = _trackingSpeed;
            rigidbody2D.velocity = distance.normalized * Speed;
        }
        else if(distance.magnitude > Mathf.Abs(_trackingRange))
        {
            //再設定
            Speed = _normalSpeed;
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * Speed;
        }
    }
}


[System.Serializable]
public class Firing
{
    [Header("基礎速度")]
    public float _normalSpeed = 0f;

    [Header("索敵範囲")]
    public float _serachRange = 0f;

    private Vector2 distance = default;

    public Firing (float normalSpeed, float serachRange)
    {
        this._normalSpeed = normalSpeed;
        this._serachRange = serachRange;
    }

    public void Movement(Vector3 player, Transform thisPos, float Speed, Rigidbody2D rigidbody2D)
    {
        distance = player - thisPos.position;

        if(distance.magnitude <= Mathf.Abs(_serachRange))
        {
            //発射モード
            Vector3 diff = (player - thisPos.position).normalized;
            thisPos.rotation = Quaternion.FromToRotation(Vector3.up, diff);
            Speed = 0f;
            rigidbody2D.velocity = -(rigidbody2D.velocity.normalized) * Speed;
        }
        else if(distance.magnitude > Mathf.Abs(_serachRange))
        {
            //速度再設定
            Speed = _normalSpeed;
            rigidbody2D.velocity = thisPos.up * Speed;
        }
    }
}