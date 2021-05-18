using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// To destroy gameobjects after they have played the animation
/// </summary>
public class AutoAnimationDestory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    
}
