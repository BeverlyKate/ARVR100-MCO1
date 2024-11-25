using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    const float SIMILARITY_THRESHOLD = 0.5f;

    readonly Queue<GameObject> pendingPoses = new();
    readonly PoseSimilarityComputer poseSimilarityComputer = new(SIMILARITY_THRESHOLD);

    int repCount = 0;
    GameObject currentPose;
    bool pauseSimlarityUpdate = false;

    [SerializeField, Header("Poses")] 
    List<GameObject> poses = new();

    [SerializeField, Header("Game Elements")]
    GameObject poseParent;
    [SerializeField]
    GameObject goodFeedbackPanel;

    [SerializeField, Header("Text Elements")]
    TextMeshProUGUI gameRepCountText;
    [SerializeField] 
    TextMeshProUGUI currentPoseText, todayRepCountText;

    void Start()
    {
        ReplenishPoses();
        SetNextPose();

        UpdateRepsUI();
        UpdateRepsTodayUI();
    }

    void Update()
    {
        if (pauseSimlarityUpdate)
        {
            return;
        }

        Pose currPosePos = currentPose.GetComponent<Pose>();

        poseSimilarityComputer.Update(
            currPosePos.HeadPosition,
            currPosePos.LeftHandPosition,
            currPosePos.RightHandPosition,
            Camera.main.transform.position,
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch),
            OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch)
        );

        if (poseSimilarityComputer.IsSimilar)
        {
            StartCoroutine(MoveToNextPose());
        }

        UpdateModelSimilarityUI();
    }

    // Handlers

    public void ExitGame()
    {
        // TODO: Update the current player's daily streak

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
        if (poseParent.transform.childCount > 0)
        {
            Destroy(poseParent.transform.GetChild(0).gameObject);
        }

        GameObject nextPosePrefab = pendingPoses.Dequeue();
        currentPose = Instantiate(nextPosePrefab, poseParent.transform);
        currentPose.AddComponent<Pose>();
        currentPose.AddComponent<PoseSimilarityVisualizer>();

        currentPoseText.text = "Current Pose: <b>" + nextPosePrefab.name + "</b>";
    }

    // UI

    void UpdateModelSimilarityUI()
    { 
        PoseSimilarityVisualizer simVis = currentPose.GetComponent<PoseSimilarityVisualizer>();

        simVis.UpdateHeadSimilarity(poseSimilarityComputer.HeadSimilarity);
        simVis.UpdateLeftHandSimilarity(poseSimilarityComputer.LeftHandSimilarity);
        simVis.UpdateRightHandSimilarity(poseSimilarityComputer.RightHandSimilarity);
    }

    void UpdateRepsUI()
    {
        gameRepCountText.text = repCount.ToString();
    }

    void UpdateRepsTodayUI()
    {
        // TODO: Fetch the current player's daily streak and set
        //todayRepCountText.text = ;
    }
}