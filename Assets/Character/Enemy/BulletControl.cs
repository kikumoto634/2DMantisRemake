using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private const int SPEED = 20; //弾の速さ
    //private float _screenTop; // 画面の一番上のy座標。画面外かどうかの判定に使用
 
    private Rigidbody2D _rb;
    private Transform _tf;
 
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tf = this.transform;
 
		// 画面の一番上のy座標を取得
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
