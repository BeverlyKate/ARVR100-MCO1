using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseSimilarityComputer
{
    readonly float threshold;
    readonly Dictionary<string, float> jointWeights;

    public float HeadSimilarity { get; private set; }
    public float LeftHandSimilarity { get; private set; }
    public float RightHandSimilarity { get; private set; }

    public PoseSimilarityComputer(float threshold, Dictionary<string, float> jointWeights = null)
    {
        this.threshold = threshold;

        HeadSimilarity = 0;
        LeftHandSimilarity = 0;
        RightHandSimilarity = 0;

        // Set default weights if none are provided
        this.jointWeights = jointWeights ?? new Dictionary<string, float>
        {
            { "Head", 1f },
            { "LeftHand", 1f },
            { "RightHand", 1f }
        };
    }

    public bool IsSimilar
    {
        get => Similarity >= threshold;
    }

    public double Similarity
    {
        get
        {
            float totalWeight = jointWeights["Head"] + jointWeights["LeftHand"] + jointWeights["RightHand"];
            return (HeadSimilarity * jointWeights["Head"] +
                    LeftHandSimilarity * jointWeights["LeftHand"] +
                    RightHandSimilarity * jointWeights["RightHand"]) / totalWeight;
        }
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

        // Normalize to account for scale invariance
        poseHeadVec = NormalizeToScale(poseHeadVec, poseCentroid);
        poseLeftHandVec = NormalizeToScale(poseLeftHandVec, poseCentroid);
        poseRightHandVec = NormalizeToScale(poseRightHandVec, poseCentroid);

        playerHeadVec = NormalizeToScale(playerHeadVec, playerCentroid);
        playerLeftHandVec = NormalizeToScale(playerLeftHandVec, playerCentroid);
        playerRightHandVec = NormalizeToScale(playerRightHandVec, playerCentroid);

        // Align global orientation
        Quaternion alignmentRotation = GetAlignmentRotation(poseHeadVec - poseCentroid, playerHeadVec - playerCentroid);
        playerHeadVec = RotateVector(playerHeadVec - playerCentroid, alignmentRotation);
        playerLeftHandVec = RotateVector(playerLeftHandVec - playerCentroid, alignmentRotation);
        playerRightHandVec = RotateVector(playerRightHandVec - playerCentroid, alignmentRotation);

        HeadSimilarity = GetSimilarity(poseHeadVec - poseCentroid, playerHeadVec);
        LeftHandSimilarity = GetSimilarity(poseLeftHandVec - poseCentroid, playerLeftHandVec);
        RightHandSimilarity = GetSimilarity(poseRightHandVec - poseCentroid, playerRightHandVec);

        Debug.Log("HeadSimilarity: " + HeadSimilarity + " LeftHandSimilarity: " + LeftHandSimilarity + " RightHandSimilarity: " + RightHandSimilarity);
    }

    Vector3 GetCentroid(Vector3 head, Vector3 leftHand, Vector3 rightHand)
    {
        return (head + leftHand + rightHand) / 3;
    }

    Vector3 NormalizeToScale(Vector3 position, Vector3 centroid)
    {
        Vector3 normalized = position - centroid;
        return normalized.sqrMagnitude > 0 ? normalized.normalized : Vector3.zero;
    }

    Quaternion GetAlignmentRotation(Vector3 poseDirection, Vector3 playerDirection)
    {
        if (poseDirection.sqrMagnitude == 0 || playerDirection.sqrMagnitude == 0)
        {
            return Quaternion.identity;
        }
        return Quaternion.FromToRotation(playerDirection.normalized, poseDirection.normalized);
    }

    Vector3 RotateVector(Vector3 vector, Quaternion rotation)
    {
        return rotation * vector;
    }

    float GetSimilarity(Vector3 v1, Vector3 v2)
    {
        float magnitudeV1 = v1.magnitude;
        float magnitudeV2 = v2.magnitude;

        // Handle zero magnitude gracefully
        if (magnitudeV1 == 0 || magnitudeV2 == 0)
        {
            return 0;
        }

        float dotProduct = Vector3.Dot(v1.normalized, v2.normalized);
        return Mathf.Clamp(dotProduct, -1f, 1f); // Clamp to handle floating-point errors
    }
}
