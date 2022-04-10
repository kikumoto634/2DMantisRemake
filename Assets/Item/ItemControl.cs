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

    public AreaItem itemArea = default;



    //コンポーネント
    private GameObject Player = null;
    private PlayerControl PlayerControl = null;


    private void Start()
    {
        itemArea = new AreaItem(false);

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerControl = Player.GetComponent<PlayerControl>();

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
                    speed = -5f;
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
            AudioManager.instance.Play("ItemCatch");
        }
        else if (Iscatch)
        {
            Vector2 dir = (characterDistance).normalized;
            // その方向へ指定した量で進む
            vx = dir.x * speed;
            vy = dir.y * speed;
            this.transform.Translate(vx / 50, vy / 50, 0f);

            speed += 0.5f;


            //接触語判定
            if(characterDistance.magnitude >= Mathf.Abs(0.5f))
            {
                return;
            }

            //獲得
            Iscatch = false;

            int i = PlayerControl.Item + 1; //  取得
            PlayerControl.Item = i;    //参照

            //Debug.Log(PlayerControl.Item);      //獲得アイテム数
            //Debug.Log(item);    //名前

            itemArea.IsCreate = true;

        }
    }


    //Random生成
    void ItemCreate()
    {
        Vector2 pos = itemArea.randomCreate.Create();

        float x = pos.x;
        float y = pos.y;

        this.transform.position = new Vector2(x, y);
        itemArea.IsCreate = false;
    }

}

/*エリア接地*/
[System.Serializable]
public class AreaItem
{
    public RandomCreate randomCreate = default;

    public bool IsCreate = false;

    public AreaItem(bool IsCreate)
    { 
        randomCreate = new RandomCreate();

        this.IsCreate = IsCreate;
    }
}

