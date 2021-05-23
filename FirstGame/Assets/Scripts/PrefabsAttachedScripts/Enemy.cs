using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A enemy script attached to enemy for
/// 1.Movement
/// 2.Damaging the wall
/// 3.Assigning whether to give power up
/// </summary>
public class Enemy : MonoBehaviour
{
    //An id to distinguish between the diferent enemies
    [SerializeField]
    int EID;
    [SerializeField]
    Animator _enemyAnimator;
    [SerializeField]
    GameObject _deathEffect;
    [SerializeField]
    GameObject laser;
    [SerializeField]
    GameObject Speed;
    [SerializeField]
    GameObject Shield;
    [SerializeField]
    GameObject _Heart;
    [SerializeField]
    UIManager _upDateEnemyKillCount;
    [SerializeField]
    CameraShake BossRoar;
    [SerializeField]
    Wall _Wall;

    public float _speed = 1f;
    public float _upSpeed;
    public int _health = 10;
    bool _isWallBroken = false;
    public bool _isBoss = false , _powerUpInstantiated = false;
    public static bool isSpeedPowerUpActive = false , IsShieldPowerUpActive = false, IsLaserPowerUpActive = false;
    
    // Start is called before the first frame update
    void Start()
    { 
        _Wall = FindObjectOfType<Wall>().GetComponent<Wall>();
        _upDateEnemyKillCount = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        BossRoar = FindObjectOfType<CameraShake>().GetComponent<CameraShake>();
        _upSpeed = Random.Range(3f, 5f);
        Wall.WallFallen += WallBroken;
        _speed = Random.Range(1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        //if not the octo make the movment linear
        if(EID !=3)
        {
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector2(-_speed, _upSpeed) * Time.deltaTime);
        }
       
    }

    //reduce the enemy health and kills enemy when enemy dead and instantiate the death effect
    public void Die(int damage , int BID)
    {
        
        _health -= damage;
        AudioManager.PlaySound("HitSound");
        if(_health <= 0)
        {
            AudioManager.PlaySound("EnemyDeath");
            if (_isBoss == true)
            {
                _deathEffect.transform.localScale *= 3;
                Instantiate(_deathEffect, transform.position, transform.rotation);
            }
            else
            {
                _deathEffect.transform.localScale = new Vector3(3, 3, 3);
                Instantiate(_deathEffect, transform.position, transform.rotation);
            }
            
         
            //before killing off the enemy check if we can spawn a powerup and the player is alive
            if(_isWallBroken == false)
            {
                int ID, PrevID = 0;
                ID = Random.Range(0, 14);
                if (PrevID == ID)
                {
                    ID += 1;
                }
                else
                {
                    PrevID = ID;
                }

                //spawn power up by first making sure that the player doesnt have the power up
                //being spawned
                if(!_isBoss)
                {
                    switch (ID)
                    {
                        case 1:
                            if(isSpeedPowerUpActive == false)
                            {
                                Instantiate(Speed, transform.position, transform.rotation);
                                _powerUpInstantiated = true;
                            }
                            break;

                        case 2:
                            if (IsLaserPowerUpActive == false)
                            {
                                Instantiate(laser, transform.position, transform.rotation);
                                _powerUpInstantiated = true;
                            }
                            break;

                        case 3:
                            if(IsShieldPowerUpActive == false)
                            {
                                Instantiate(Shield, transform.position, transform.rotation);
                                _powerUpInstantiated = true;
                            }
                            break;
                    }
                }
                else
                {
                    ID = 15;
                    Instantiate(_Heart, transform.position, Quaternion.identity);
                    _powerUpInstantiated = true;
                }
                

                //since FindObjectOfType is a costly operation ,
                //perform it only when needed and call the function from power up script

                if (_powerUpInstantiated == true)
                {
                    FindObjectOfType<PowerUps>().GetComponent<PowerUps>().MoveToarea();
                }

            }

            _isBoss = false;
            Wall.WallFallen -= WallBroken;
            _upDateEnemyKillCount.UpdateKillCount(EID);

            
            Destroy(gameObject);
        }
    }


    //Static variables that govern whether the player has the power up or not 
    public static void PowerUpUpdater(int ID)
    {
        switch(ID)
        {
            case 1:
                isSpeedPowerUpActive = true;
                break;
            case 2:
                isSpeedPowerUpActive = false;
                break;

            case 3:
                IsLaserPowerUpActive = true;
                break;

            case 4:
                IsLaserPowerUpActive = false;
                break;

            case 5:
                IsShieldPowerUpActive = true;
                break;

            case 6:
                IsShieldPowerUpActive = false;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        switch(other.tag)
        {
            // if the enemy has reached the wall attck the wall and reduce the speed to zero
            case "Wall":
                _speed = 0;
                _upSpeed = 0;
                _enemyAnimator.SetTrigger("HasReachedWall");

                //animation event will take care ot it
                break;

                //bounce off boundary , only applicable to opctopus
            case "Boundary":
                _upSpeed *= -1;
                break;

                //if came in contact with shiled kill itself
            case "Shield":
                Die(1000,1);
                break;

                //play appropriate sound effect for respective boss
            case "BossEntryCheck":
                if(_isBoss == true)
                {
                    StartCoroutine(BossRoar.Shake(1f, 0.05f));

                    switch(EID)
                    {
                        case 1:
                            AudioManager.PlaySound("CrabBoss");
                            break;

                        case 2:
                            AudioManager.PlaySound("SpiderBoss");
                            break;

                        case 3:
                            AudioManager.PlaySound("OctoBoss");
                            break;
                    }
                }
                break;
        }
       
    }

    //damage the wall and additional functions if boss
    public void WallhitEffect()
    {
       if (_isWallBroken == false)
        {
            if (_Wall != null)
            {
                if (_isBoss == true)
                {
                    StartCoroutine(BossRoar.Shake(0.1f, 0.05f));
                    AudioManager.PlaySound("HitWallBoss");
                    _Wall.TakeDamage(Random.Range(3f, 5f));
                }
                else
                {
                    _Wall.TakeDamage(Random.Range(0.1f, 0.5f));
                    AudioManager.PlaySound("HitWall");
                }
               
            }
        }
    }


    //to transform a basic enemy into a boss
    public void Bossify()
    {
        if(EID == 3)
        {
            transform.position = new Vector2(transform.position.x + 20, 0);
        }
        else
        {
            transform.position = new Vector2(transform.position.x + 20, transform.position.y);
        }
       
        _isBoss = true;
        _speed = 0.25f;
        _health = 100;
        Vector2 LocalScale = transform.localScale;
        LocalScale *= 3f;
        transform.localScale = LocalScale;
       
    }

    //once the wall is broken let the enemy  walk normally
    public void WallBroken()
    {
        _isWallBroken = true;
        _enemyAnimator.SetTrigger("WallDestroyed");
        _speed = 1f;
        Wall.WallFallen -= WallBroken;
    }

    
}
