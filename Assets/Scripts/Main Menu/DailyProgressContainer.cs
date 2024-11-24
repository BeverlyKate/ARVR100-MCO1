using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyProgressContainer : MonoBehaviour
{
    [SerializeField] private GameObject dailyProgressPanel;

    void Start()
    {
        //if (CurrentAccount.Account == null)
        //{
        //    return;
        //}

        //var dailyProgress = CurrentAccount.Account.DailyProgress;
        var dailyProgressPanelComp = dailyProgressPanel.GetComponent<DailyProgressPanel>();
        dailyProgressPanelComp.SetProgress(new List<int>() {  1, 2, 3, 4, 5, 6, 7 });
    }
}
