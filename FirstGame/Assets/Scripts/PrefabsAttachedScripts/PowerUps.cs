using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is responsible for navigating the powerup from the enemey death position
/// to the random position that the player can take
/// </summary>
public class PowerUps : MonoBehaviour
{
    [SerializeField]
    int PowerUpID;
    [SerializeField]
    GameObject TopLeft;
    [SerializeField]
    GameObject BottomRight;

    //if collides with the player activate power up
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if(player != null)
            { 
                player.PowerUp(PowerUpID);
                Destroy(gameObject);
            }
        }
    }

    //move the power up to its designated area
    public void MoveToarea()
    {
        Vector3 Destination = new Vector3((Random.Range(TopLeft.transform.position.x, BottomRight.transform.position.x))
            , (Random.Range(BottomRight.transform.position.y, TopLeft.transform.position.y)),0);

        StartCoroutine(Move(Destination));
        Destroy(gameObject, 5f);
    }

    //this is to make sure that the game does break while running the while loop in update
    IEnumerator Move(Vector3 Dest)
    {
        while(transform.position != Dest)
        {
            transform.position = Vector3.Lerp(transform.position, Dest, Time.deltaTime);
            yield return null;
        }
    }
}
