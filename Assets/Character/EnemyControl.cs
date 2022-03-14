using System.Collections;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    //public
    [System.Serializable]
    public enum EnemyType
    {
        Wandering,  //���Q(�U����)
        Tracking,   //�ǐ�(�ߋ���)
        Firing,     //����(������)
    }
    public EnemyType type = EnemyType.Wandering;

    //Tyap
    public Wandering wandering = new Wandering(0f);
    public Tracking tracking  = new Tracking(0f, 0f, 0f);
    public Firing firing = new Firing(0f, 0f);

    //�̗�
    public int _hp = 2;

    //�����ۑ�
    public float WaitTime = 1.5f;
    public float NockSpeed = 1.5f;

    //�R���|�[�l���g
    public Rigidbody2D _rb = null;
    public GameObject _player = null;
    public GameObject _effect = null;

    SpriteRenderer Sp = null;
    CircleCollider2D Cc2D = null;

    //private

    //�e�푬�x
    private float AllSpeed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;

    //�ړ�����
    [Range(-1f, 1f)] private float XDirection = 0f;
    [Range(-1f, 1f)] private float YDirection = 0f;

    //�t���O
    bool IsDamage = false;

    //�����ۑ�
    Vector2 SaveDir = default;
    Vector2 NockBckDir = default;

    //�N���X
    RandomCreate randomCreate = default;

    private void Start()
    {
        Sp = this.gameObject.GetComponent<SpriteRenderer>();
        Cc2D = this.gameObject.GetComponent<CircleCollider2D>();

        randomCreate = new RandomCreate();

        _effect.SetActive(false);

        //type���Ƃ̐ݒ�
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
                StartCoroutine(firing.ShotBullet());

                break;
        }

        //�����ړ��ݒ�
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
        }
        //�_���[�W
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
        
        //���S
        if(_effect.activeSelf == true) return ;
        AudioManager.instance.Play("Dead");
        _effect.SetActive(true);
        Sp.enabled = false;
        Cc2D.enabled = false;

        //���X�|�[��
        if(Sp.enabled == true) return ;
        this.gameObject.transform.position = randomCreate.Create();
        _hp = 2;
        Sp.enabled = true;
        Cc2D.enabled = true;
        _effect.SetActive(false);
    }

    //�_���[�W(�ҋ@����)
    void ResetVelocity()
    { 
        _effect.SetActive(false);

        IsDamage = false;
        _rb.velocity = SaveDir * AllSpeed;
        SaveDir = default;
        NockBckDir = default;

        CancelInvoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���ˎ��̑��x�Đݒ�
        _rb.velocity = _rb.velocity.normalized * AllSpeed;
    }

    //�����蔻��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Slash"))
        {
            if(!IsDamage)
            {
                IsDamage = true;

                AudioManager.instance.Play("Damage");

                _hp = _hp - 1;

                SaveDir = _rb.velocity.normalized;
                NockBckDir = (_player.transform.position - this.transform.position).normalized;

                _effect.SetActive(true);
            }
        }
    }
}



//���Q�^
[System.Serializable]
public class Wandering
{
    [Header("��b���x")]
    public float _normalSpeed = 0f;

    public Wandering(float normalSpeed)
    {
        this._normalSpeed = normalSpeed;
    }
}


//�ǐՌ^
[System.Serializable]
public class Tracking
{
    [Header("��b���x")]
    public float _normalSpeed = 0f;

    [Header("�ǐՑ��x")]
    public float _trackingSpeed = 0f;

    [Header("�ǐՔ͈�")]
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
            //�ǐՃ��[�h
            Speed = _trackingSpeed;
            rigidbody2D.velocity = distance.normalized * Speed;
        }
        else if(distance.magnitude > Mathf.Abs(_trackingRange))
        {
            //�Đݒ�
            Speed = _normalSpeed;
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * Speed;
        }
    }
}


[System.Serializable]
public class Firing
{
    [Header("��b���x")]
    public float _normalSpeed = 0f;

    [Header("���G�͈�")]
    public float _serachRange = 0f;

    private Vector2 distance = default;


    //�ˌ�
    private float shotDelay = 1.0f;
    private Transform tf = default;
    private BulletPool pool = null;

    public Firing (float normalSpeed, float serachRange)
    {
        this._normalSpeed = normalSpeed;
        this._serachRange = serachRange;
    }

    public void Cash(Transform thistrans)
    {
        tf = thistrans;
        pool = GameObject.Find("Pool").GetComponent<BulletPool>();
        //����
    }

    public IEnumerator ShotBullet()
    {
        while(true)
        {
            Shot();

            yield return new WaitForSeconds(shotDelay);
        }
    }

    private void Shot()
    {
        var bullet = pool.GetBurret();
        bullet.transform.localPosition = tf.position;
    }

    public void Movement(Vector3 player, Transform thisPos, float Speed, Rigidbody2D rigidbody2D)
    {
        distance = player - thisPos.position;

        if(distance.magnitude <= Mathf.Abs(_serachRange))
        {
            //���˃��[�h
            Vector3 diff = (player - thisPos.position).normalized;
            thisPos.rotation = Quaternion.FromToRotation(Vector3.up, diff);
            Speed = 0f;
            rigidbody2D.velocity = -(rigidbody2D.velocity.normalized) * Speed;
        }
        else if(distance.magnitude > Mathf.Abs(_serachRange))
        {
            //���x�Đݒ�
            Speed = _normalSpeed;
            rigidbody2D.velocity = thisPos.up * Speed;
        }
    }
}