using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    #region Physics Variables
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    #endregion

    #region Ground Variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    #endregion

    private Vector3 velocity;

    private Rigidbody rb; 

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    #region Bools
    private bool canMove = true;
    private bool isGrounded;
    private bool isMoving;
    #endregion

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // checks if the groundCheck object is touching the groundMask layer

        // Resetting the velocity if the player is on the ground
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Getting the inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Crerate a direction vector
        Vector3 move = transform.right * x + transform.forward * z; // right is the red axis, forward is the blue axis

        // Move the player
        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            // Actually jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        // Falling down
        velocity.y += gravity * Time.deltaTime;

        // Excecuting the jump
        controller.Move(velocity * Time.deltaTime);

        if(lastPosition != gameObject.transform.position && isGrounded)
        {
            isMoving = true;
            // will use later
        }
        else
        {
            isMoving = false;
            // will use later

        }


        lastPosition = gameObject.transform.position;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                canMove = false; // Disable movement when colliding with a wall
                Debug.Log("Player has collided with a wall. Movement stopped.");
            }
        }
    }
}
