/*using UnityEngine;
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

        playerNamesText.text = "Player\n";
        playerKillsText.text = "Kills\n";
        playerDamageText.text = "Damage\n";
        playerDeathsText.text = "Deaths\n";
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
                Debug.LogError("Failed to fetch " + leaderboardKey + " leaderboard: " + response.errorData.message);
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
                Debug.LogError("Failed to upload score: " + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => !done);

        yield return StartCoroutine(FetchLeaderboardRoutine(leaderboardKey));
    }

    private IEnumerator FetchPlayerScore(string playerName, string leaderboardKey, TextMeshProUGUI targetText)
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            if (response.success)
            {
                int playerScore = 0;
                foreach (var item in response.items)
                {
                    string name = string.IsNullOrEmpty(item.player.name) ? item.player.id.ToString() : item.player.name;
                    if (name == playerName)
                    {
                        playerScore = item.score;
                        break;
                    }
                }
                targetText.text = playerScore.ToString();
                done = true;
            }
            else
            {
                Debug.LogError("Failed to fetch " + leaderboardKey + " leaderboard: " + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => !done);
    }

    private void UpdateLeaderboardData(LootLockerLeaderboardMember[] leaderboardMembers, string leaderboardKey)
    {
        playerNamesText.text = "Player\n";

        for (int i = 0; i < leaderboardMembers.Length; i++)
        {
            string playerName = string.IsNullOrEmpty(leaderboardMembers[i].player.name) ? leaderboardMembers[i].player.id.ToString() : leaderboardMembers[i].player.name;
            int playerScore = leaderboardMembers[i].score;

            playerNamesText.text += playerName + "\n";

            if (leaderboardKey.Equals(globalKillsLeaderboardKey))
            {
                playerKillsText.text = "Kills\n";
                playerKillsText.text += playerScore.ToString() + "\n";

                StartCoroutine(FetchPlayerScore(playerName, globalDamageLeaderboardKey, playerDamageText));
                StartCoroutine(FetchPlayerScore(playerName, globalDeathsLeaderboardKey, playerDeathsText));
            }
            else if (leaderboardKey.Equals(globalDamageLeaderboardKey))
            {
                StartCoroutine(FetchPlayerScore(playerName, globalKillsLeaderboardKey, playerKillsText));
                StartCoroutine(FetchPlayerScore(playerName, globalDeathsLeaderboardKey, playerDeathsText));

                playerKillsText.text = "Kills\n";

                playerDamageText.text = "Damage\n";
                playerDamageText.text += playerScore.ToString() + "\n";

            }
            else if (leaderboardKey.Equals(globalDeathsLeaderboardKey))
            {
                StartCoroutine(FetchPlayerScore(playerName, globalKillsLeaderboardKey, playerKillsText));
                StartCoroutine(FetchPlayerScore(playerName, globalDamageLeaderboardKey, playerDamageText));

                playerDeathsText.text = "Deaths\n";
                playerDeathsText.text += playerScore.ToString() + "\n";
            }
        }
    }

    public override void OnJoinedRoom()
    {
        FetchLeaderboardData();
    }
}
*/

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
            SubmitScore(0, globalKillsLeaderboardKey);
            SubmitScore(0, globalDeathsLeaderboardKey);
            SubmitScore(0, globalDamageLeaderboardKey);
        }
    }

    public void FetchLeaderboardData()
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

        yield return StartCoroutine(FetchLeaderboardRoutine(leaderboardKey));
    }

    public void UpdateLeaderboardData(LootLockerLeaderboardMember[] leaderboardMembers, string leaderboardKey)
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
}