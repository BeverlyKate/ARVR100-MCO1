using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsContainer : MonoBehaviour
{
    [SerializeField] private GameObject leaderboard;

    async void Start()
    {
        var leaderboardComp = leaderboard.GetComponent<Leaderboard>();

        GroupLeaderboardDB gldb = new();
        foreach (var row in await gldb.FetchLeaderboardRows())
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

    public void OnBackButtonClicked()
    {
        CurrentAccount.Account = null;
        SceneManager.LoadScene("Login Scene");
    }
}
