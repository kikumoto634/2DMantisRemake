using UnityEngine;

public class SlashAudio : MonoBehaviour
{
    public void SlashSE()
    {
        AudioManager.instance.Play("Slash");
    }
}
