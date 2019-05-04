using UnityEngine;

public class WorldObjectDialogueDisplay : MonoBehaviour
{
    [SerializeField]
    private string dialogueKey;

    private void OnMouseDown()
    {
        DialogueController.Instance.ShowDialogue(dialogueKey);
    }
}
