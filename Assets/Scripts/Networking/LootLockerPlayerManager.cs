using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LootLockerPlayerManager : MonoBehaviour
{
    public Leaderboard leaderboard;

    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        //StartCoroutine(FetchLeaderboardRoutine());
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Player was logged in");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.Log("Could not start session");
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    void Update()
    {

    }
}
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LootLockerPlayerManager : MonoBehaviour
{
    [SerializeField] string email;
    [SerializeField] string password;
    [SerializeField] bool rememberMe;

    void Start()
    {
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
    {
        bool done = false;

        LootLockerSDKManager.CheckWhiteLabelSession((whiteLabelSessionResponse) =>
        {
            if (whiteLabelSessionResponse)
            {
                Debug.Log("White Label session is valid, starting game session...");
                StartGameSession();
            }
            else
            {
                Debug.Log("White Label session is NOT valid, starting guest session...");
                LootLockerSDKManager.StartGuestSession((guestSessionResponse) =>
                {
                    if (guestSessionResponse.success)
                    {
                        Debug.Log("Guest session started");
                        PlayerPrefs.SetString("PlayerID", guestSessionResponse.player_id.ToString());
                        done = true;
                    }
                    else
                    {
                        Debug.Log("Could not start guest session");
                        done = true;
                    }
                });
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    void StartGameSession()
    {
        LootLockerSDKManager.WhiteLabelLogin(email, password, rememberMe, (loginResponse) =>
        {
            if (!loginResponse.success)
            {
                Debug.Log("Error while logging in: " + loginResponse.errorData.message);
                return;
            }

            LootLockerSDKManager.StartWhiteLabelSession((sessionResponse) =>
            {
                if (!sessionResponse.success)
                {
                    Debug.Log("Error while starting LootLocker session: " + sessionResponse.errorData.message);
                    return;
                }

                Debug.Log("White Label session started successfully");
                PlayerPrefs.SetString("PlayerID", sessionResponse.player_id.ToString());
            });
        });
    }

    void Update()
    {

    }
}
*/