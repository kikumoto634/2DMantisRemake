using UnityEngine;

public class SlashControl : MonoBehaviour
{
    private GameObject Player = null;
    private SpriteRenderer PlayerSp = null;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerSp = Player.GetComponent<SpriteRenderer>();
    }

    public void SlashSE()
    {
        AudioManager.instance.Play("Slash");
    }

    public void SpAnim(Sprite sp)
    {
        PlayerSp.sprite = sp;
    }
}
