using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
public class Leaderboard : MonoBehaviour
{
    int leaderboardID = 30428;
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;


    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID.ToString(), (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully Uploaded Score");
                done = true;
            }

            else
            {
                Debug.Log("Failed" + response.errorData);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }


    public IEnumerator FetchTopHighScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardID.ToString(), 10, 0, (response) =>
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

                    tempPlayerScores += members[i].score + "\n";
                    tempPlayerNames += "\n";
                }

                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;

            }

            else
            {
                Debug.Log("Failed" + response.errorData);
                done = true;
            }




        });

        yield return new WaitWhile(() => done == false);
    }
}
