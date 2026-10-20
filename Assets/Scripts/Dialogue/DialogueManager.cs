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
    [SerializeField] private int sceneIndex;
    public static DialogueManager Instance;

    public Image tag1Image;
    public Image tag2Image;

    [Header("Idle sprites (shown while the character is not speaking)")]
    [SerializeField] private Sprite tag1IdleSprite;
    [SerializeField] private Sprite tag2IdleSprite;
    public TMP_Text characterName;
    public TMP_Text dialogueArea;
    public AudioClip music;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] tag1TypingSounds; // Player voice
    [SerializeField] private AudioClip[] tag2TypingSounds; // Boss voice
    private AudioSource audioSource;

    [Tooltip("A sound plays once every N letters (1 = every letter)")]
    [SerializeField] private int lettersPerSound = 3;

    // True while the current line is still being typed letter by letter
    private bool isTyping = false;
    private DialogueLine activeLine;

    // Voice used for the line that is currently being typed
    private AudioClip[] currentTypingSounds;


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
        // If the line is still being typed, finish it instead of skipping to the next one
        if (isTyping)
        {
            CompleteCurrentLine();
            return;
        }

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();
        activeLine = currentLine;

        characterName.text = currentLine.character.name;
        bool isTag1Speaking = characterName.text == tag1;

        if (isTag1Speaking)
        {
            tag1Image.color = talkingColor;
            tag2Image.color = listeningColor;
            currentTypingSounds = tag1TypingSounds;

            // Speaker shows the line's portrait, listener goes back to idle
            if (currentLine.character.portrait != tag1Image.sprite)
                tag1Image.sprite = currentLine.character.portrait;

            if (tag2IdleSprite != null)
                tag2Image.sprite = tag2IdleSprite;
        }
        else
        {
            tag2Image.color = talkingColor;
            tag1Image.color = listeningColor;
            currentTypingSounds = tag2TypingSounds;

            // Speaker shows the line's portrait, listener goes back to idle
            if (currentLine.character.portrait != tag2Image.sprite)
                tag2Image.sprite = currentLine.character.portrait;

            if (tag1IdleSprite != null)
                tag1Image.sprite = tag1IdleSprite;
        }

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        isTyping = true;
        dialogueArea.text = "";
        int letterCount = 0;
        int step = Mathf.Max(1, lettersPerSound);

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;

            // Play a random sound from the current speaker's voice list every N letters
            if (letterCount % step == 0 && currentTypingSounds != null && currentTypingSounds.Length > 0)
            {
                int i = Random.Range(0, currentTypingSounds.Length);
                AudioManager.instance.PlayMusic(currentTypingSounds[i]);
            }

            letterCount++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // Instantly shows the full text of the line that is being typed
    private void CompleteCurrentLine()
    {
        StopAllCoroutines();
        dialogueArea.text = activeLine.line;
        isTyping = false;
    }

    void EndDialogue()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}