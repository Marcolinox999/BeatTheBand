using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    
    [SerializeField] private string tag1 = "Player";
    [SerializeField] private string tag2 = "Boss";
    [SerializeField] private Color talkingColor;
    [SerializeField] private Color listeningColor;
    
    public static DialogueManager Instance;
 
    public Image tag1Image;
    public Image tag2Image;
    public TMP_Text characterName;
    public TMP_Text dialogueArea;
    public AudioClip music;
 
    private Queue<DialogueLine> lines;
    
    public bool isDialogueActive = false;
 
    public float typingSpeed = 0.2f;

    [Header("Audio")] [SerializeField] private AudioClip[] dialogueTypingSound;
    private AudioSource audioSource;
    
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
 
        lines = new Queue<DialogueLine>();
        
    }
 
    public void StartDialogue(Dialogue dialogue)
    {
        if (isDialogueActive == false)
        {
            isDialogueActive = true;
            AudioManager.instance.PlayMusic(music);

            lines.Clear();

            foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
            {
                lines.Enqueue(dialogueLine);
            }

            DisplayNextDialogueLine();
        }
        else
        {
            DisplayNextDialogueLine();
        }
    }
 
    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
 
        DialogueLine currentLine = lines.Dequeue();
        
        characterName.text = currentLine.character.name;
        if (characterName.text == tag1)
        {
            tag2Image.color = listeningColor;
            tag1Image.color = talkingColor;
        }
        else
        {
            tag2Image.color = talkingColor;
            tag1Image.color = listeningColor;  
        }

        if (currentLine.character.portrait != tag2Image.sprite)
        { 
            tag2Image.sprite = currentLine.character.portrait; 
        }

        if (currentLine.character.name == tag1 && currentLine.character.portrait != tag1Image.sprite)
        {
            tag1Image.sprite = currentLine.character.portrait;
        }
        else if (currentLine.character.name == tag2 && currentLine.character.portrait != tag2Image.sprite)
        {
            tag2Image.sprite = currentLine.character.portrait;
        }
        StopAllCoroutines();
 
        StartCoroutine(TypeSentence(currentLine));
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
           int  i= Random.Range(0, dialogueTypingSound.Length);
           AudioManager.instance.PlayMusic(dialogueTypingSound[i]);
            yield return new WaitForSeconds(typingSpeed);
        }
    }
 
    void EndDialogue()
    {
        Debug.Log("Fin del dialogo");
    }
}

 