using UnityEngine;
using DG.Tweening;

public class ItemControl : MonoBehaviour
{
    float vx = 0f;
    float vy = 0f;

    //キャラ間の距離
    private Vector2 characterDistance = default;
    bool Iscatch = false;

    float speed = -3.0f;

    //コンポーネント
    private GameObject Player = null;
    private AudioSource AS = null;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        characterDistance = Player.transform.position - this.transform.position;

        if(!Iscatch)
        {
            if (characterDistance.magnitude < Mathf.Abs(5f))
            { 
                Iscatch = true;
                AS.Play();
            }
        }
        else if (Iscatch)
        {
            Vector2 dir = (characterDistance).normalized;
            // その方向へ指定した量で進む
            vx = dir.x * speed;
            vy = dir.y * speed;
            this.transform.Translate(vx / 50, vy / 50, 0f);

            speed += 0.1f;

            if (characterDistance.magnitude < Mathf.Abs(0.5f))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
