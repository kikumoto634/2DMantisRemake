using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // Soundクラス配列
    [SerializeField]
    private Sound[] sounds;
    // シングルトン化
    public static AudioManager instance;

    private void Awake()
    {
        // AudioManagerインスタンスが存在しなければ生成
        // 存在すればDestroy，return
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Soundクラスに入れたデータをAudioSourceに当てはめる
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            //s.audioSource.playOnAwake = false;
        }
    }

    public void Play(string name)
    {
        // ラムダ式　第二引数はPredicate
        // Soundクラスの配列の中の名前に，
        // 引数nameに等しいものがあるかどうか確認
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // なければreturn
        if (s == null)
        {
            print("Sound" + name + "was not found");
            return;
        }
        // あればPlay()
        s.audioSource.Play();
    }
}

[System.Serializable]
public class Sound
{
    [Tooltip("サウンドの名前")]
    public string name;
    // AudioSourceに必要な情報
    [Tooltip("サウンドの音源")]
    public AudioClip clip;
    [Tooltip("サウンドボリューム, 0.0から1.0まで")]
    public float volume;
    // AudioSource．Inspectorに表示しない
    [HideInInspector]
    public AudioSource audioSource;
}
