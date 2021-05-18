using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip _bulletSound, _laserSound, _bossEntryCrab, _bossEntryOcto, _bossEntrySpider, _Enemydeath, _HitSound , _playerDeath , _waveAlert ,ButtonHover , ButtonPress ,_powerUpSound ,WallDamage , _healthRegeneration , _hitWall ,_alertMusic;
    static AudioSource _audiosource;
    // Start is called before the first frame update
    void Start()
    {
        _bulletSound = Resources.Load<AudioClip>("BulletShot");
        _laserSound = Resources.Load<AudioClip>("laser_shot");
        _bossEntryCrab = Resources.Load<AudioClip>("wolf2atk");
        _bossEntryOcto = Resources.Load<AudioClip>("roar23");
        _bossEntrySpider = Resources.Load<AudioClip>("insect2");
        _Enemydeath = Resources.Load<AudioClip>("EnemyDeath");
        _HitSound = Resources.Load<AudioClip>("Hit");
        _playerDeath = Resources.Load<AudioClip>("GruntVoice01");
        _waveAlert = Resources.Load<AudioClip>("WaveAlert");
        ButtonHover = Resources.Load<AudioClip>("ButtonHover");
        ButtonPress = Resources.Load<AudioClip>("ButtonPress");
        _powerUpSound = Resources.Load<AudioClip>("power_up_sound");
        WallDamage = Resources.Load<AudioClip>("explosion_sound");
        _healthRegeneration = Resources.Load<AudioClip>("HealthRegen");
        _hitWall = Resources.Load<AudioClip>("HitWall");
        _alertMusic = Resources.Load<AudioClip>("AlertMusic");

        _audiosource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string SoundName)
    {
        switch(SoundName)
        {
            case "BulletShot":
                _audiosource.PlayOneShot(_bulletSound);
                break;

            case "LaserShot":
                _audiosource.PlayOneShot(_laserSound);
                break;

            case "EnemyDeath":
                _audiosource.PlayOneShot(_Enemydeath);
                break;

            case "PowerUp":
                _audiosource.PlayOneShot(_powerUpSound);
                break;

            case "HitSound":
                _audiosource.PlayOneShot(_HitSound);
                break;

            case "WaveAlert":
                _audiosource.PlayOneShot(_waveAlert);
                break;

            case "ButtonHover":
                _audiosource.PlayOneShot(ButtonHover);
                break;

            case "ButtonPress":
                _audiosource.PlayOneShot(ButtonPress);
                break;

            case "CrabBoss":
                _audiosource.PlayOneShot(_bossEntryCrab);
                break;

            case "SpiderBoss":
                _audiosource.PlayOneShot(_bossEntrySpider);
                break;

            case "OctoBoss":
                _audiosource.PlayOneShot(_bossEntryOcto);
                break;

            case "PlayerDeath":
                _audiosource.PlayOneShot(_playerDeath);
                break;

            case "WallDamage":
                _audiosource.PlayOneShot(WallDamage);
                break;

            case "HealthRegen":
                _audiosource.PlayOneShot(_healthRegeneration);
                break;

            case "HitWall":
                _audiosource.PlayOneShot(_hitWall ,0.25f);
                break;

            case "HitWallBoss":
                _audiosource.PlayOneShot(_hitWall);
                break;

            
        }
    }
}
