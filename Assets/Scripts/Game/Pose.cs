using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pose : MonoBehaviour
{
    public Vector3 HeadPosition
    {
        get => transform.Find("HeadPoint").position;
    }

    public Vector3 LeftHandPosition
    {
        get => transform.Find("LeftHandPoint").position;
    }

    public Vector3 RightHandPosition
    {
        get => transform.Find("RightHandPoint").position;
    }
}
