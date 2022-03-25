using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPControl : MonoBehaviour
{
    private GameObject Player = null;
    private PlayerControl playerControl = null;
    private Image HpImage = null;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerControl = Player.GetComponent<PlayerControl>();
        HpImage = this.gameObject.GetComponent<Image>();

        HpImage.fillAmount = 1;
    }

    private void Update()
    {
        DOTween.To
            (
                () => HpImage.fillAmount,
                (x) => HpImage.fillAmount = x,
                (float)playerControl.hp / (float)playerControl.Maxhp,
                1.0f
            );
    }
}
