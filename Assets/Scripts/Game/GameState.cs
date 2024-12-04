using Oculus.Interaction.Body.PoseDetection;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    int repCount = 0;
    GameObject currentPose;
    bool pauseSimlarityUpdate = false;

    [SerializeField, Header("Poses")]
    List<GameObject> poses = new();

    [SerializeField, Header("Game Elements")]
    GameObject poseParent;
    [SerializeField]
    GameObject goodFeedbackPanel;
    [SerializeField]
    PoseFromBody poseFromBody; // Reference to the real-time pose tracker

    [SerializeField, Header("Text Elements")]
    TextMeshProUGUI gameRepCountText;
    [SerializeField]
    TextMeshProUGUI currentPoseText, todayRepCountText;

    readonly Queue<GameObject> pendingPoses = new();

    void Start()
    {
        if (poseFromBody == null)
        {
            Debug.LogError("PoseFromBody component is not assigned in the inspector.");
            return;
        }

        ReplenishPoses();
        SetNextPose();

        UpdateRepsUI();
        UpdateRepsTodayUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ExitGame();
        }

        if (pauseSimlarityUpdate || currentPose == null) return;

        // Get the BodyPoseComparerActiveState component from the current pose
        var comparer = currentPose.GetComponent<BodyPoseComparerActiveState>();
        if (comparer == null)
        {
            Debug.LogError("BodyPoseComparerActiveState is missing on the current pose.");
            return;
        }

        if (comparer.Active)
        {
            Debug.Log("Pose matched successfully!");
            StartCoroutine(MoveToNextPose());
        }
    }

    // Handlers

    public async void ExitGame()
    {
        PlayerDB pdb = new();
        await pdb.UpdateStreak(CurrentAccount.Account);
        await pdb.UpdateReps(CurrentAccount.Account, 5);

        SceneManager.LoadScene("Main Menu Scene", LoadSceneMode.Single);
    }

    // Game Flow

    IEnumerator MoveToNextPose()
    {
        pauseSimlarityUpdate = true;

        repCount += 1;
        UpdateRepsUI();

        goodFeedbackPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        goodFeedbackPanel.SetActive(false);

        if (pendingPoses.Count == 0)
        {
            ReplenishPoses();
        }

        SetNextPose();

        pauseSimlarityUpdate = false;
    }

    void ReplenishPoses()
    {
        List<GameObject> shuffledPoses = new(poses);

        System.Random rng = new();
        int n = shuffledPoses.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (shuffledPoses[n], shuffledPoses[k]) = (shuffledPoses[k], shuffledPoses[n]);
        }

        foreach (var pose in shuffledPoses)
        {
            pendingPoses.Enqueue(pose);
        }
    }

    void SetNextPose()
    {
        Debug.Log("Set next pose called");
        if (poseParent.transform.childCount > 0)
        {
            Destroy(poseParent.transform.GetChild(0).gameObject);
        }

        GameObject nextPosePrefab = pendingPoses.Dequeue();
        currentPose = Instantiate(nextPosePrefab, poseParent.transform);

        // Set "Pose A" for BodyPoseComparerActiveState
        var comparer = currentPose.GetComponent<BodyPoseComparerActiveState>();
        if (comparer != null)
        {
            comparer.InjectPoseA(poseFromBody);
        }
        else
        {
            Debug.LogError("BodyPoseComparerActiveState is missing on the instantiated pose prefab.");
        }

        currentPoseText.text = "Current Pose: <b>" + nextPosePrefab.name + "</b>";
    }

    // UI

    void UpdateRepsUI()
    {
        gameRepCountText.text = repCount.ToString();
    }

    void UpdateRepsTodayUI()
    {
        // TODO: Fetch the current player's daily streak and set
        // todayRepCountText.text = ;
    }
}
