using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyProgressPanel : MonoBehaviour
{
    readonly List<string> daysOfTheWeek = new() { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

    public void SetProgress(List<int> progress)
    {
        for (int i = 0; i < progress.Count; i++)
        {
            var child = transform.GetChild(i);
            child.Find("Label").GetComponent<TMPro.TextMeshProUGUI>().text = daysOfTheWeek[i];
            child.Find("Panel").transform.Find("Reps").GetComponent<TMPro.TextMeshProUGUI>().text = progress[i].ToString();
        }
    }
}