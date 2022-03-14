using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField]
	private GameObject _poolObj; // �v�[������I�u�W�F�N�g�B�����ł͒e
	private List<GameObject> _poolObjList; // ���������e�p�̃��X�g�B���̃��X�g�̒����疢�g�p�̂��̂�T�����肷��
	private const int MAXCOUNT = 10; // �ŏ��ɐ�������e�̐� 
 
	void Awake()
	{
		CreatePool();
	}
 
    // �ŏ��ɂ�����x�̐��A�I�u�W�F�N�g���쐬���ăv�[�����Ă�������
    private void CreatePool()
	{
        _poolObjList = new List<GameObject>();
        for (int i = 0; i < MAXCOUNT; i++) 
		{
            var newObj = CreateNewBurret(); // �e�𐶐�����
            newObj.GetComponent<Rigidbody2D>().simulated = false; // �������Z��؂���(=���g�p�ɂ���)
            _poolObjList.Add(newObj); // ���X�g�ɕۑ����Ă���
        }
    }
 
	// ���g�p�̒e��T���ĕԂ�����
    public GameObject GetBurret()
	{
        // �g�p���łȂ����̂�T���ĕԂ�
        foreach (var obj in _poolObjList)
		{
			var objrb = obj.GetComponent<Rigidbody2D>();
            if (objrb.simulated  == false) 
			{
				objrb.simulated = true;
                return obj;
            }
        }
 
        // �S�Ďg�p����������V�������A���X�g�ɒǉ����Ă���Ԃ�
        var newObj = CreateNewBurret();
        _poolObjList.Add(newObj);
 
		newObj.GetComponent<Rigidbody2D>().simulated = true;
        return newObj;
    }
 
	// �V�����e���쐬���鏈��
    private GameObject CreateNewBurret()
    {
		var pos = new Vector2(100,100); // ��ʊO�ł���΂ǂ��ł�OK
        var newObj = Instantiate(_poolObj, pos, Quaternion.identity); // �e�𐶐����Ă�����
        newObj.name = _poolObj.name + (_poolObjList.Count + 1); // ���O��A�Ԃł��Ă���
 
        return newObj; // �Ԃ�
    }
}
