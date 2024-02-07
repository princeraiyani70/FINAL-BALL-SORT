using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
   public AudioSource SoundSource;

    [SerializeField]
    Sprite SoundOnSprite, SoundOffSprite;

    [SerializeField]
    Button SoundButton;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GettingSoundData();
    }

    public void SoundManagement()
    {
        int Sound = PlayerPrefs.GetInt("Sound", 0);

        if (Sound == 1)
        {
            ButtonClickSound();
            SoundSource.mute = false;
            SoundButton.GetComponent<Image>().sprite = SoundOnSprite;
            PlayerPrefs.SetInt("Sound", 0);
        }
        if (Sound == 0)
        {
            SoundSource.mute = true;
            SoundButton.GetComponent<Image>().sprite = SoundOffSprite;
            PlayerPrefs.SetInt("Sound", 1);
        }
    }
    public void GettingSoundData()
    {
        int Sound = PlayerPrefs.GetInt("Sound", 0);

        if (Sound == 1)
        {
            SoundSource.mute = true;
            SoundButton.GetComponent<Image>().sprite = SoundOffSprite;
        }
        if (Sound == 0)
        {
            SoundSource.mute = false;
            SoundButton.GetComponent<Image>().sprite = SoundOnSprite;
        }
    }

    public void ButtonClickSound()
    {
        SoundSource.Play();
    }

}
