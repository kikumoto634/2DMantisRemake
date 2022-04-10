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

    //体力
    public int _hp = 2;

    //方向保存
    public float WaitTime = 1.5f;
    public float NockSpeed = 1.5f;

    //コンポーネント
    public Rigidbody2D _rb = null;
    private GameObject player = null;
    public GameObject _effect = null;

    SpriteRenderer Sp = null;
    CircleCollider2D Cc2D = null;

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
    Vector2 NockBckDir = default;

    //クラス
    RandomCreate randomCreate = default;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Sp = this.gameObject.GetComponent<SpriteRenderer>();
        Cc2D = this.gameObject.GetComponent<CircleCollider2D>();

        randomCreate = new RandomCreate();

        _effect.SetActive(false);

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

                firing.Cash(this.transform);

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
            RotationChange(this.transform, _rb);

            switch(type)
            {

                case EnemyType.Tracking:

                    tracking.Movement(player.transform.position, this.transform.position, AllSpeed, _rb);

                    break;

                case EnemyType.Firing:

                    firing.Movement(player.transform.position, this.transform, AllSpeed, _rb);

                    if(firing.ShotIterative())
                    {
                        firing.Shot();
                        audioManager("Shot");
                    }

                    break;
            }
        }
        //ダメージ
        else if (IsDamage)
        {
            _rb.velocity = default;
            _rb.velocity = -(NockBckDir) * NockSpeed;
            Invoke(nameof(ResetVelocity), WaitTime);
        }
    }

    private void LateUpdate()
    {
        if(_hp > 0) return ;
        
        //死亡
        if(_effect.activeSelf == true) return ;
        audioManager("Dead");
        _effect.SetActive(true);
        Sp.enabled = false;
        Cc2D.isTrigger = true;

        //リスポーン
        if(Sp.enabled == true) return ;
        this.gameObject.transform.position = randomCreate.Create();
        _hp = 2;
        Sp.enabled = true;
        Cc2D.isTrigger = false;
        _effect.SetActive(false);
    }

    //ダメージ(待機処理)
    void ResetVelocity()
    { 
        _effect.SetActive(false);

        IsDamage = false;
        _rb.velocity = SaveDir * AllSpeed;
        SaveDir = default;
        NockBckDir = default;

        CancelInvoke();
    }

    void audioManager(string name)
    {
        AudioManager.instance.Play(name);
    }

    void RotationChange(Transform thisPos, Rigidbody2D rigidbody2D)
    {
        thisPos.rotation = Quaternion.FromToRotation(Vector3.up, rigidbody2D.velocity.normalized);
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
                IsDamage = true;

                audioManager("Damage");

                _hp = _hp - 1;

                SaveDir = _rb.velocity.normalized;
                NockBckDir = (player.transform.position - this.transform.position).normalized;

                _effect.SetActive(true);
            }
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

    //射撃
    private float shotDelay = 0.5f;
    private Transform tf = default;
    private BulletPool pool = null;

    private float timeElapsed = 0.0f;

    public Firing (float normalSpeed, float serachRange)
    {
        this._normalSpeed = normalSpeed;
        this._serachRange = serachRange;
    }

    public void Cash(Transform thistrans)
    {
        tf = thistrans;
        pool = GameObject.Find("Pool").GetComponent<BulletPool>();
    }

    public void Shot()
    {
        var bullet = pool.GetBurret();
        bullet.transform.localPosition = tf.position;
        bullet.transform.localRotation = tf.rotation;
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

            //開始
            timeElapsed += Time.deltaTime;
        }
        else if(distance.magnitude > Mathf.Abs(_serachRange))
        {
            //速度再設定
            Speed = _normalSpeed;
            rigidbody2D.velocity = thisPos.up * Speed;

            //リセット
            timeElapsed = 0.0f;
        }
    }

    //発射間の計測
    public bool ShotIterative()
    {
        if(timeElapsed >= shotDelay)
        {
            timeElapsed = 0.0f;
            return true;
        }

        return false;
    }
}