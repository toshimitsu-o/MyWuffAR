using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The different states the dog can be in.
public enum DogState {Idle = 0, Sitting = 1, Fetching = 2, PickingUp = 3};

public class DogScript : MonoBehaviour
{
    // A link to the object where the camera is
    public GameObject player;
    //A link to the dog ball object we created before 
    public GameObject ball;
    //the bone in the skeelton we attach the ball to when it's picked up 
    public GameObject attachBone;
    //The movement speed of the dog.
    public float speed = 2.0f;
    //A link to the animator component 
    private Animator animator;
    //The current state for the dog, as defiend by the enum at the top. 
    private DogState dogState;
    //This defines how close the ball needs to be to a thing for it to count as 'close'
    private float ballClose = 2.0f;
    //A varaible to store if the ball has a rigidbody and if it is kinematic
    private bool ballKinematic = false;
    //Does the dog currently have the ball
    private bool hasBall = false;
    //a timer for the pickup behvaiour
    private float pickUpTimer = 0;
    //a timer for the sit behvaiour
    private float sitTimer = 0;
    //a timer for the idle behvaiour
    private float idleTimer = 0;

    // start is called before the first frame update
    void Start() {
        //get the link to the animator component
        animator = GetComponent<Animator>();
        //set the intial state to be idle
        dogState = DogState.Idle;
    }

    //function that returns if the ball needs fetching right now 
    bool DoesBallNeedFetching() {
        //check if ball is near camera 
        float dist = Vector3.Distance(transform.position, ball.transform.position);
        if (dist < ballClose * 4) {
            //ball is too far away, the dog needs to go and get it.
            return true;
        }
        return false;
    }

    //uses a raycast to stick the dog model to our floor. should be called whenever the object is moved.
    void StickToFloor() {
        RaycastHit hit;
        Vector3 raycastPos = transform.position;
        raycastPos.y += 1;
        if (Physics.Raycast(raycastPos, new Vector3(0, -1, 0), out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"))) {
            //get the y position of the raycast hit
            raycastPos.y = hit.point.y;
            //Apply the new position
            transform.position = raycastPos;
        }
    }

    // Update is called once per frame
    void Update() {
        ////////
        // Sitting Behaviour
        ////////
        if (dogState == DogState.Sitting) {
            //if the dog is sitting, we countdown the sitting timer each frame.
            //this timer is set when the state is set.
            sitTimer -= Time.deltaTime;
            //if the timer has reached o, our dog should stand up again.
            if (sitTimer <= 0) {
                idleTimer = Random.Range(3, 5);
                dogState = DogState.Idle;
            }
            //our dog should also stand up again if the ball now needs to be fetched.
            if (DoesBallNeedFetching()) {
                dogState = DogState.Idle;
            }
        }

        ////////
        // Fetching Behaviour
        ////////
        if (dogState == DogState.Fetching) {
        //if we don't have the ball yet, than this means we need to go and get it.
            if (hasBall == false) {
                //check how far from the ball the dog is
                float dist = Vector3.Distance(transform.position, ball.transform.position);
                if (dist < ballClose) {
                    //if the dog is close to the ball, that means we need to pick it up.
                    //we set the state back to idle for the transition.
                    dogState = DogState.Idle;
                } else {
                    //the dog is not near the ball, we need to move towards ball
                    //first we must rotate to face the ball
                    Vector3 ballPos = ball.transform.position;
                    //we can elimate vertical rotation by setting our look target to be at the same
                    // y axis value as the dog.
                    ballPos.y = transform.position.y;
                    //call lookat to point our object at the ball. 
                    transform.LookAt(ballPos);
                    //This will cause the dog ot look in the oppersite direction however 
                    //we can fix this by just turn it 180 degrees after calling lookup. 
                    transform.Rotate(new Vector3(0, 180, 0));

                    //we are now facing the ball, now we need to move towards it.
                    //get the vector towards the ball
                    Vector3 direction = ball.transform.position - transform.position;
                    //get rid any Y axi movement so we don't get a flying dog 
                    direction.y = 0;
                    //always normalise 
                    direction.Normalize();
                    //apply this vector to the dog's position with the time pass and movement speed. 
                    transform.Translate(direction * Time.deltaTime * speed, Space.World);
                    //lastly we need to keep our down on the ground, so we call our function at the //the bottom that will stick it to the ground.
                    StickToFloor();
                }

            //now we handle if we have picked up the ball
            //this is similar, but instead of traveling to the ball, we travel to the player object
            } else {
                //check how far from the player the dog is
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist < 4.0f) {
                    //if the dog is close to the player, that means we need to drop the ball /and we set the state back to idle, but with the timer set so it stays idle for a while.
                    idleTimer = Random.Range(3, 5);
                    dogState = DogState.Idle;
                    //here we drop the ball by setting it's parent object to mull and renabling physics it if has it.
                    // destoy ball component
                    ball.transform.position = new Vector3(0, -10, 0);
                    // ball.transform.parent = null;
                    // if (ball.GetComponent<Rigidbody>()) {
                    //     //ball.GetComponent<Rigidbody>().isKinematic = ballKinematic;
                    // }
                    //lastly we update our flag to say we've dropped the ball 
                    hasBall = false;
                    //Lastly we handle if we have the ball but have'nt got back to the player object yet
                } else {
                    //first we must rotate to face the ball
                    Vector3 playerPos = player.transform.position;
                    //we can elimate vertical rotation by setting our look target to be at the same
                    // y axis value as the dog.
                    playerPos.y = transform.position.y;
                    //call lookat to point our object at the ball. 
                    transform.LookAt(playerPos);
                    //This will cause the dog ot look in the oppersite direction however 
                    //we can fix this by just turn it 180 degrees after calling lookup. 
                    transform.Rotate(new Vector3(0, 180, 0));

                    //we are now facing the player, now we need to move towards it.
                    //get the vector towards the player
                    Vector3 direction = player.transform.position - transform.position;
                    //get rid any y axi movement so we don't get a flying dog 
                    direction.y = 0;
                    //always normalise 
                    direction.Normalize();
                    //apply this vector to the dog's position with the time pass and movement speed. 
                    transform.Translate(direction * Time.deltaTime * speed, Space.World);
                    //lastly we need to keep our down on the ground, so we call our function at the //the bottom that will stick it to the ground.
                    StickToFloor();
                }
            }
        }
        ////////
        // Pickup Behaviour
        ////////
        if (dogState == DogState.PickingUp) {
            //our pickup animation takes 1 second to play, during this we need to actually pick up 
            //the object at e.5 seconds and attach it to the dog object.

            //so we decrement a timer to track this 
            pickUpTimer -= Time.deltaTime;
            //if the timer is less than e, we have finished the animation and need to go back to idle
            if (pickUpTimer <= 0) {
                dogState = DogState.Idle;
            //else if we are 0.5 seconds into the animation and haven't got the ball yet, we need to pick it up
            } else if (pickUpTimer <= 0.5 && hasBall == false) {
                //we set the parent object of the ball to be the bone object we linked
                ball.transform.parent = attachBone.transform;
                //we then set the bone to local position 0,0, so it is centred on the bone.
                ball.transform.localPosition = new Vector3(0, 0, 0);
                //we disable any physics that might be trying to run on the object, so it doesn't fall out.
                if (ball.GetComponent<Rigidbody>()) {
                    //since we don't know if this object is supposed to have physics or not, we store it's original value here 
                    //so they aren't accidently enabled when dropped later. 
                    ballKinematic = ball.GetComponent<Rigidbody>().isKinematic;
                    ball.GetComponent<Rigidbody>().isKinematic = true;
                }
                //lastly we set our flag to say we've picked it up. 
                hasBall = true;
            }
            
        }
        ////////
        // Idle/Transition Behaviour
        ////////
        // Our idle state acts as both a do nothing state and as a transition state between other animations.
        if (dogState == DogState.Idle) {
            //check if the idle animation is currently playing. If if it not, don't do anything and wait for it to fully transition.
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                //get how far dog is to the ball
                float distToBall = Vector3.Distance(transform.position, ball.transform.position);
                //check if we have the ball
                if (hasBall) {
                    //if we do, then we need to be returning that to the person /we must have just picked it up
                    dogState = DogState.Fetching;
                    animator.SetInteger("DogState", (int)DogState.Fetching);
                //if not, check if the ball needs fetching
                } else if (DoesBallNeedFetching()) {
                    //check if dog is near the ball
                    if (distToBall < ballClose) {
                        //lf so, pick up the ball. 
                        dogState = DogState.PickingUp;
                        animator.SetInteger("DogState", (int)DogState.PickingUp);

                        //set the pickup timer to 1 second since that's how long the animation takes. 
                        pickUpTimer = 1.0f;
                    } else {
                        //if dog is not near the ball, send it to fetch 
                        dogState = DogState.Fetching;
                        animator.SetInteger("DogState", (int)DogState.Fetching);
                    }
                } else {
                    //in any other case, we are just standing idle not trying to transition 
                    //countdown the idle timer 
                    idleTimer -= Time.deltaTime;
                    //if the idle timer is less than 0, transition to the sitting animation 
                    if (idleTimer <= 0) {
                        //Iset a random time to sit for 
                        sitTimer = Random.Range(5, 10);
                        dogState = DogState.Sitting;
                        animator.SetInteger("DogState", (int)DogState.Sitting);
                    }
                }
            //in the case we are in the idle state, but the idle animation has not started running yet //just set the idle animation to run, just in case.
            //otherwise we are just waiting for it to start or transition fully.
            } else {
                animator.SetInteger("DogState", (int)DogState.Idle);
            }
        }
    }
}