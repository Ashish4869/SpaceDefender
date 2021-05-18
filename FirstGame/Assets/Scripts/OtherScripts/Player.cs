using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script the controls the following about the player 
/// 1. Movement
/// 2. Shooting
/// </summary>

public class Player : MonoBehaviour
{
    public float _speed = 5f;
    float _screenHalfHeightInWorldUnits;
    float _screenHalfWidthInWorldUnits;
    float _fireRate = 0.75f;
    float _nextFire = 0f;
    bool _isLaserPowerUpPicked = false;
    bool _isPlayerDisbled = false;
    bool _canFire = true;
    int _powerUpID;
    Vector2 inputDirection;

    [SerializeField]
    UIManager _PowerUpUI;
    [SerializeField]
    Transform _ShootPosition;
    [SerializeField]
    GameObject _bulletPrefab;
    [SerializeField]
    GameObject _laserPrefab;
    Animator _playerAnimator;
    [SerializeField]
    GameObject _MuzzleFlash;
    [SerializeField]
    GameObject _Shield;
    SpriteRenderer _playerSprite;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //storing the half height and witdth of the camera veiw into this variable
        _screenHalfHeightInWorldUnits = Camera.main.orthographicSize;
        _screenHalfWidthInWorldUnits = Camera.main.aspect * Camera.main.orthographicSize;

        _playerAnimator = GetComponent<Animator>();
        _playerSprite = GetComponent<SpriteRenderer>();

        Wall.WallFallen += DisablePlayer; 
    }

    // Update is called once per frame
    void Update()
    {
        //checking if the player is pressing the fire button facing towards and right and not in cooldown time
        if(Input.GetButtonDown("Fire1") && inputDirection.x >= 0 && Time.time > _nextFire && _canFire == true)
        {
            _nextFire = Time.time + _fireRate;
            _playerAnimator.SetBool("IsShooting", true);
            Shoot();
        }

        CalcMovement();

    }

    void CalcMovement()
    {
        if(_isPlayerDisbled == true)
        {
            _speed = 0;
        }

        //taking user input and make the player move 
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        transform.Translate(inputDirection * _speed * Time.deltaTime);
        
        //Defining the edges
        float MaxHeightPlayerCanGo = _screenHalfHeightInWorldUnits - (transform.localScale.y) / 2;
        float MaxWidthPlayerCanGO = _screenHalfWidthInWorldUnits - (transform.localScale.x) / 6;

        //triggering the movement animation
        if(inputDirection.magnitude != 0 )
        {
            _playerAnimator.SetBool("IsMoving",true);
        }
        else
        {
            _playerAnimator.SetBool("IsMoving", false);
            _playerAnimator.SetBool("IsShooting", false);
        }
        
        //fliping the player as per the direction
        if(inputDirection.x < 0)
        {
            _playerSprite.flipX = true;
        }
        else
        {
            _playerSprite.flipX = false;
        }
        //making sure the player cannot go out of bounds
        if (transform.position.y >= MaxHeightPlayerCanGo)
        {
            transform.position = new Vector2(transform.position.x, MaxHeightPlayerCanGo);
        }

        if (transform.position.y <= -MaxHeightPlayerCanGo)
        {
            transform.position = new Vector2(transform.position.x, -MaxHeightPlayerCanGo);
        }

        if (transform.position.x <= -MaxWidthPlayerCanGO)
        {
            transform.position = new Vector2(-MaxWidthPlayerCanGO, transform.position.y);
        }

        if(transform.position.x >= -MaxWidthPlayerCanGO + MaxWidthPlayerCanGO/2)
        {
            transform.position = new Vector2(-MaxWidthPlayerCanGO + (MaxWidthPlayerCanGO / 2), transform.position.y);
        }


    }

    void Shoot()
    {
        
        
        GameObject ZaBullet;
        if(_isLaserPowerUpPicked == false)
        {
            ZaBullet = _bulletPrefab;
            AudioManager.PlaySound("BulletShot");
            StartCoroutine(MuzzleFlash());
        }
        else
        {
            ZaBullet = _laserPrefab;
            AudioManager.PlaySound("LaserShot");
            StartCoroutine(MuzzleFlash());
        }

        if (inputDirection.magnitude == 0)
        {
            Instantiate(ZaBullet, _ShootPosition.position, _ShootPosition.rotation );
        }
        else
        { //making the player aim worse if he is moving

            Instantiate(ZaBullet, _ShootPosition.position, Quaternion.Euler(_ShootPosition.rotation.x, _ShootPosition.rotation.y, Random.Range(-20, 20)));
        }
    }


    IEnumerator MuzzleFlash()
    {
        _MuzzleFlash.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        _MuzzleFlash.SetActive(false);

    }


    //We will check which powerup has been picked up and from their call the appropriate function
   public void PowerUp(int PowerUpID)
    {
        _powerUpID = PowerUpID;
        switch(_powerUpID)
        {
            case 1:
                StartCoroutine(SpeedUp(_powerUpID));
                break;

            case 2:
                StartCoroutine(LaserPowerUp(_powerUpID));
                break;

            case 3:
                StartCoroutine(ActivateShields(_powerUpID));
                break;

            case 15:
                int wallhealth = Random.Range(10, 25);
                AudioManager.PlaySound("HealthRegen");
                _PowerUpUI.SetHealth(wallhealth);
                FindObjectOfType<Wall>().GetComponent<Wall>().Heart(wallhealth);
                break;
        }
    }

    //PowerUp function to enhance speed and firerate
    IEnumerator SpeedUp(int PowerUp)
    {
        AudioManager.PlaySound("PowerUp");
        Enemy.PowerUpUpdater(1);
        _PowerUpUI.ActivatePowerUpUI(PowerUp);
        _speed = 8f;
        _fireRate = 0.1f;

        yield return new WaitForSeconds(10f);

        _PowerUpUI.DeactivatePowerUpUI(PowerUp);
        _speed = 5f;
        _fireRate = 0.75f;
        Enemy.PowerUpUpdater(2);
    }

    //PowerUp function to use a stronger laser bullet
    IEnumerator LaserPowerUp(int PowerUp)
    {
        AudioManager.PlaySound("PowerUp");
        Enemy.PowerUpUpdater(3);
        _PowerUpUI.ActivatePowerUpUI(PowerUp);
        _isLaserPowerUpPicked = true;

        yield return new WaitForSeconds(15f);

        _PowerUpUI.DeactivatePowerUpUI(PowerUp);
        _isLaserPowerUpPicked = false;
        Enemy.PowerUpUpdater(4);
    }

    //PowerUp function to built a sheild that is cannot be destroyed
    IEnumerator ActivateShields(int PowerUp)
    {
        AudioManager.PlaySound("PowerUp");
        Enemy.PowerUpUpdater(5);
        _PowerUpUI.ActivatePowerUpUI(PowerUp);
       
            _Shield.SetActive(true);

            yield return new WaitForSeconds(10f);

            _Shield.SetActive(false);
        
        _PowerUpUI.DeactivatePowerUpUI(PowerUp);
        Enemy.PowerUpUpdater(6);

    }

    //Once the wall is fallen we will make sure that the player can no longer play
    void DisablePlayer()
    {
        AudioManager.PlaySound("PlayerDeath");
        _canFire = false;
        _isPlayerDisbled = true;
        _playerAnimator.SetTrigger("GameOver");
        Wall.WallFallen -= DisablePlayer;

    }
}
