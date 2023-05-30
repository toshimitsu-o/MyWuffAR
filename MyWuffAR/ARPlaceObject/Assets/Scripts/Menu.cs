using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    // Text to output score
    public TMP_Text message;
    // Speed of movement
    float speed = 220f;
    // Destination X position
    float destinationX = 750f;
    // Position property
    public Vector3 pos;
    // State of moving
    public bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set the start position
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            PopOutMenu();
        }
    }

    // Button call this function as click event
    public void MoveMenu(){
        moving = true;
    }

    // Move the object across
    void PopOutMenu() {
        //var pos = transform.position;
            var newX = pos.x - Time.time * speed;
            transform.position = new Vector3(newX, pos.y, pos.z);
            if (transform.position.x <= destinationX) {
                moving = false;
                message.text = "stopped";
            }
            message.text = transform.position.ToString();
    }
}
