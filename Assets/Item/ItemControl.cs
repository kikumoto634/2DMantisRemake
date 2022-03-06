using UnityEngine;
using System.Collections;

public class ItemControl : MonoBehaviour
{
    float vx = 0f;
    float vy = 0f;

    //キャラ間の距離
    private Vector2 characterDistance = default;
    bool Iscatch = false;

    float speed = -5.0f;


    [System.Serializable]
    public enum Item
    {
        Area,
        Drop,
    }
    public Item item = Item.Area;

    public AreaItem itemArea = new AreaItem(default, default, false);



    //コンポーネント
    private GameObject Player = null;
    private PlayerControl PlayerControl = null;
    private AudioSource AS = null;



    private void Start()
    {
        itemArea._rangeA = GameObject.FindGameObjectWithTag("RangeA").transform;
        itemArea._rangeB = GameObject.FindGameObjectWithTag("RangeB").transform;

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerControl = Player.GetComponent<PlayerControl>();

        AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        switch(item)
        {
            case Item.Area:

                if (!itemArea.IsCreate)
                {
                    ItemCollect();
                }
                else if (itemArea.IsCreate)
                { 
                    ItemCreate();
                }

                break;


            case Item.Drop:

                ItemCollect();

                break;
        }
    }


    //アイテム回収
    void ItemCollect()
    { 
        characterDistance = Player.transform.position - this.transform.position;

        if (!Iscatch)
        {
            if(characterDistance.magnitude >= Mathf.Abs(5f))
            {
                return ;
            }

            Iscatch = true;
            AS.Play();

        }
        else if (Iscatch)
        {
            Vector2 dir = (characterDistance).normalized;
            // その方向へ指定した量で進む
            vx = dir.x * speed;
            vy = dir.y * speed;
            this.transform.Translate(vx / 50, vy / 50, 0f);

            speed += 0.5f;


            if(characterDistance.magnitude >= Mathf.Abs(0.5f))
            {
                return;
            }

            Iscatch = false;

            int i = PlayerControl.Item + 1; //  取得
            PlayerControl.Item = i;    //参照

            Debug.Log(PlayerControl.Item);      //獲得アイテム数
            Debug.Log(item);    //名前

            itemArea.IsCreate = true;

        }
    }


    //Random生成
    void ItemCreate()
    {
        float x = Random.Range(itemArea._rangeA.position.x, itemArea._rangeB.position.x);
        float y = Random.Range(itemArea._rangeA.position.y, itemArea._rangeB.position.y);

        this.transform.position = new Vector2(x, y);
        itemArea.IsCreate = false;
    }

}

/*エリア接地*/
[System.Serializable]
public class AreaItem
{
    [Header("生成範囲A")]
    public Transform _rangeA = default;
    [Header("生成範囲B")]
    public Transform _rangeB = default;

    public bool IsCreate = false;

    public AreaItem(Transform RangeA, Transform RangeB, bool IsCreate)
    { 
        this._rangeA = RangeA;
        this._rangeB = RangeB;

        this.IsCreate = IsCreate;
    }
}

//7日(月)　14時

//三光クリニック