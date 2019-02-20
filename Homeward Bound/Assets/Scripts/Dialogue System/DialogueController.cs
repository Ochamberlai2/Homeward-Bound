using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;


/*
 * Controls the games dialogue system including showing dialogue to the screen and getting dialogue lines
 */
public class DialogueController : MonoBehaviour
{
    #region Singleton
    private static DialogueController instance;

    public static DialogueController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogueController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(DialogueController).Name;
                    instance = obj.AddComponent<DialogueController>();
                }
            }
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private float dialogueWritingSpeed = 0.05f;

    private GameObject dialogueBox;
    private TextMeshProUGUI textComponent;

    [SerializeField]
    private DialogueDatabase dialogueDatabase;

    private string currentDialogueKey;


    public System.Action dialogueBoxClicked;

    public void Awake()
    {
        dialogueBox = GameObject.Find("DialogueBox");
        textComponent = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogueBoxClicked += DialogueClicked;
        dialogueBox.SetActive(false);
    }
    /*
     * Queries the dialogue database for the correct line 
     */
    public string RetrieveDialogueLine(string key)
    {
        string value;
        dialogueDatabase.database.TryGetValue(key, out value);
        if(value == null)
        {
            throw new KeyNotFoundException(key + " is not a key in the current dialogue database");
        }

        return value;
    }

    /*
     * sets the active state of the dialogue panel
     */
    public void ToggleDialoguePane(bool toggleState)
    {
        dialogueBox.SetActive(toggleState);
    }
    /*
     * Adds a "typing" style animation to the text while it is being rendered to the screen
     */
    private IEnumerator writeTextToScreen(string dialogueLine)
    {

        string currentText = "";
        for(int i = 0;  i < dialogueLine.Length; i++)
        {
            currentText += dialogueLine[i];
            textComponent.text = currentText;
            yield return new WaitForSeconds(dialogueWritingSpeed);
        }
    }
    /*
     * Handles getting the required dialogue line, showing the panel and then displaying the text to the panel
     */ 
    public void ShowDialogue(string key)
    {
        string dialogueLine = RetrieveDialogueLine(key);
        currentDialogueKey = key;
        ToggleDialoguePane(true);

        StartCoroutine(writeTextToScreen(dialogueLine));
    }

    /*
     * Called when the dialogueBoxClicked event is called
     */
    public void DialogueClicked()
    {
        string dialogueLine = RetrieveDialogueLine(currentDialogueKey);
        if (textComponent.text != dialogueLine)
        {
            StopAllCoroutines();
            textComponent.text = dialogueLine;
        }
        else
        {
            ToggleDialoguePane(false);
        }
    }
}
