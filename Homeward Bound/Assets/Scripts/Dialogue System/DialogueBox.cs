using UnityEngine.EventSystems;
using UnityEngine;

public class DialogueBox : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueController.Instance.dialogueBoxClicked.Invoke();
    }
}
