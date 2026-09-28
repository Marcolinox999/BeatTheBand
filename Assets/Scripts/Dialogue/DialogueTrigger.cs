using System;
using System.Collections.Generic;
using UnityEngine;
 

//https://youtu.be/DOP_G5bsySA?si=uuXa9kLHUoXqDYu6
[Serializable] public class DialogueCharacter
{
    public string name;
    public Sprite portrait;
}
 
[Serializable] public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
 
[Serializable] public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
 
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
 
    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerDialogue();
        }
    }
}