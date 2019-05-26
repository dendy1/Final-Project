using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utils
{
    public static List<T> MergeArrays<T>(params T[][] arrays)
    {
        List<T> result = new List<T>();
        
        foreach (var array in arrays)
        {
            result.AddRange(array);
        }

        return result;
    }
    
    public static List<GameObject> FindGameObjectsWithTags(params string[] tags)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (var tag in tags)
        {
            result.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }

        return result;
    }
    
    public static void SetText(float value, Text text, string prefix = "")
    {
        text.text = string.Format("{0}{1}", prefix, Mathf.Round(value));
    }
    
    public static void SetText(string value, Text text, string prefix = "")
    {
        text.text = string.Format("{0}{1}", prefix, value);
    }
}
