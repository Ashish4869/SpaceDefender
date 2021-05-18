using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialougeManager : MonoBehaviour
{
    public CameraShake shake;
    public AudioClip _alertMusic;
    public AudioSource _audiosource;
    public Animator _CrossFade;
    public GameObject mainMenuHolder;
   
    [SerializeField]
    public Animator _enemyPassedThough;
    public GameObject npc1, npc2, npc3;
    public Animator animator;
    public Text NameText, DialougeText;
    Queue<string> sentences;
    int dialougeTurn = 0;
    int Dialougenumber = 0;
    bool DidEnemyPass = false , _canpress = true;
    int FunctionCalltimes = 0;
    Dialouge[] SD, SCD;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialouge(Dialouge[] SoldierDialouge , Dialouge[] ScientistDialouge)
    {
        SD = SoldierDialouge;
        SCD = ScientistDialouge;
        animator.SetBool("IsOpen", true);
        DisplayDialouge(SoldierDialouge[0], ScientistDialouge[0]);
    }

    public void DisplayDialouge(Dialouge currentSolderDialouge , Dialouge CurrentScientistDialouge)
    {
        
        sentences.Clear();
        if (dialougeTurn == 0)
        {
            NameText.text = currentSolderDialouge.NPCName;
            NameText.color = Color.yellow;


            foreach (string sentence in currentSolderDialouge.sentences)
            {
                sentences.Enqueue(sentence);
            }

        }
        else
        {
            NameText.text = CurrentScientistDialouge.NPCName;
            NameText.color = Color.green;

            foreach (string sentence in CurrentScientistDialouge.sentences)
            {
                sentences.Enqueue(sentence);
            }

        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(_canpress == true)
        {
            if (sentences.Count == 0)
            {
                EndDialouge();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        
    }

    IEnumerator TypeSentence(string Sentence)
    {
        DialougeText.text = "";
        foreach(char letter in Sentence.ToCharArray())
        {
            DialougeText.text += letter;
            yield return null ;
        }
    }



    public void EndDialouge()
    {
        
        if(dialougeTurn == 1 && Dialougenumber == 6 && DidEnemyPass == false )
        {
            _canpress = false;
            DidEnemyPass = true;
            _enemyPassedThough.SetTrigger("Pass");
            StartCoroutine(EnemyHitGenerator());
        }
        else if(dialougeTurn == 1 && Dialougenumber == 12)
        {
            mainMenuHolder.SetActive(false);

            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

            
        }
        else
        {
            FunctionCalltimes++;
            if (dialougeTurn == 0)
            {
                dialougeTurn = 1;
            }
            else
            {
                dialougeTurn = 0;
            }

            if (FunctionCalltimes % 2 == 0)
            {
                Dialougenumber++;
                DisplayDialouge(SD[Dialougenumber], SCD[Dialougenumber]);
            }
            else
            {
                DisplayDialouge(SD[Dialougenumber], SCD[Dialougenumber]);
            }
        }      
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        _CrossFade.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(LevelIndex);
    }

    IEnumerator EnemyHitGenerator()
    {
        AudioManager.PlaySound("CrabBoss");
        yield return new WaitForSeconds(0.5f);
        _canpress = true;
        AudioManager.PlaySound("WallDamage");
        AudioSource.PlayClipAtPoint(_alertMusic , Vector3.zero);
        StartCoroutine(shake.Shake(0.3f, 1f));
        Destroy(npc1);
        Destroy(npc2);
        Destroy(npc3);
       
        EndDialouge();
    }
}
