using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isPlaying;
    private bool isThrowingBallExist;

    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
    public bool IsThrowingBallExist { get => isThrowingBallExist; set => isThrowingBallExist = value; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        isPlaying = true;
    }
}
