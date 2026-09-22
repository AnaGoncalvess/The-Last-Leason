using System;
using UnityEngine;

/// <summary>
/// Singleton de pontuação que se auto-instancia e persiste entre cenas.
/// Assine ScoreManager.OnScoreChanged para atualizar UI (ver ScoreUI.cs).
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ScoreManager (Auto)");
                instance = go.AddComponent<ScoreManager>();
            }

            return instance;
        }
    }

    public static event Action<int> OnScoreChanged;

    public int CurrentScore { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Add(int amount)
    {
        CurrentScore += amount;
        OnScoreChanged?.Invoke(CurrentScore);
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke(CurrentScore);
    }
}
