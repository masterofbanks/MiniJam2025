using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
public class PlayerManager : MonoBehaviour
{
    public Leaderboard leaderboard;
    public TMP_InputField playerNameInputField;
    public bool inGame;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        yield return LoginRoutine();
        if (!inGame)
        {
            yield return leaderboard.FetchTopHighScoresRoutine();

        }
    }

    public void SetPlayerName()
    {

        string n = playerNameInputField.text;
        if (n.Length > 7)
        {
            n = n.Substring(0, 7);
        }

        LootLockerSDKManager.SetPlayerName(n, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully Set Player Name");
            }

            else
            {
                Debug.Log("Could not set player name" + response.errorData);
            }
        });
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

    // Update is called once per frame
    void Update()
    {

    }
}
