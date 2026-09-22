using TMPro;
using UnityEngine;

/// <summary>
/// Plug-and-play: coloque este script num objeto de UI com TextMeshProUGUI (ou TMP_Text)
/// para exibir a pontuação atual automaticamente, sem precisar arrastar referências.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private string prefix = "Pontos: ";

    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateLabel;
        UpdateLabel(ScoreManager.Instance.CurrentScore);
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateLabel;
    }

    private void UpdateLabel(int score)
    {
        label.text = prefix + score;
    }
}
