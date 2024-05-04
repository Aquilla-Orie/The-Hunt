/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using Photon.Pun;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    string leaderboardKey = "globalKills";
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;

    void Start()
    {

    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PhotonNetwork.NickName;
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardKey, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully uploaded score");
                done = true;
            }
            else
            {
                Debug.Log("Failed" + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchTopHighscoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[] members = response.items;

                for (int i = 0; i < members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores += members[i].kills + "\n";
                    tempPlayerNames += "\n";
                }
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.Log("Failed" + response.errorData.message);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }
}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Leaderboard : MonoBehaviourPunCallbacks
{
    private string globalKillsLeaderboardKey = "globalKills";
    private string globalDeathsLeaderboardKey = "globalDeaths";
    private string globalDamageLeaderboardKey = "globalDamage";
    private string localKillsLeaderboardKey = "localKills";
    private string localDeathsLeaderboardKey = "localDeaths";
    private string localDamageLeaderboardKey = "localDamage";

    [SerializeField] private TextMeshProUGUI playerNamesText;
    [SerializeField] private TextMeshProUGUI playerKillsText;
    [SerializeField] private TextMeshProUGUI playerDamageText;
    [SerializeField] private TextMeshProUGUI playerDeathsText;

    private Dictionary<int, LootLockerLeaderboardMember> localLeaderboard = new Dictionary<int, LootLockerLeaderboardMember>();

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            ResetLocalScores();
            FetchLeaderboardData();
        }
    }

    private void ResetLocalScores()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SubmitScoreRoutine(localKillsLeaderboardKey, 0));
            StartCoroutine(SubmitScoreRoutine(localDeathsLeaderboardKey, 0));
            StartCoroutine(SubmitScoreRoutine(localDamageLeaderboardKey, 0));
        }
    }

    private void FetchLeaderboardData()
    {
        StartCoroutine(FetchLeaderboardRoutine());
    }

    private IEnumerator FetchLeaderboardRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(globalKillsLeaderboardKey, 50, 0, (response) =>
        {
            if (response.success)
            {
                UpdateLeaderboardData(response.items);
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

    public void SubmitKill()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(SubmitScoreRoutine(localKillsLeaderboardKey, 1));
            StartCoroutine(SubmitScoreRoutine(globalKillsLeaderboardKey, 1));
        }
    }

    public void SubmitDamage(int damage)
    {
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(SubmitScoreRoutine(localDamageLeaderboardKey, damage));
            StartCoroutine(SubmitScoreRoutine(globalDamageLeaderboardKey, damage));
        }
    }

    public void SubmitDeath()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartCoroutine(SubmitScoreRoutine(localDeathsLeaderboardKey, 1));
            StartCoroutine(SubmitScoreRoutine(globalDeathsLeaderboardKey, 1));
        }
    }

    private IEnumerator SubmitScoreRoutine(string leaderboardKey, int score)
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

        FetchLeaderboardData();
    }

    private void UpdateLeaderboardData(LootLockerLeaderboardMember[] leaderboardMembers)
    {
        playerNamesText.text = "Player\n";
        playerKillsText.text = "Kills\n";
        playerDamageText.text = "Damage\n";
        playerDeathsText.text = "Deaths\n";

        localLeaderboard.Clear();

        foreach (var member in leaderboardMembers)
        {
            string playerName = string.IsNullOrEmpty(member.player.name) ? member.player.id.ToString() : member.player.name;
            int playerKills = member.kills;
            int playerDamage = member.damage;
            int playerDeaths = member.deaths;

            if (!localLeaderboard.ContainsKey(member.player.id))
            {
                localLeaderboard.Add(member.player.id, member);
            }

            playerNamesText.text += playerName + "\n";
            playerKillsText.text += playerKills.ToString() + "\n";
            playerDamageText.text += playerDamage.ToString() + "\n";
            playerDeathsText.text += playerDeaths.ToString() + "\n";
        }
    }

    public override void OnJoinedRoom()
    {
        FetchLeaderboardData();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        FetchLeaderboardData();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        FetchLeaderboardData();
    }
}