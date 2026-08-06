using TMPro;
using UnityEngine;

public class PlayerCellStat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerScore;

    public int Score;

    public void SetData(string name, int score, Color color)
    {
        _playerName.text = name.ColorWith(color);
        SetScore(score);
    }

    public void SetScore(int score)
    {
        Score = score;
        _playerScore.text = score.ToString();
    }
}
