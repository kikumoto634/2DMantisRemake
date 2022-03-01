using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("�e��X�e�[�^�X")]
    public float _NormalSpeed = 6f;

    public float _SlashSpeed = 6f;

    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;


    bool IsAttack = false;
    bool IsSlash = false;

    [Header("�R���|�[�l���g")]
    public Rigidbody2D _rb = null;

    public Animator _slashAnim = null;

    public Animator _slashAreaAnim = null;
    public BoxCollider2D _slashCollider = null;


    private Vector3 cameraForward = default;
    private Vector3 moveForward = default;

    private void Start()
    {
        Speed = _NormalSpeed;
    }

    private void Update()
    {
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


        if(!IsSlash && Input.GetKeyDown(KeyCode.Backspace))
        {
            IsSlash = true;
            Speed /= 2f;
        }
        if(IsSlash && Input.GetKeyUp(KeyCode.Backspace))
        {
            _slashCollider.enabled = true;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.Scale(transform.up, new Vector3(_SlashSpeed, _SlashSpeed, 0)), _SlashSpeed);
            Speed *= 2f;
            IsSlash = false;
        }
        if(!IsAttack && Input.GetKeyDown(KeyCode.Return))   IsAttack = true;

    }

    private void LateUpdate()
    {

        if(_slashCollider.enabled)  
        {
            Debug.Log("Off");
            _slashCollider.enabled = false;
        }

        //�A�j���[�V��������
        _slashAnim.SetBool("IsAttack", IsAttack);
        _slashAreaAnim.SetBool("IsAttack", IsSlash);
        //�A�j���[�V�����I������
        if (_slashAnim.GetCurrentAnimatorStateInfo(0).IsName("Slash"))
        { 
            IsAttack = false;
        }
    }

    private void FixedUpdate()
    {
        cameraForward = Vector3.Scale(Camera.main.transform.up, new Vector3(1, 1, 0)).normalized;
 
        moveForward = cameraForward * YSpeed + Camera.main.transform.right * XSpeed;
 
        _rb.velocity = moveForward * Speed + new Vector3(0, 0, 0);
 
        if (moveForward != Vector3.zero) {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, moveForward);
        }
    }


}
