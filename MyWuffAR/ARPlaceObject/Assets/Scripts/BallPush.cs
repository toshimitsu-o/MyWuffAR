using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPush : MonoBehaviour
{
    // Max distance to capture
    public float distance = 200;
    // Power of the push
    public float power = 100;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // When hit space bar
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, distance))

            //if (hit.collider != null) // Check if the ray hit a collider
            {
                if (hit.collider.gameObject.CompareTag("Ball"))
                {
                    // Add force to the ball to push forward
                    hit.collider.attachedRigidbody.AddForce(transform.forward * power);
                    // Add force to the ball to push up
                    hit.collider.attachedRigidbody.AddForce(new Vector3(0, 1, 0) * power);
                }
                else if (hit.collider.gameObject.CompareTag("BallGreen"))
                {
                    // Add force to the ball to push forward
                    hit.collider.attachedRigidbody.AddForce(transform.forward * power);
                    // Add force to the ball to push up
                    hit.collider.attachedRigidbody.AddForce(new Vector3(0, 1, 0) * power);
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
}
