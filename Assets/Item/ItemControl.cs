using UnityEngine;
using DG.Tweening;

public class ItemControl : MonoBehaviour
{
    float vx = 0f;
    float vy = 0f;

    //キャラ間の距離
    private Vector2 characterDistance = default;
    bool Iscatch = false;

    float speed = -5.0f;


    [Header("生成範囲A")]
    [SerializeField] private Transform _rangeA = default;
    [Header("生成範囲B")]
    [SerializeField] private Transform _rangeB = default;

    bool IsCreate = false;


    //コンポーネント
    private GameObject Player = null;
    private PlayerControl PlayerControl = null;
    private AudioSource AS = null;



    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerControl = Player.GetComponent<PlayerControl>();

        AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!IsCreate)
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

                Debug.Log(PlayerControl.Item);

                IsCreate = true;

            }
        }
        else if (IsCreate)
        { 
            ItemCreate();
        }
    }


    //Random生成
    void ItemCreate()
    {
        float x = Random.Range(_rangeA.position.x, _rangeB.position.x);
        float y = Random.Range(_rangeA.position.y, _rangeB.position.y);

        this.transform.position = new Vector2(x, y);
        IsCreate = false;
        //this.gameObject.SetActive(true);
    }

}
