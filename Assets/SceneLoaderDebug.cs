using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public static class SceneLoaderDebug
{
    public static void Load(string name)
    {
        Debug.LogError($"[SceneLoad] LoadScene('{name}') | frame {Time.frameCount}\n{new StackTrace(true)}");
        SceneManager.LoadScene(name);
    }

    public static void Load(int index)
    {
        Debug.LogError($"[SceneLoad] LoadScene({index}) | frame {Time.frameCount}\n{new StackTrace(true)}");
        SceneManager.LoadScene(index);
    }

    public static AsyncOperation LoadAsync(string name)
    {
        Debug.LogError($"[SceneLoad] LoadSceneAsync('{name}') | frame {Time.frameCount}\n{new StackTrace(true)}");
        return SceneManager.LoadSceneAsync(name);
    }

    public static AsyncOperation LoadAsync(int index)
    {
        Debug.LogError($"[SceneLoad] LoadSceneAsync({index}) | frame {Time.frameCount}\n{new StackTrace(true)}");
        return SceneManager.LoadSceneAsync(index);
    }
}