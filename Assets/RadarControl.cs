using UnityEngine;
using System.Collections.Generic;

public class RadarControl : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject Prefab1 = null;
    [SerializeField] private GameObject Prefab2 = null;
    private List<GameObject> EnemyList = null;
    private List<GameObject> TargetList = null;
    private const int IniNum = 3;

    private GameObject Player = null;
    private GameObject RadarCenter = null;
    private GameObject EnemyHouse = null;

    private RandomCreate randam = null;

    private float r = 12f;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        randam = new RandomCreate();
        RadarCenter = GameObject.FindGameObjectWithTag("RadarTarget");
        EnemyHouse = GameObject.FindGameObjectWithTag("EnemyHouse");

        CreatePool();
    }

    private void Update()
    {
        Debug.Log("ëçêî(ìG)ÅF"+EnemyList.Count);
        Debug.Log("ëçêî(çÙ)ÅF"+TargetList.Count);

    
        for(int i = 0; i < EnemyList.Count; i++)
        {
            if ((EnemyList[i].transform.position - Player.transform.position).magnitude <= Mathf.Abs(30f))
            {
                TargetList[i].transform.localPosition = (EnemyList[i].transform.position - Player.transform.position) * r;
            }
            else
            {
                TargetList[i].transform.localPosition = new Vector2(500, 500);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateObj();
        }
    }


    //ééÇµä÷êî
    private void CreatePool()
    {
        EnemyList = new List<GameObject>();
        TargetList = new List<GameObject>();
        for(int i = 0; i < IniNum; i++)
        {
            var newObj1 = CreateNewEnemy();
            var newObj2 = CreateNewTarget();
            EnemyList.Add(newObj1);
            TargetList.Add(newObj2);
        }
    }

    private GameObject CreateNewEnemy()
    {
        var pos = randam.Create();
        var newObj = Instantiate(Prefab1, pos, Quaternion.identity);
        newObj.name = Prefab1.name + (EnemyList.Count + 1);
        newObj.transform.parent = EnemyHouse.transform;
        return newObj;
    }

    private GameObject CreateNewTarget()
    {
        var pos = new Vector2(0, 0);
        var newObj = Instantiate(Prefab2, pos, Quaternion.identity);
        newObj.name = Prefab2.name + (TargetList.Count + 1);
        newObj.transform.parent = RadarCenter.transform;
        return newObj;
    }

    //Updateì‡
    private void CreateObj()
    {
        var newObj1 = CreateNewEnemy();
        var newObj2 = CreateNewTarget();

        EnemyList.Add(newObj1);
        TargetList.Add(newObj2);
    }
}
