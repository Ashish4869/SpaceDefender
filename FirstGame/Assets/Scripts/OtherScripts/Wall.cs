using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public static event System.Action WallFallen;
    [SerializeField]
    Sprite _wallDamaged;
    [SerializeField]
    Sprite _wallHeavilydamaged;
    [SerializeField]
    Sprite _wallNoDamage;
    [SerializeField]
    CameraShake Shake;

    bool _hasShakePlayedWhen50 = false, _hasShakePlayedWhen25 = false;
    SpriteRenderer _wallSprites;
    [SerializeField]
    UIManager _healthbar;

    [SerializeField]
    float _health = 100;
    // Start is called before the first frame update
    void Start()
    {
        _healthbar.SetMaxHealth(_health);
        _wallSprites = GetComponent<SpriteRenderer>();
        _wallSprites.sprite = _wallNoDamage;
        _healthbar.SetWall(_wallNoDamage);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _healthbar.SetHealth(_health);
        if(_health < 0)
        {
            WallFallen();
            Destroy(gameObject,0.1f);
        }
        else if(_health < 25)
        {
            if (_hasShakePlayedWhen25 == false)
            {
                AudioManager.PlaySound("WallDamage");
                StartCoroutine(Shake.Shake(0.1f, 0.75f));
            }

            _hasShakePlayedWhen25 = true;
            _wallSprites.sprite = _wallHeavilydamaged;
            _healthbar.SetWall(_wallHeavilydamaged);
        }
        else if(_health < 50)
        {
            if (_hasShakePlayedWhen50 == false)
            {
                AudioManager.PlaySound("WallDamage");
                StartCoroutine(Shake.Shake(0.1f, 0.5f));
            }

            _hasShakePlayedWhen50 = true;
            _wallSprites.sprite = _wallDamaged;
            _healthbar.SetWall(_wallDamaged);
            _hasShakePlayedWhen25 = false;
        }
        else
        {
            _hasShakePlayedWhen50 = false;
            _healthbar.SetWall(_wallNoDamage);
        }
    }

    public void Heart(int hearth)
    {
        _health += hearth;
    }
}
