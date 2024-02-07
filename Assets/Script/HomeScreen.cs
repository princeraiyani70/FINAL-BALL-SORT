using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    [SerializeField]
    Button PlayButton;

    [SerializeField]
    GameObject ExitPanel;

    public void PlayButtonClickAction()
    {
        SoundManager.Instance.ButtonClickSound();
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (ExitPanel.activeInHierarchy)
            {
                ExitPanelNoClickAction();
            }
            else
            {
                ExitPanel.SetActive(true);
            }
        }
    }

    public void ExitPanelYesClickAction()
    {
        SoundManager.Instance.ButtonClickSound();
        Application.Quit();
    }

    public void ExitPanelNoClickAction()
    {
        SoundManager.Instance.ButtonClickSound();
        ExitPanel.transform.GetChild(0).GetComponent<Animator>().Play("YesButtonExit");
        ExitPanel.transform.GetChild(1).GetComponent<Animator>().Play("NoButtonExit");
        ExitPanel.transform.GetChild(2).GetComponent<Animator>().Play("AreYouSureExit");
        StartCoroutine(ExitPanelCloseAnimation());
    }

    IEnumerator ExitPanelCloseAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        ExitPanel.SetActive(false);
    }

   
}
