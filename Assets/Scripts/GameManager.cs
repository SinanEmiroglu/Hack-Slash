using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
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

    public void Begin()
    {
        StartCoroutine(BeginGame());
    }

    private IEnumerator BeginGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        yield return new WaitUntil(() => operation.isDone);

        Debug.Log("Started");
        PlayerManager.Instance.SpawnPlayerCharacters();
    }
}