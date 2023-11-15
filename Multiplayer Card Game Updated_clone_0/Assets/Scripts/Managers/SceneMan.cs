using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
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
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadSceneAsync(SceneIndex);
        
        var NetMan = GameObject.FindObjectOfType<NetworkManager>();
        if (NetMan != null)
        { 
            var NetManObj = GameObject.FindObjectOfType<NetworkManager>().gameObject;
            Destroy(NetMan);
            Destroy(NetManObj);
        }

    }

    public void LoadMatchMakingScene(int MatchMakingSceneIndex)
    { 
        SceneManager.LoadScene(3);
        SceneManager.LoadSceneAsync(MatchMakingSceneIndex, LoadSceneMode.Additive);
    }

    public void UnloadMatchMakingScene(int SceneIndex)
    {
        var MatchMaker = GameObject.FindAnyObjectByType<MainUIMatchMake>();
        if (MatchMaker != null) 
        {
            if (MatchMaker.currentLobby != null)
            {
                CloseLobby(MatchMaker.currentLobby.Id);
            }
        }

        var currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene);
        SceneManager.LoadSceneAsync(SceneIndex);
        var NetMan = GameObject.FindObjectOfType<NetworkManager>();
        var NetManObj = GameObject.FindObjectOfType<NetworkManager>().gameObject;
        if (NetMan != null)
        {
            Destroy(NetMan);
            Destroy(NetManObj);
        }
    }

    private async void CloseLobby(string ID)
    {
        await LobbyService.Instance.DeleteLobbyAsync(ID);
    }

    public void UnloadMatchMaking(int MatchMakingSceneIndex)
    {
        SceneManager.UnloadSceneAsync(MatchMakingSceneIndex);
    }

    public void OnClickExitGame()
    { 
        Application.Quit();
    }
}
