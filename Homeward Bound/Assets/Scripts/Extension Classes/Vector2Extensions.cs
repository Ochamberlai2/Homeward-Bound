using UnityEngine;

/*
 * Extension methods for the Unity Vector2 class
 */
public static class Vector2Extensions
{
   
    public static Vector2 SetY(this Vector2 vector, float yPosition)
    {
        return new Vector2(vector.x, yPosition);
    }

    public static Vector2 Flatten(this Vector2 vector)
    {
        return new Vector2(vector.x, 0);
    }
}
