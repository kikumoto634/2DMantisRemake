using System.Collections;
using UnityEngine;


public class Test_EnemyControl : MonoBehaviour
{
    public enum EnemyType
    { 
        Wandering,  //úQ(U³)
        Tracking,   //ÇÕ(ß£)
        Firing      //­Ë(£)
    };
    public EnemyType CurrentEnemyType = EnemyType.Wandering;


    [Header("îbXe[^X")]
    [SerializeField]private float _NormalSpeed = 5f;   //úQAÇÕ
    [SerializeField]private float _SlowSpeed = 2.5f;    //­Ë

    //¬x
    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;

    //Ú®ûü
    [Range (-1f, 1f)] private float XDirection = 0;
    [Range (-1f, 1f)] private float YDirection = 0;


    [Header("TrackingXe[^X")]
    [SerializeField]private float _TracSpeed = 8f;

    //ÍÍ£
    private Vector2 distance = default;


    [Header("R|[lg")]
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


        //úÚ®Ýè
        XDirection = Random.Range(-1f, 1f);
        YDirection = Random.Range(-1f, 1f);
        XSpeed = XDirection * Speed;
        YSpeed = YDirection * Speed;

        _rb.velocity = new Vector2(XSpeed, YSpeed);
    }

    private void LateUpdate()
    {
        //£ªè
        distance = _player.transform.position - this.transform.position;
        //if (!IsDamage)
        //{
            switch (CurrentEnemyType)
            {
                case EnemyType.Tracking:

                    if (distance.magnitude <= Mathf.Abs(8f))
                    {
                        //ÇÕ[hÚs
                        Speed = _TracSpeed;
                        _rb.velocity = distance.normalized * Speed;
                    }
                    else if (distance.magnitude > Mathf.Abs(8f))
                    {
                        //¬xÌÄÝè
                        Speed = _NormalSpeed;
                        _rb.velocity = _rb.velocity.normalized * Speed;
                    }

                    break;

                case EnemyType.Firing:

                    if (distance.magnitude <= Mathf.Abs(10f))
                    {
                        //­Ë[h
                        Vector3 diff = (_player.transform.position - this.transform.position).normalized;
                        this.transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
                        Speed = 0f;
                        _rb.velocity = -(_rb.velocity.normalized) * Speed;
                    }
                    else if (distance.magnitude > Mathf.Abs(10f))
                    {
                        //¬xÌÄÝè
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
        //½ËÉ¬xÌÄÝè
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


    //½è»è
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slash"))
        { 
            Debug.Log(this.gameObject.name+":_[W");

            //SaveDirection = _rb.velocity.normalized;
            //SaveSpeed = Speed;
            //IsDamage = true;

            _rb.AddForce(collision.gameObject.transform.up * 100f, ForceMode2D.Force);
            Debug.Log(collision.gameObject.name);
        }
    }
}
