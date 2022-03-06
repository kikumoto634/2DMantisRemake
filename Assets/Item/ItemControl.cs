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

    public AreaItem itemArea = new AreaItem(default, default, false);



    //�R���|�[�l���g
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
            AS.Play();

        }
        else if (Iscatch)
        {
            Vector2 dir = (characterDistance).normalized;
            // ���̕����֎w�肵���ʂŐi��
            vx = dir.x * speed;
            vy = dir.y * speed;
            this.transform.Translate(vx / 50, vy / 50, 0f);

            speed += 0.5f;


            if(characterDistance.magnitude >= Mathf.Abs(0.5f))
            {
                return;
            }

            Iscatch = false;

            int i = PlayerControl.Item + 1; //  �擾
            PlayerControl.Item = i;    //�Q��

            Debug.Log(PlayerControl.Item);      //�l���A�C�e����
            Debug.Log(item);    //���O

            itemArea.IsCreate = true;

        }
    }


    //Random����
    void ItemCreate()
    {
        float x = Random.Range(itemArea._rangeA.position.x, itemArea._rangeB.position.x);
        float y = Random.Range(itemArea._rangeA.position.y, itemArea._rangeB.position.y);

        this.transform.position = new Vector2(x, y);
        itemArea.IsCreate = false;
    }

}

/*�G���A�ڒn*/
[System.Serializable]
public class AreaItem
{
    [Header("�����͈�A")]
    public Transform _rangeA = default;
    [Header("�����͈�B")]
    public Transform _rangeB = default;

    public bool IsCreate = false;

    public AreaItem(Transform RangeA, Transform RangeB, bool IsCreate)
    { 
        this._rangeA = RangeA;
        this._rangeB = RangeB;

        this.IsCreate = IsCreate;
    }
}

//7��(��)�@14��

//�O���N���j�b�N