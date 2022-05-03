using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExpControl : MonoBehaviour
{
    private Image ExpImage = null;

    private int AddExp = 0;
    private int CurrentExp = 0;
    private int MaxExp = 10;

    private void Start()
    {
        ExpImage = this.gameObject.GetComponent<Image>();

        ExpImage.fillAmount = 0;
    }

    private void LateUpdate()
    {
        if(ExpImage.fillAmount >= 1)
        {
            CurrentExp = 0;
            ExpImage.fillAmount = 0;
        }

        if(AddExp > 0.0f)
        {
            CurrentExp += AddExp;
            AddExp = 0;

            DOTween.To
            (
                () => ExpImage.fillAmount,
                (x) => ExpImage.fillAmount = x,
                (float)CurrentExp / (float)MaxExp,
                1.0f
            );
        }

        //Debug.Log("Now Exp : " + CurrentExp);
    }

    public void SetAddExp(int value)
    {
        AddExp = value;
    }
}
