using UnityEngine;

public class RandomCreate
{
    [Header("¶¬”ÍˆÍA")]
    public Transform _rangeA = default;
    [Header("¶¬”ÍˆÍB")]
    public Transform _rangeB = default;

    Vector3 pos;

    public RandomCreate()
    {
        _rangeA = GameObject.FindGameObjectWithTag("RangeA").transform;
        _rangeB = GameObject.FindGameObjectWithTag("RangeB").transform;

        pos = default;
    }

    public Vector3 Create()
    { 
        pos.x = Random.Range(_rangeA.transform.position.x, _rangeB.transform.position.x);
        pos.y = Random.Range(_rangeA.transform.position.y, _rangeB.transform.position.y);

        return pos;
    }
}
