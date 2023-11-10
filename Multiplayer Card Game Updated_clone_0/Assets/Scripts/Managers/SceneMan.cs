using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    public static SceneMan Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGivenScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void LoadMatchMaking()
    { 
        SceneManager.LoadScene(2);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    public void UnloadMatchMaking()
    {
        SceneManager.UnloadSceneAsync(1);
    }
}
