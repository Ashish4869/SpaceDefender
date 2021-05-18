using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class that is made to attach to an trigger object so that the convo can starrt
/// </summary>
public class DialougeTrigger : MonoBehaviour
{
    [SerializeField]
    Dialouge[] ScientistDialouge;

    [SerializeField]
    Dialouge[] SoldierDialouge; 
    
    //Start convo
    public void TriggerDialouge()
    {
        FindObjectOfType<DialougeManager>().StartDialouge(SoldierDialouge , ScientistDialouge);
    }
}
