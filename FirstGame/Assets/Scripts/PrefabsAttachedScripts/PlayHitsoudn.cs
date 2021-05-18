using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHitsoudn : MonoBehaviour
{
    void PlaySound()
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        enemy.WallhitEffect();
    }
}
