using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This is for managing all the UI interaction in the main menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    Animator _mainmenuAnimation;
    [SerializeField]
    GameObject HowToplay;
    [SerializeField]
    GameObject Credits;
    [SerializeField]
    GameObject HighScore;
    [SerializeField]
    GameObject SkipIntro;
    [SerializeField]
    Text HighScoreS;
    [SerializeField]
    Animator _CrossFade;
    [SerializeField]
    GameObject MainMenu;

    private void Start()
    {
        HighScoreS.text = "HighScore : " + PlayerPrefs.GetInt("Waves" , 0).ToString() + " Waves";
    }
    public void PlayGame()
    {
        AudioManager.PlaySound("ButtonPress");
        HighScore.SetActive(false);
        SkipIntro.SetActive(true);      
    }
    public void HideMainMenu()
    {
        AudioManager.PlaySound("ButtonPress");
        _mainmenuAnimation.SetBool("MOVEIN", false);
    }

    public void ShowMainMenu()
    {
        _mainmenuAnimation.SetBool("MOVEIN", true);
    }

    public void ShowHowtoPlay()
    {
        AudioManager.PlaySound("ButtonPress");
        HowToplay.SetActive(true);
        HideMainMenu();
    }

    public void HideHowToPlay()
    {
        AudioManager.PlaySound("ButtonPress");
        HowToplay.SetActive(false);
        ShowMainMenu();
    }

    public void ShowCredits()
    {
        AudioManager.PlaySound("ButtonPress");
        Credits.SetActive(true);
        HideMainMenu();
    }

    public void HideCredits()
    {
        AudioManager.PlaySound("ButtonPress");
        Credits.SetActive(false);
        ShowMainMenu();
    }

    public void Quit()
    {
        AudioManager.PlaySound("ButtonPress");
        Application.Quit();
    }

    public void Skip()
    {
        AudioManager.PlaySound("ButtonPress");
        MainMenu.SetActive(false);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        _CrossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(LevelIndex);
    }

}
