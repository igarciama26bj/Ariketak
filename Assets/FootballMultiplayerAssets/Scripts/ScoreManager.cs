using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int blueScore, redScore;
    [SerializeField] private TMP_Text blueScoreText, redScoreText;
    
    public void IncreaseScore(FootballTeam team)
    {
        if (team == FootballTeam.Blue)
        {
            blueScore++;
            blueScoreText.SetText("", blueScore);
        } else if (team == FootballTeam.Red)
        {
            redScore++;
            redScoreText.SetText("", redScore);
        }
    }
}
