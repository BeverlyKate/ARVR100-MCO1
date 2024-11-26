using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyProgressContainer : MonoBehaviour
{
    [SerializeField] private GameObject dailyProgressPanel;

    void Start()
    {
        if (CurrentAccount.Account == null)
        {
            return;
        }

        var dailyProgress = CurrentAccount.Account.DailyProgress;
        var dailyProgressPanelComp = dailyProgressPanel.GetComponent<DailyProgressPanel>();
        dailyProgressPanelComp.SetProgress(dailyProgress);
    }
}
