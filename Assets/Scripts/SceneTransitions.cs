using System.Collections;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneName;

    private static SceneTransitions _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Load();
        }
    }

    public static void Load(float delay = 0, params GameObject[] objects)
    {
        _instance.StartCoroutine(_instance.LoadSceneCoroutine(delay, objects));
    }

    private IEnumerator LoadSceneCoroutine(float delay, params GameObject[] objects)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(delay);
        Scenes.Load(sceneName, objects);
    }
}
