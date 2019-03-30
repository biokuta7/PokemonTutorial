using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    
    public Text dialogueText;

    [Space(3f)]

    [Range(.01f, .1f)]
    public float timeBetweenCharacters = .05f;

    private Queue<string> sentences;
    private bool currentlyReadingDialogue = false;

    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();
        dialogueText.text = "";
    }

    
    public void Update()
    {
        if (sentences != null && sentences.Count > -1)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!currentlyReadingDialogue)
                {
                    DisplayNextSentence();
                    
                }
            }
        }
    }

    public void AddDialogue(string[] s)
    {
        AddDialogue(new Dialogue(s));
    }

    public void AddDialogue(string s)
    {
        AddDialogue(new Dialogue(s));
    }

    public void AddDialogue(Dialogue d)
    {
        foreach (string sentence in d.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
    }

    public void ClearDialogue()
    {
        sentences.Clear();
    }

    public void DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        

        string sentence = sentences.Dequeue();

        Debug.Log(sentence);

        StopCoroutine("DialogueRead");
        StartCoroutine(DialogueRead(sentence));
    }

    IEnumerator DialogueRead(string target)
    {
        currentlyReadingDialogue = true;
        dialogueText.text = "";
        foreach (char letter in target.ToCharArray())
        {

            dialogueText.text += letter;
            if (!letter.Equals('.'))
            {
                yield return new WaitForSeconds(timeBetweenCharacters);
            }
            else
            {
                yield return new WaitForSeconds(.1f);
            }
        }
        currentlyReadingDialogue = false;
    }

    public void EndDialogue()
    {
        sentences = new Queue<string>();
    }

    public bool CurrentlyReadingDialogue()
    { return currentlyReadingDialogue; }

    public bool AllCaughtUp()
    {
        return (sentences.Count <= 0 && !CurrentlyReadingDialogue());
    }

}