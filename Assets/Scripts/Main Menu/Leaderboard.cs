using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private GameObject content;

    void Start()
    {
        var headerGObj = content.transform.Find("LeaderboardHeader");
        headerGObj.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Rank";
        headerGObj.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Gym";
        headerGObj.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Score";
    }

    public void AddRow(string groupName, int groupScore) {
        GameObject listItem = Instantiate(rowPrefab, content.transform);
        listItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (content.transform.childCount - 3).ToString();
        listItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = groupName;
        listItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = groupScore.ToString();
    }
}
