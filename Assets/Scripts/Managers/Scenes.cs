using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Scenes
{
    private static List<GameObject> _objects;

    public static void Load(string name, List<GameObject> objects = null)
    {
        _objects = objects;
        SceneManager.LoadScene(name);
    }

    public static void Load(string name, params GameObject[] objects)
    {
        _objects = new List<GameObject>();
        _objects.AddRange(objects);
        SceneManager.LoadScene(name);
    }

    public static List<GameObject> GetSceneParameters
    {
        get { return _objects; }
    }

    public static void AddObject(GameObject obj)
    {
        if (_objects == null)
            _objects = new List<GameObject>();
        
        _objects.Add(obj);
    }
}
