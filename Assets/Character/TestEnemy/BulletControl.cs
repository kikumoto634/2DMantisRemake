using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private const int SPEED = 20; //�e�̑���
    //private float _screenTop; // ��ʂ̈�ԏ��y���W�B��ʊO���ǂ����̔���Ɏg�p
 
    private Rigidbody2D _rb;
    private Transform _tf;
 
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tf = this.transform;
 
		// ��ʂ̈�ԏ��y���W���擾
		//_screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
    }
    
    private void Update() 
    {
		if(_rb.simulated == false)
			return;

        _rb.velocity = _tf.up.normalized * SPEED;

        if(Mathf.Abs(_tf.position.magnitude) > 50)
        {
            _rb.simulated = false;
            _tf.position = new Vector2(100, 100);
        }
    }
}
