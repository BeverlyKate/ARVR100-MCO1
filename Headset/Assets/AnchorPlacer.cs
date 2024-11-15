using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorPlacer : MonoBehaviour
{
    ARAnchorManager anchorManager;

    [SerializeField] private GameObject prefabToAnchor;
    [SerializeField] private float forwardOffset = 2f;

    // Start is called before the first frame update
    void Start()
    {
        anchorManager = GetComponent<ARAnchorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector3 spawnPos =
                Camera.main.ScreenPointToRay(Input.GetTouch(0).position)
                .GetPoint(forwardOffset);
            AnchorObject(spawnPos);
        }
    }

    public void AnchorObject(Vector3 worldPos)
    {
        GameObject newAnchor = new GameObject("NewAnchor");
        newAnchor.transform.parent = null;
        newAnchor.transform.position = worldPos;
        newAnchor.AddComponent<ARAnchor>();

        GameObject obj = Instantiate(prefabToAnchor, newAnchor.transform);
        obj.transform.localPosition = Vector3.zero;
    }
}

