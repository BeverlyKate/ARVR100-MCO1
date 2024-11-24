using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsContainer : MonoBehaviour
{
    [SerializeField] private GameObject leaderboard;

    void Start()
    {
        var leaderboardComp = leaderboard.GetComponent<Leaderboard>();

        GroupLeaderboardDB gldb = new();
        foreach (var row in gldb.FetchLeaderboardRows())
        {
            leaderboardComp.AddRow(row.GroupName, row.GroupScore);
        }
    }

    public void OnGameStartButtonClicked() {
        SceneManager.LoadScene("Gameplay Scene");
    }

    public void OnMyGymButtonClicked() {
        SceneManager.LoadScene("Gyms Scene");
    }
}
