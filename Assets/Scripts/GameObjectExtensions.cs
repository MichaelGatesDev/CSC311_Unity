using UnityEngine;

public static class GameObjectExtensions
{
    public static GameObject FindParentWithTag(this GameObject childObject, string tag)
    {
        var t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.CompareTag(tag))
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }
}