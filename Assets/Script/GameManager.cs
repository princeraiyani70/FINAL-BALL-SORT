using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject BallObject,FirstClickedObject,ParentOfLevel,SuccessPanel;

    [SerializeField]
    ParticleSystem Particle1, Particle2;

    [SerializeField]
    Button BackButton,RetryButton;

    [SerializeField]
    List<GameObject> TestTubes;

    [SerializeField]
    List<GameObject> SortedTubes;

    [SerializeField]
    GameObject[] AllLevels;

    [SerializeField]
    TextMeshProUGUI LevelText;

    [SerializeField]
    AudioSource SoundSource;

    [SerializeField]
    Sprite SoundOnSprite, SoundOffSprite;

    [SerializeField]
    Button SoundButton;

    [SerializeField]
    AudioClip[] SoundClips;


    int BallSort;

    bool FirstClick;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GettingSoundData();

        int Level = PlayerPrefs.GetInt("Level", 0);

        GameObject LevelObject = Instantiate(AllLevels[Level], ParentOfLevel.transform);
        Level = Level + 1;
        LevelText.text = "Level " + Level;

        for (int i = 0; i < LevelObject.transform.childCount; i++) 
        {
            TestTubes.Add(LevelObject.transform.GetChild(i).gameObject);
        }
    }



    public void GameLogic(GameObject SelectedTube)
    {
        if (!FirstClick)
        {
            SoundPlayOnTestTubeClick();
            FirstClick = true;
            FirstClickedObject = SelectedTube;
            SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).DOMove(SelectedTube.transform.GetChild(4).transform.position, 0.3f);
            // SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).position = SelectedTube.transform.GetChild(4).transform.position;
        }
        else
        {
            SoundPlayOnTestTubeClick();
            if (SelectedTube.transform.childCount == 5)
            {
                FirstClick = false;
                FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 1).transform.parent = SelectedTube.transform;
                SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).transform.DOMove(SelectedTube.transform.GetChild(4).transform.position, 0.5f).OnComplete(() => SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).transform.DOMove(SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 6).transform.position, 0.5f).SetEase(Ease.OutBounce));
                // FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 1).transform.position = SelectedTube.transform.GetChild(4).transform.position;
                // SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).transform.position = SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 6).transform.position;
            }
            else if (FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 1).gameObject.tag == SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).gameObject.tag && SelectedTube.transform.childCount != 9)
            {
                FirstClick = false;
                FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 1).transform.parent = SelectedTube.transform;
                SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).transform.DOMove(SelectedTube.transform.GetChild(4).transform.position, 0.5f);
                SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).transform.DOMove(SelectedTube.transform.GetChild(4).transform.position, 0.5f).OnComplete(() => SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).transform.DOMove(SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 6).transform.position, 0.5f).SetEase(Ease.OutBounce));
                // FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 1).transform.position = SelectedTube.transform.GetChild(4).transform.position;
                // SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 1).transform.position = SelectedTube.transform.GetChild(SelectedTube.transform.childCount - 6).transform.position;
            }
            else
            {
                FirstClick = false;
                FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 1).transform.DOMove(FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 6).transform.position, 0.5f).SetEase(Ease.OutBounce);
                // FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 1).transform.position = FirstClickedObject.transform.GetChild(FirstClickedObject.transform.childCount - 6).transform.position;
            }
        }
        CheckSorting();
    }

    void CheckSorting()
    {
        for (int i = 0; i < TestTubes.Count; i++)
        {
            BallSort = 0;
            if (TestTubes[i].transform.childCount == 9)
            {
                for (int j = 5; j <= 8; j++)
                {
                    if (TestTubes[i].transform.GetChild(j).gameObject.tag == TestTubes[i].transform.GetChild(5).gameObject.tag)
                    {
                        BallSort++;
                    }
                    if (BallSort == 4)
                    {
                        if (!SortedTubes.Contains(TestTubes[i]))
                        {
                            SortedTubes.Add(TestTubes[i]);
                            Debug.Log("Sorted Tubes" + SortedTubes.Count);
                        }
                    }
                }
            }
        }


        if (SortedTubes.Count == Level.Instance.FillUpTubes)
        {
            StartCoroutine(ParticleWaiting());
        }

    }

    IEnumerator ParticleWaiting()
    {
        yield return new WaitForSeconds(0.8f);
        Particle1.Play();
        SoundPlayOnSuccess();
        Particle2.Play();
        SoundPlayOnSuccess();
        StartCoroutine(SuccessPanelWaiting());
    }

    IEnumerator SuccessPanelWaiting()
    {
        yield return new WaitForSeconds(1f);
        SuccessPanel.SetActive(true);
        int Level = PlayerPrefs.GetInt("Level", 0);
        Level++;
        SuccessPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + Level + " Completed";
    }

    public void SuccessPanelClickAction()
    {
        SuccessPanel.transform.GetChild(0).transform.GetComponent<Animator>().Play("LevelCompletedExit");
        SuccessPanel.transform.GetChild(1).transform.GetComponent<Animator>().Play("TapToContinueExit");
        StartCoroutine(SuccessPanelExitAnimation());
    }

    IEnumerator SuccessPanelExitAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Winner");
        int Level = PlayerPrefs.GetInt("Level", 0);
        Level++;
        PlayerPrefs.SetInt("Level", Level);
        SuccessPanel.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void BackButtonClickAction()
    {
        SoundPlayOnButtonClick();
        SceneManager.LoadScene(0);
    }

    public void RetryButtonClickAction()
    {
        SoundPlayOnButtonClick();
        StartCoroutine(SceneLoadingWaiting());
    }

    IEnumerator SceneLoadingWaiting()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }


    public void SoundManagement()
    {
        int Sound = PlayerPrefs.GetInt("Sound", 0);

        if (Sound == 1)
        {
            SoundPlayOnButtonClick();
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

    public void SoundPlayOnButtonClick()
    {
        SoundSource.clip = SoundClips[0];
        SoundSource.Play();
    }

    public void SoundPlayOnTestTubeClick()
    {
        SoundSource.clip = SoundClips[1];
        SoundSource.Play();
    }

    public void SoundPlayOnSuccess()
    {
        SoundSource.clip = SoundClips[2];
        SoundSource.Play();
    }


}
