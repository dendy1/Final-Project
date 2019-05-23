using System.Collections.Generic;
using UnityEngine;

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
}
