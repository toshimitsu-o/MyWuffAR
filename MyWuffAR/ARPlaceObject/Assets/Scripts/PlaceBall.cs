using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceBall : MonoBehaviour
{
    ARRaycastManager raycastManager;
    GameObject ballInstance = null;

    GameObject dogInstance = null;

    // Scaling variables
    float startDistance;
    Vector3 startScale;
    bool touching = false;

    public GameObject ballPrefab;

    public GameObject dogPrefab;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

    }

    // Update is called once per frame
    void Update()
    {
        // Check how many touch inuts we have. We only want 1.
        if (Input.touchCount == 1)
        {
            // Stores the list of object the ray hits
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            // Run the raycast and get back if it was successful. If so, out hits list should be populated.
            bool success = raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon);

            // Debug.Log("Hit: " + success + " Count: " + hits.Count);

            // If we hit something, place the cube
            if (success && hits.Count > 0)
            {
                // Get the intersection point where we touched, this is order from closest to farest, get the firtst point.
                Vector3 hitPosition = hits[0].pose.position;
                var ballPosition =  hitPosition + new Vector3(0, 1.0f, 0);

                // Make or move ball in the scene.
                if (ballInstance == null)
                {
                    ballInstance = Instantiate(ballPrefab, ballPosition, Quaternion.identity);
                }
                else
                {
                    ballInstance.transform.position = ballPosition;
                }

                // Instatiate the dog prefab in the scene.
                if (dogInstance == null)
                {
                    var dogPosition = hitPosition + new Vector3(2.0f, 0, 0);
                    dogInstance = dogPrefab;
                    dogInstance.transform.position = dogPosition;
                
                }
            }
        }

        if (Input.touchCount == 2)
        {
            // is this the first frame of a double touch?
            if (this.touching == false) {
                this.touching = true;
                // Get the starting distance between the two touches
                // We treat this as our scale = current position
                this.startDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                this.startScale = this.ballInstance.transform.localScale;
            } else {
                // get the current touch distance
                float newDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                // Calculate a percentage compared to the starting touches
                float percentage = newDistance / this.startDistance;
                // Make sure the percentage doesn't end up as 0 or negative
                if (percentage < 0.01f) {
                    percentage = 0.01f;
                }
                // apply that percentage to the starting scale and put onto cube
                if (ballInstance != null) {
                    this.ballInstance.transform.localScale = this.startScale * percentage;
                }
            }
        }
        // once we stop touching again, disable the scaling touch flag
        if (Input.touchCount == 0) {
            this.touching = false;
        }

    }
}
