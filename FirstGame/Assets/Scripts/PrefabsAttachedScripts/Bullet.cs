using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is responsible for bullet travel and detecting collisions
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField]
    int BulletID;
    Rigidbody2D _bulletRigidbody;
    float _speed = 20f;

    [SerializeField]
    GameObject _impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        
        _bulletRigidbody = GetComponent<Rigidbody2D>();
        _bulletRigidbody.velocity = transform.right * _speed;
    }

    // Update is called once per frame
    void Update()
    {
        //making sure that bullet is destroyed right before it goes out of the camera view
        if (transform.position.x > Camera.main.aspect * Camera.main.orthographicSize)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //making sure that bullet doestn hit the wall
        if(collision.tag != "Wall" && collision.tag != "Shield" && collision.tag != "Power" && collision.tag != "BossEntryCheck")
        {
            if(BulletID == 0)
            {
                Instantiate(_impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(_impactEffect, transform.position, transform.rotation);
            }
 
        }

        //if the bullet hits the enemy call the die function and kill the enemy as per the health
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                if(BulletID == 0)
                {
                    enemy.Die(Random.Range(3, 7) , 0);
                }
                else
                {
                    enemy.Die(Random.Range(10, 15) , 1);
                }
                
            }
        }

    }
}
