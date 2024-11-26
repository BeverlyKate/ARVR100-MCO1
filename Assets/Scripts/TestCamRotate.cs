using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamRotate : MonoBehaviour
{
    public float rotationSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.parent.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            transform.parent.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
