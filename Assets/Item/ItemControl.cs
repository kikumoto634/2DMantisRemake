using UnityEngine;
using DG.Tweening;

public class ItemControl : MonoBehaviour
{
    //キャラ間の距離
    private Vector2 characterDistance = default;
    bool Iscatch = false;

    //コンポーネント
    private GameObject Player = null;

    //キャッシュ
    private Vector3 ThisPos = default;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        ThisPos = this.transform.position;
    }

    private void Update()
    {
        if(!Iscatch)
        {
            characterDistance = Player.transform.position - ThisPos;
        }
    }

    private void LateUpdate()
    {
        if(!Iscatch)
        {
            if (characterDistance.magnitude < Mathf.Abs(5f))
            { 
                Iscatch = true;
                Debug.Log("キャッチ");
            }
        }
        else if (Iscatch)
        { 
            //transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, 0.1f);

            transform.DOMove(Player.transform.position, 2f).SetEase(Ease.InBack);
        }
    }
}
