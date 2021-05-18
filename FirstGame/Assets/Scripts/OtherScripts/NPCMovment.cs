using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic NPC movemnet that takes place in the BG
/// </summary>
public class NPCMovment : MonoBehaviour
{
    [SerializeField]
    Transform LeftEnd;
    [SerializeField]
    Transform RightEnd;

    public float speed = 5f, waittime;
     bool reachedRightEnd = false;
    Animator NPCAnimator;
    SpriteRenderer NPCSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        NPCAnimator = GetComponent<Animator>();
        NPCSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //move the character to a position defined in the inspesctor
        Vector3 DirectionToMoveRight = (RightEnd.position - transform.position ).normalized;
        if(reachedRightEnd == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, RightEnd.position, speed * Time.deltaTime);
            if (transform.position == RightEnd.position)
            {
                speed = 0;
                reachedRightEnd = true;
                NPCAnimator.SetBool("IsWaiting", true);
                StartCoroutine(WaitforSomeTime(1));
            }
        }

        Vector2 DirectionToMoveLeft = (LeftEnd.position - transform.position).normalized;
        if (reachedRightEnd == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, LeftEnd.position, speed * Time.deltaTime);
            if (transform.position == LeftEnd.position)
            {
                reachedRightEnd = false;
                speed = 0;
                NPCAnimator.SetBool("IsWaiting", true);
                StartCoroutine(WaitforSomeTime(2));
            }
        }
    }


    //wait for some before turn around
    IEnumerator WaitforSomeTime(int side)
    {
        yield return new WaitForSeconds(waittime);
        if(side == 1)
        {
            NPCSpriteRenderer.flipX = true;
            speed = 5f;
            NPCAnimator.SetBool("IsWaiting", false);
        }
        else
        {
            speed = 5f;
            NPCSpriteRenderer.flipX = false;
            NPCAnimator.SetBool("IsWaiting", false);

        }
    }
}
