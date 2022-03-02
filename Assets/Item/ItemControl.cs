using UnityEngine;
using DG.Tweening;

public class ItemControl : MonoBehaviour
{
    //�L�����Ԃ̋���
    private Vector2 characterDistance = default;
    bool Iscatch = false;

    //�R���|�[�l���g
    private GameObject Player = null;

    //�L���b�V��
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
                Debug.Log("�L���b�`");
            }
        }
        else if (Iscatch)
        { 
            //transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, 0.1f);

            transform.DOMove(Player.transform.position, 2f).SetEase(Ease.InBack);
        }
    }
}
