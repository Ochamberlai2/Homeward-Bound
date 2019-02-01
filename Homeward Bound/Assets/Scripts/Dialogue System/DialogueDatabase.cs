using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName ="Dialogue Database", menuName = "Create Dialogue Database")]
public class DialogueDatabase : SerializedScriptableObject
{
    public Dictionary<string, string> database;
}
