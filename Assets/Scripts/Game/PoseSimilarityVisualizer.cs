using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseSimilarityVisualizer : MonoBehaviour
{
    readonly Color similarColor = new(0.0f, 1.0f, 0.0f, 0.5f);
    readonly Color dissimilarColor = new(1.0f, 0.0f, 0.0f, 0.5f);

    GameObject HeadVisualizer;
    GameObject LeftHandVisualizer;
    GameObject RightHandVisualizer;

    void Start()
    {
        GameObject visualizerPrefab = Resources.Load<GameObject>("SimilarityVisualization");

        HeadVisualizer = Instantiate(visualizerPrefab, transform.Find("HeadPoint"));
        HeadVisualizer.transform.localScale = Vector3.one * 1.5f;

        LeftHandVisualizer = Instantiate(visualizerPrefab, transform.Find("LeftHandPoint"));
        RightHandVisualizer = Instantiate(visualizerPrefab, transform.Find("RightHandPoint"));
    }

    public void UpdateHeadSimilarity(float similarity)
    {
        HeadVisualizer.GetComponent<Renderer>().material.color = Color.Lerp(dissimilarColor, similarColor, similarity);
    }

    public void UpdateLeftHandSimilarity(float similarity)
    {
        LeftHandVisualizer.GetComponent<Renderer>().material.color = Color.Lerp(dissimilarColor, similarColor, similarity);
    }

    public void UpdateRightHandSimilarity(float similarity)
    {
        RightHandVisualizer.GetComponent<Renderer>().material.color = Color.Lerp(dissimilarColor, similarColor, similarity);
    }
}
