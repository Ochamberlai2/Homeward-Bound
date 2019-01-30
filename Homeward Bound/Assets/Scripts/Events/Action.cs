using System.Collections;
using UnityEngine;

/*
 * Base class for actions. These are simple one step actions such as playing an animation or lerping to a position
 */
public abstract class Action : ScriptableObject
{

    public bool ActionCompleted { get; private set; }

    public void Fire()
    {
        ActionFunction();
    }

    protected abstract IEnumerator ActionFunction();
    
}
