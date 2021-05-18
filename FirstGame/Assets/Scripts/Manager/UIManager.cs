using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
    public CameraShake LastShake;
    float _slowDownFactor = 0.1f;
    float _slowDownLenght = 5f;
    bool _slowmoUpdatedo = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
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

    public void NewWave()
    {
        Wave++;
        _waveCounterText.text = "WAVE : " + Wave.ToString();
        AudioManager.PlaySound("WaveAlert");
        StartCoroutine(AnimateWaveReadier());
    }

    IEnumerator AnimateWaveReadier()
    {
        _WaveReadierAnim.SetBool("NewWave", true);
        yield return new WaitForSeconds(3.5f);
        _WaveReadierAnim.SetBool("NewWave", false);
    }

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;
        _fillImage.color = _gradient.Evaluate(1f);

    }

    public void SetHealth(float health)
    {
        _slider.value = health;
        _fillImage.color = _gradient.Evaluate(_slider.normalizedValue);
    }
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

    public void SetWall(Sprite CurrentWallCondition)
    {
        _walls.sprite = CurrentWallCondition;
    }

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


    public void PauseGame()
    {
        _PauseMenu.SetActive(true);
        _PauseMenuAnim.SetBool("IsPaused", true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _PauseMenu.SetActive(false);
        _PauseMenuAnim.SetBool("IsPaused", false);
        Time.timeScale = 1;
        AudioManager.PlaySound("ButtonPress");
    }

    public void GameOver()
    {
        if(Wave > PlayerPrefs.GetInt("Waves" , 0))
        {
            PlayerPrefs.SetInt("Waves", Wave);
        }
        
        StartCoroutine(WaitForGameOver());
        SlowMotion();
        StartCoroutine(PlayLastShakeLate());
        Wall.WallFallen -= GameOver;
    }

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


    public void ReloadGame()
    {
        AudioManager.PlaySound("ButtonPress");
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      
    }

    void SlowMotion()
    {
        _slowmoUpdatedo = true;
        Time.timeScale = _slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    IEnumerator PlayLastShakeLate()
    {
        yield return null;
        StartCoroutine(LastShake.Shake(0.1f, 1f));
    }

   public void QuitGame()
    {
        Application.Quit();
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1;
        _UIManager.SetActive(false);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        _CrossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(LevelIndex);
    }


}
