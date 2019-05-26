using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public Animator transitionAnim;
    private static SceneTransitions _instance;
    private static readonly int End = Animator.StringToHash("End");

    private void Awake()
    {
        _instance = this;

        if (!_instance)
        {
            Debug.LogError("You must have SceneTransition script attached to GameObject");
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            transitionAnim.SetTrigger(End);
        }
    }

    public static void LoadScene(string sceneName, float delay = 0)
    {
        var coroutine = _instance.LoadSceneCoroutine(sceneName, delay);
        _instance.StartCoroutine(coroutine);
    }
    
    public static void LoadScene(int sceneIndex, float delay = 0)
    {
        var coroutine = _instance.LoadSceneCoroutine(sceneIndex, delay);
        _instance.StartCoroutine(coroutine);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        transitionAnim.SetTrigger(End);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex, float delay)
    {
        transitionAnim.SetTrigger(End);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}
