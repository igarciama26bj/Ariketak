using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    private readonly NetworkVariable<int>
        blueScore = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server),
        redScore = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private TMP_Text blueScoreText, redScoreText;

    public override void OnNetworkSpawn()
    {
        blueScore.OnValueChanged += BlueScoreChanged;
        redScore.OnValueChanged += RedScoreChanged;

        BlueScoreChanged(blueScore.Value, blueScore.Value);
        RedScoreChanged(redScore.Value, redScore.Value);
    }

    public void IncreaseScore(FootballTeam team) => RequestIncreaseScoreRpc(team);

    [Rpc(SendTo.Server)]
    private void RequestIncreaseScoreRpc(FootballTeam team, RpcParams rpcParams = default)
    {
        if (team == FootballTeam.Blue)
        {
            blueScore.Value++;
        } else if (team == FootballTeam.Red)
        {
            redScore.Value++;
        }
    }

    private void BlueScoreChanged(int oldValue, int newValue)
    {
        blueScoreText.SetText("{0}", newValue);
    }

    private void RedScoreChanged(int oldValue, int newValue)
    {
        redScoreText.SetText("{0}", newValue);
    }
}
