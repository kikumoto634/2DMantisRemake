using UnityEngine;
using System.Collections;

public class ItemControl : MonoBehaviour
{
    float vx = 0f;
    float vy = 0f;

    //�L�����Ԃ̋���
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



    //�R���|�[�l���g
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


    //�A�C�e�����
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
            // ���̕����֎w�肵���ʂŐi��
            vx = dir.x * speed;
            vy = dir.y * speed;
            this.transform.Translate(vx / 50, vy / 50, 0f);

            speed += 0.5f;


            //�ڐG�ꔻ��
            if(characterDistance.magnitude >= Mathf.Abs(0.5f))
            {
                return;
            }

            //�l��
            Iscatch = false;

            int i = PlayerControl.Item + 1; //  �擾
            PlayerControl.Item = i;    //�Q��

            //Debug.Log(PlayerControl.Item);      //�l���A�C�e����
            //Debug.Log(item);    //���O

            itemArea.IsCreate = true;

        }
    }


    //Random����
    void ItemCreate()
    {
        Vector2 pos = itemArea.randomCreate.Create();

        float x = pos.x;
        float y = pos.y;

        this.transform.position = new Vector2(x, y);
        itemArea.IsCreate = false;
    }

}

/*�G���A�ڒn*/
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

