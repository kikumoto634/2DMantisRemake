using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("�e��X�e�[�^�X")]
    public float _NormalSpeed = 6f;



    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;


    bool IsAttack = false;

    [Header("�R���|�[�l���g")]
    public Rigidbody2D _rb = null;
    public GameObject _slash = null;
    public Animator _slashAnim = null;


    private Vector3 cameraForward = default;
    private Vector3 moveForward = default;

    private void Start()
    {
        Speed = _NormalSpeed;
    }

    private void LateUpdate()
    {
        XSpeed = Input.GetAxis("Horizontal");
        YSpeed = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Return))   IsAttack = true;
        

        //�A�j���[�V��������
        _slashAnim.SetBool("IsAttack", IsAttack);
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
