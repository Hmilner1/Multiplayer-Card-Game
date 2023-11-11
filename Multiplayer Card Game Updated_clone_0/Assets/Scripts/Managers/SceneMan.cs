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

    public void LoadMatchMakingScene(int MatchMakingSceneIndex)
    { 
        SceneManager.LoadScene(3);
        SceneManager.LoadSceneAsync(MatchMakingSceneIndex, LoadSceneMode.Additive);
    }

    public void UnloadMatchMaking(int MatchMakingSceneIndex)
    {
        SceneManager.UnloadSceneAsync(MatchMakingSceneIndex);
    }
}
