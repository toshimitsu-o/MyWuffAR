using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Speed of movement
    public float speedMove = 20.0f;
    // Speed of turning
    public float speedTurn = 600.0f;

    // Update is called once per frame
    void Update()
    {
        // Mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float timePassed = Time.deltaTime;

        // Horizontal Rotation around Y axis
        this.transform.Rotate(new Vector3(0, 1, 0), mouseX * timePassed * speedTurn, Space.World);

        // Vertical Rotation around left axis
        //this.transform.Rotate(-this.transform.right, mouseY * timePassed * speedTurn, Space.World);

        // Keyboard input
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(this.transform.forward * timePassed * speedMove, Space.World);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(-this.transform.forward * timePassed * speedMove, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(-this.transform.right * timePassed * speedMove, Space.World);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(this.transform.right * timePassed * speedMove, Space.World);
        }
    }
}
