using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctions : MonoBehaviour
{
    public static List<GameObject> GetChildrenOf(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.GetComponentInChildren<Transform>())
        {
            children.Add(child.gameObject);
        }
        return children;
    }
}
