using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject cow;
    public GameObject cam;

    public TMP_Text Vel;
    

    public TMP_InputField Gravity;
    //public TMP_Text Mass;
    public TMP_InputField Drag;
    public TMP_InputField AngularDrag;
    public TMP_InputField Mass;
    public TMP_InputField camAngle;

    float lastVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vel.text = "Velocity: " + cow.GetComponent<Rigidbody>().velocity.magnitude.ToString("F2");
        
        cow.GetComponent<Rigidbody>().drag = float.Parse(Drag.text);
        cow.GetComponent<Rigidbody>().angularDrag = float.Parse(AngularDrag.text);
        cow.GetComponent<Rigidbody>().mass = float.Parse(Mass.text);
        Physics.gravity = new Vector3(0, float.Parse(Gravity.text), 0);

        // set  angle of camera with input field
        cam.transform.eulerAngles = new Vector3(float.Parse(camAngle.text), 0, 0);






       

    }
}
