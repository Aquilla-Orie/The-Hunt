using UnityEngine;
using System.Collections;
using LootLocker.Requests;
using TMPro;
using Photon.Pun;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    private string globalKillsLeaderboardKey = "globalKills";
    private string globalDeathsLeaderboardKey = "globalDeaths";
    private string globalDamageLeaderboardKey = "globalDamage";

    [SerializeField] private TextMeshProUGUI playerNamesText;
    [SerializeField] private TextMeshProUGUI playerKillsText;
    [SerializeField] private TextMeshProUGUI playerDamageText;
    [SerializeField] private TextMeshProUGUI playerDeathsText;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            FetchLeaderboardData();
        }
    }

    private void FetchLeaderboardData()
    {
        StartCoroutine(FetchLeaderboardRoutine(globalKillsLeaderboardKey));
        StartCoroutine(FetchLeaderboardRoutine(globalDeathsLeaderboardKey));
        StartCoroutine(FetchLeaderboardRoutine(globalDamageLeaderboardKey));
    }

    private IEnumerator FetchLeaderboardRoutine(string leaderboardKey)
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            if (response.success)
            {
                UpdateLeaderboardData(response.items, leaderboardKey);
                done = true;
            }
            else
            {
                Debug.Log("Failed to fetch leaderboard: " + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => !done);
    }

    public void SubmitScore(int score, string leaderboardKey)
    {
        StartCoroutine(SubmitScoreRoutine(score, leaderboardKey));
    }

    private IEnumerator SubmitScoreRoutine(int score, string leaderboardKey)
    {
        string playerID = PhotonNetwork.NickName;

        bool done = false;
        LootLockerSDKManager.SubmitScore(playerID, score, leaderboardKey, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed to upload score: " + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => !done);

        // Fetch leaderboard after submitting the score
        yield return StartCoroutine(FetchLeaderboardRoutine(leaderboardKey));
    }

    private void UpdateLeaderboardData(LootLockerLeaderboardMember[] leaderboardMembers, string leaderboardKey)
    {
        playerNamesText.text = "Player\n";
        playerKillsText.text = "Kills\n";
        playerDamageText.text = "Damage\n";
        playerDeathsText.text = "Deaths\n";

        

        for (int i = 0; i < leaderboardMembers.Length; i++)
        {
            string playerName = string.IsNullOrEmpty(leaderboardMembers[i].player.name) ? leaderboardMembers[i].player.id.ToString() : leaderboardMembers[i].player.name;
            int playerScore = leaderboardMembers[i].score;

            playerNamesText.text += playerName + "\n";
            // Update UI
            if (leaderboardKey.Equals(globalKillsLeaderboardKey))
            {
                playerKillsText.text += playerScore.ToString() + "\n";
            }

            if (leaderboardKey.Equals(globalDamageLeaderboardKey))
            {
                playerDamageText.text += playerScore.ToString() + "\n";
            }

            if (leaderboardKey.Equals(globalDeathsLeaderboardKey))
            {
                playerDeathsText.text += playerScore.ToString() + "\n";
            }
            
        }
    }

    public override void OnJoinedRoom()
    {
        FetchLeaderboardData();
    }
}