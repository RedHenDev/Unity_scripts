using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dronePilot : MonoBehaviour
{

    public float speed = 10f;
    public float mouseSpeed = 10f;

    void Start()
    {
        
    }

    float mx;
    void Update()
    {
        // Key inputs.
        if (Input.GetKey("w")){
            this.transform.Translate(Vector3.forward *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("s")){
            this.transform.Translate(-Vector3.forward *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("a")){
            this.transform.Translate(-Vector3.right *
             speed * Time.deltaTime);
        }
        if (Input.GetKey("d")){
            this.transform.Translate(Vector3.right *
             speed * Time.deltaTime);
        }

        // Add new mouse position on X-axis.
        mx += Input.GetAxisRaw("Mouse X") * mouseSpeed;
        // Create new rotation based on mouse change.
        this.transform.localRotation = 
        Quaternion.AngleAxis(mx, Vector3.up);

    }
}
