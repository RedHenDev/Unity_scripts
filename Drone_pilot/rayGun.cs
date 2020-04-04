using UnityEngine;

public class rayGun : MonoBehaviour
{

    private LineRenderer beam;

    private Camera cam;

    private Vector3 origin;
    private Vector3 endPoint;
    private Vector3 mousePos;

    void Start()
    {
        // Grabbed our laser.
        beam = this.gameObject.AddComponent<LineRenderer>();
        beam.startWidth = 0.2f;
        beam.endWidth = 0.2f;

        // Grab the main camera.
        cam = Camera.main;
    }

    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            checkLaser();
        }
        else beam.enabled = false;
    }


    void checkLaser()
    {

        // Finding the origin and end point of laser.
        origin = this.transform.position +
            this.transform.forward * 0.5f * this.transform.lossyScale.z;

        // Finding mouse pos in 3D space.
        mousePos = Input.mousePosition;
        mousePos.z = 300f;
        endPoint = cam.ScreenToWorldPoint(mousePos);

        // Find direction of beam.
        Vector3 dir = endPoint - origin;
        dir.Normalize();

        // Are we hitting any colliders?
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, 300f))
        {
            // If yes, then set endpoint to hit-point.
            endPoint = hit.point;

            // Has this hit object got a rigidbody? 
            if (hit.transform.GetComponent<Rigidbody>() != null)
            {
                hit.transform.GetComponent<Rigidbody>().
                    AddForce(dir, ForceMode.Impulse);
            }
        }

        // Set end point of laser.
        beam.SetPosition(0, origin);
        beam.SetPosition(1, endPoint);
        // Draw the laser!
        beam.enabled = true;
        
    }


}
