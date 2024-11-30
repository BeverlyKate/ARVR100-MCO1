using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseSimilarityComputer
{
    readonly float threshold;

    public float HeadSimilarity { get; private set; }
    public float LeftHandSimilarity { get; private set; }
    public float RightHandSimilarity { get; private set; }

    public PoseSimilarityComputer(float threshold)
    {
        this.threshold = threshold;

        HeadSimilarity = 0;
        LeftHandSimilarity = 0;
        RightHandSimilarity = 0;
    }

    public bool IsSimilar
    {
        get => Similarity >= threshold;
    }

    public double Similarity
    {
        get => (HeadSimilarity + LeftHandSimilarity + RightHandSimilarity) / 3.0;
    }

    public void Update(
        Vector3 poseHeadVec, 
        Vector3 poseLeftHandVec, 
        Vector3 poseRightHandVec, 
        Vector3 playerHeadVec,
        Vector3 playerLeftHandVec,
        Vector3 playerRightHandVec
    )
    {
        Vector3 poseCentroid = GetCentroid(poseHeadVec, poseLeftHandVec, poseRightHandVec);
        Vector3 playerCentroid = GetCentroid(playerHeadVec, playerLeftHandVec, playerRightHandVec);

        HeadSimilarity = GetSimilarity(poseHeadVec - poseCentroid, playerHeadVec - playerCentroid);
        LeftHandSimilarity = GetSimilarity(poseLeftHandVec - poseCentroid, playerLeftHandVec - playerCentroid);
        RightHandSimilarity = GetSimilarity(poseRightHandVec - poseCentroid, playerRightHandVec - playerCentroid);

        Debug.Log("HeadSimilarity: " + HeadSimilarity + " LeftHandSimilarity: " + LeftHandSimilarity + " RightHandSimilarity " + RightHandSimilarity);
    }

    Vector3 GetCentroid(Vector3 head, Vector3 leftHand, Vector3 rightHand)
    {
        return (head + leftHand + rightHand) / 3;
    }

    float GetSimilarity(Vector3 v1, Vector3 v2)
    {
        float dotProduct = Vector3.Dot(v1, v2);
        float magnitudeV1 = v1.magnitude;
        float magnitudeV2 = v2.magnitude;

        return dotProduct / (magnitudeV1 * magnitudeV2);
    }
}
