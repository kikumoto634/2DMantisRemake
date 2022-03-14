using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private const int SPEED = 10; //�e�̑���
    private float _screenTop; // ��ʂ̈�ԏ��y���W�B��ʊO���ǂ����̔���Ɏg�p
 
    private Rigidbody2D _rb;
    private Transform _tf;
 
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tf = this.transform;
 
		// ��ʂ̈�ԏ��y���W���擾
		_screenTop = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
		// �e����Ɉړ�������
		_rb.velocity = _tf.up.normalized * SPEED;
    }
    
    private void Update() 
    {
		// Rigidbody2D��simulated��false(�e���g���Ă��Ȃ����)�ł���Ή������Ȃ�
		if(_rb.simulated == false)
			return;
 
		// ���������Rigidbody2D��simulated��true�̏ꍇ(=�e�������Ă���ꍇ)
		// ��ʊO�ɒe���o�Ă�����Rigidbody2D��simulated��false�ɂ��ĕ������Z���~�߂�(�e���X�g�b�v����)
		// �{�P���Ă���̂͗]�T�������Ă��邾���ł��B
        if(_tf.position.y > _screenTop + 1)
        {
           _rb.simulated = false;
        }
    }
}
