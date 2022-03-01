using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("各種ステータス")]
    [SerializeField]private float _NormalSpeed = 6f;

    [SerializeField]private float _SlashSpeed = 6f;

    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;


    bool IsAttack = false;

    bool IsSlash = false;
    bool IsGo = false;

    [Header("コンポーネント")]
    private Rigidbody2D _rb = null;
    private BoxCollider2D _bc = null;

    [SerializeField]private Animator _slashAnim = null;

    [SerializeField]private Animator _slashAreaAnim = null;
    [SerializeField]private BoxCollider2D _slashCollider = null;


    private Vector3 cameraForward = default;
    private Vector3 moveForward = default;

    private Vector3 SavePos = default;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _bc = gameObject.GetComponent<BoxCollider2D>();

        Speed = _NormalSpeed;
    }

    private void Update()
    {
        /*off:通常、on:居合移動時の操作不能*/
        if(!IsGo)
        {
            //移動
            if (!IsAttack)
            {
                XSpeed = Input.GetAxis("Horizontal");
                YSpeed = Input.GetAxis("Vertical");
            }
            else if (IsAttack)
            { 
                XSpeed = 0f;
                YSpeed = 0f;
            }

            //攻撃居合
            if (!IsSlash && Input.GetKeyDown(KeyCode.Backspace))
            {
                IsSlash = true;
                Speed /= 2f;
            }
            else if (IsSlash && Input.GetKeyUp(KeyCode.Backspace))
            {
                IsGo = true;
                _bc.isTrigger = true;
                _slashCollider.enabled = true;

                SavePos = transform.position + Vector3.Scale(transform.up, new Vector3(_SlashSpeed, _SlashSpeed, 0));
                Speed *= 2f;

                IsSlash = false;
            }
            //攻撃
            if (!IsAttack && Input.GetKeyDown(KeyCode.Return))   IsAttack = true;

        }
        else if (IsGo)
        { 
            transform.position = Vector2.MoveTowards(transform.position, SavePos, 64f * Time.deltaTime);

            if (transform.position == SavePos)
            { 
                SavePos = default;

                _bc.isTrigger = false;
                _slashCollider.enabled = false;

                IsGo = false;
            }

        }
    }

    private void LateUpdate()
    {

        //アニメーション処理
        _slashAnim.SetBool("IsAttack", IsAttack);
        _slashAreaAnim.SetBool("IsAttack", IsSlash);
        //アニメーション終了判定
        if (_slashAnim.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
        { 
            IsAttack = false;
        }
    }

    private void FixedUpdate()
    {
        //カメラから正面方向を取得
        cameraForward = Vector3.Scale(Camera.main.transform.up, new Vector3(1, 1, 0)).normalized;
        //キャラの前方方向を取得
        moveForward = cameraForward * YSpeed + Camera.main.transform.right * XSpeed;
 
        _rb.velocity = moveForward * Speed + new Vector3(0, 0, 0);
 
        //移動時のみ回転を可能にする
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, moveForward);
        }
    }


}
