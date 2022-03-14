using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    // Sound�N���X�z��
    [SerializeField]
    private Sound[] sounds;
    // �V���O���g����
    public static AudioManager instance;

    private void Awake()
    {
        // AudioManager�C���X�^���X�����݂��Ȃ���ΐ���
        // ���݂����Destroy�Creturn
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

        // Sound�N���X�ɓ��ꂽ�f�[�^��AudioSource�ɓ��Ă͂߂�
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
        // �����_���@��������Predicate
        // Sound�N���X�̔z��̒��̖��O�ɁC
        // ����name�ɓ��������̂����邩�ǂ����m�F
        Sound s = Array.Find(sounds, sound => sound.name == name);
        // �Ȃ����return
        if (s == null)
        {
            print("Sound" + name + "was not found");
            return;
        }
        // �����Play()
        s.audioSource.Play();
    }
}

[System.Serializable]
public class Sound
{
    [Tooltip("�T�E���h�̖��O")]
    public string name;
    // AudioSource�ɕK�v�ȏ��
    [Tooltip("�T�E���h�̉���")]
    public AudioClip clip;
    [Tooltip("�T�E���h�{�����[��, 0.0����1.0�܂�")]
    public float volume;
    // AudioSource�DInspector�ɕ\�����Ȃ�
    [HideInInspector]
    public AudioSource audioSource;
}
