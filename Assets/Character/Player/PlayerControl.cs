using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("�e��X�e�[�^�X")]
    [SerializeField] private float _NormalSpeed = 6f;



    private float Speed = 0f;
    private float XSpeed = 0f;
    private float YSpeed = 0f;


    [Header("�R���|�[�l���g")]
    [SerializeField] private Rigidbody2D _rb = null;

    private void Start()
    {
        Speed = _NormalSpeed;
    }

    private void LateUpdate()
    {
        XSpeed = Input.GetAxis("Horizontal") * Speed;
        YSpeed = Input.GetAxis("Vertical") * Speed;
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(XSpeed, YSpeed);
    }
}
