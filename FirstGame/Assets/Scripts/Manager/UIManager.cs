using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject MobileGameUI;
    [SerializeField]
    GameObject Pause;
    public Animator _CrossFade;
    public Slider _slider;
    public Gradient _gradient;
    public Image _walls , _SpeedImage , _LaserImage , _ShieldImage , _fillImage;
    public Text _crab, _spider, _Octo, _SpeedPowerUpText, _LaserPowerUpText, _ShieldPowerUpText;
    public Text _FinalCrabKill, _FinaloctoKill, _FinalSpiderKil, _WavesLasted , _killCount;
    public GameObject _healthIncrease;
    public Text _HealthIncreaseText ,_waveCounterText;
    int _crabKillCount = 0, _SpiderKillCount = 0, _OctopusKillCount = 0 ,Wave = 1;
    [SerializeField]
    Animator _PauseMenuAnim;
    [SerializeField]
    Animator _GameOverMenuAnim;
    [SerializeField]
    Animator _WaveReadierAnim;
    [SerializeField]
    GameObject _PauseMenu;
    [SerializeField]
    GameObject _UIManager;
    [SerializeField]
    GameObject _WaveCounter;
    public CameraShake LastShake;
    float _slowDownFactor = 0.1f;
    float _slowDownLenght = 5f;
    bool _slowmoUpdatedo = false;
    bool _canPause = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _WaveCounter.SetActive(true);
        for(int i =1; i < 4; i++)
        {
            DeactivatePowerUpUI(i);
        }
        Wall.WallFallen += GameOver;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        _PauseMenuAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        _GameOverMenuAnim.updateMode = AnimatorUpdateMode.UnscaledTime;

        if(_slowmoUpdatedo == true)
        {
            Time.timeScale += (1f / _slowDownLenght) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        }
       
    }

    //update ui
    public void NewWave()
    {
        Wave++;
        _waveCounterText.text = "WAVE : " + Wave.ToString();
        AudioManager.PlaySound("WaveAlert");
        StartCoroutine(AnimateWaveReadier());
    }
    
    //Play next wave animation
    IEnumerator AnimateWaveReadier()
    {
        _WaveReadierAnim.SetBool("NewWave", true);
        yield return new WaitForSeconds(3.5f);
        _WaveReadierAnim.SetBool("NewWave", false);
    }

    //set max health to fulll
    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;
        _fillImage.color = _gradient.Evaluate(1f);

    }

    //set health to wall the current wall health is 
    public void SetHealth(float health)
    {
        _slider.value = health;
        _fillImage.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    //increase health in case heart as been ecountered and display approriate message
    public void SetHealth(int health)
    {
        _slider.value += health;
        _fillImage.color = _gradient.Evaluate(_slider.normalizedValue);
        StartCoroutine(HealthIncreaseMessage(health));
    }
    
    IEnumerator HealthIncreaseMessage(int health)
    {
        _healthIncrease.SetActive(true);
        _HealthIncreaseText.text = "Health Increased by " + health.ToString() + "%";

        yield return new WaitForSeconds(3f);

        _healthIncrease.SetActive(false);
    }

    //Update ui
    public void SetWall(Sprite CurrentWallCondition)
    {
        _walls.sprite = CurrentWallCondition;
    }

    //increment kill count in the ui
    public void UpdateKillCount(int EID)
    {
        switch(EID)
        {
            case 1:
                _crabKillCount++;
                _crab.text = " X " + _crabKillCount.ToString();
                break;

            case 2:
                _SpiderKillCount++;
                _spider.text = " X " + _SpiderKillCount.ToString();
                break;

            case 3:
                _OctopusKillCount++;
                _Octo.text = " X " + _OctopusKillCount.ToString();
                break;
        }
    }


    //Update the ui as the player picks the power up
    public void ActivatePowerUpUI(int PowerUpID)
    {
        
        switch(PowerUpID)
        {
            case 1:
                _SpeedImage.color = new Color(1, 0 , 0.86f , 1f);
                _SpeedPowerUpText.color = Color.white;
                break;

            case 2:
                _LaserImage.color = new Color(1, 0.65f, 0.65f, 1f);
                _LaserPowerUpText.color = Color.white;
                break;

            case 3:
                _ShieldImage.color = new Color(0, 0.015f, 0.99f , 1f);
               _ShieldPowerUpText.color = Color.white;
                break;
        }
    }

    //deactivate the power up after its designated cooldown
    public void DeactivatePowerUpUI(int PowerUpID)
    {
        switch (PowerUpID)
        {
            case 1:
                _SpeedImage.color = new Color(1, 0, 0.86f, 0.1f);
                _SpeedPowerUpText.color = Color.clear;
                break;

            case 2:
                _LaserImage.color = new Color(1, 0.65f, 0.65f, 0.1f);
                _LaserPowerUpText.color = Color.clear;
                break;

            case 3:
                _ShieldImage.color = new Color(0, 0.015f, 0.99f, 0.1f);
                _ShieldPowerUpText.color = Color.clear;
                break;
        }
    }

    //pause game and stop time
    public void PauseGame()
    {
        if(_canPause == true)
        {
            Pause.SetActive(false);
            AudioManager.PlaySound("ButtonPress");
            _PauseMenu.SetActive(true);
            _PauseMenuAnim.SetBool("IsPaused", true);
            Time.timeScale = 0;
        }
        
    }

    //resume game and make time run as normal
    public void ResumeGame()
    {
        _PauseMenu.SetActive(false);
        Pause.SetActive(true);
        _PauseMenuAnim.SetBool("IsPaused", false);
        Time.timeScale = 1;
        AudioManager.PlaySound("ButtonPress");
    }

    //starts the game over animation and stores the highscore
    public void GameOver()
    {
        MobileGameUI.SetActive(false);
        Pause.SetActive(false);
        _WaveCounter.SetActive(false);
        
        if(Wave > PlayerPrefs.GetInt("Waves" , 0))
        {
            PlayerPrefs.SetInt("Waves", Wave);
        }
        
        StartCoroutine(WaitForGameOver());
        SlowMotion();
        StartCoroutine(PlayLastShakeLate());
        Wall.WallFallen -= GameOver;
        _canPause = false;
    }

    //waiting for dramatic effect
    IEnumerator WaitForGameOver()
    {
        int totalKillCount = _crabKillCount + _OctopusKillCount + _SpiderKillCount;
        yield return new WaitForSeconds(2f);
        _killCount.text = "KILLCOUNT : " + totalKillCount.ToString();
        _GameOverMenuAnim.SetTrigger("GameOver");
        _FinalCrabKill.text = " X " + _crabKillCount.ToString();
        _FinaloctoKill.text = " X " + _OctopusKillCount.ToString();
        _FinalSpiderKil.text = " X " + _SpiderKillCount.ToString();
        _WavesLasted.text = "You Have Lasted " + Wave + " Waves!";
    }

    //relaod the game
    public void ReloadGame()
    {
        FindObjectOfType<AdManager>().ShowInterstitialAds();
        AudioManager.PlaySound("ButtonPress");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    //perform slow motion effect for more drama
    void SlowMotion()
    {
        _slowmoUpdatedo = true;
        Time.timeScale = _slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    //perform the last camera shake effect once the player dies
    IEnumerator PlayLastShakeLate()
    {
        yield return null;
        StartCoroutine(LastShake.Shake(0.1f, 1f));
    }


    //to quit game
   public void QuitGame()
    {
        AudioManager.PlaySound("ButtonPress");
        Application.Quit();
    }

    //To navigate to main menu
    public void GotoMainMenu()
    {
        FindObjectOfType<AdManager>().ShowInterstitialAds();
        _UIManager.SetActive(false);
        AudioManager.PlaySound("ButtonPress");
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    //To load next level with fade effect
    IEnumerator LoadLevel(int LevelIndex)
    {
        _CrossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(LevelIndex);
    }
}
