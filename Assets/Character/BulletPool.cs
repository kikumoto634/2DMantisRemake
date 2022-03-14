using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField]
	private GameObject _poolObj; // プールするオブジェクト。ここでは弾
	private List<GameObject> _poolObjList; // 生成した弾用のリスト。このリストの中から未使用のものを探したりする
	private const int MAXCOUNT = 10; // 最初に生成する弾の数 
 
	void Awake()
	{
		CreatePool();
	}
 
    // 最初にある程度の数、オブジェクトを作成してプールしておく処理
    private void CreatePool()
	{
        _poolObjList = new List<GameObject>();
        for (int i = 0; i < MAXCOUNT; i++) 
		{
            var newObj = CreateNewBurret(); // 弾を生成して
            newObj.GetComponent<Rigidbody2D>().simulated = false; // 物理演算を切って(=未使用にして)
            _poolObjList.Add(newObj); // リストに保存しておく
        }
    }
 
	// 未使用の弾を探して返す処理
    public GameObject GetBurret()
	{
        // 使用中でないものを探して返す
        foreach (var obj in _poolObjList)
		{
			var objrb = obj.GetComponent<Rigidbody2D>();
            if (objrb.simulated  == false) 
			{
				objrb.simulated = true;
                return obj;
            }
        }
 
        // 全て使用中だったら新しく作り、リストに追加してから返す
        var newObj = CreateNewBurret();
        _poolObjList.Add(newObj);
 
		newObj.GetComponent<Rigidbody2D>().simulated = true;
        return newObj;
    }
 
	// 新しく弾を作成する処理
    private GameObject CreateNewBurret()
    {
		var pos = new Vector2(100,100); // 画面外であればどこでもOK
        var newObj = Instantiate(_poolObj, pos, Quaternion.identity); // 弾を生成しておいて
        newObj.name = _poolObj.name + (_poolObjList.Count + 1); // 名前を連番でつけてから
 
        return newObj; // 返す
    }
}
