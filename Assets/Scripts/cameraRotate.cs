using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotate : MonoBehaviour
{
    // write script to rotate camera up and down using w and s keys
    public GameObject cam;
    public float speed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float rotationAngle = 0.0f;
    public float rotationAngleMax = 90.0f;
    public float rotationAngleMin = -90.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // write script to rotate camera up and down using w and s keys
        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.Rotate(Vector3.right * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.Rotate(Vector3.left * speed);
        }

        



    }
}
