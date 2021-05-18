using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{
    [SerializeField]
    Dialouge[] ScientistDialouge;

    [SerializeField]
    Dialouge[] SoldierDialouge; 
    

    public void TriggerDialouge()
    {
        FindObjectOfType<DialougeManager>().StartDialouge(SoldierDialouge , ScientistDialouge);
    }
}
