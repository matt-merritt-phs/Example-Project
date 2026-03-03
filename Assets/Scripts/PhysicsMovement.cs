using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    // By making this private, it will not appear in the editor
    private Rigidbody rb;

    public float moveSpeed;
    public float turnSpeed;
    public float jumpForce;

    public bool onGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Automatically looks for a Rigidbody component on the current GameObject
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // ------- Movement -------

        // Creates one vector where X is the input from A and D, and Z is the input from X and Y
        Vector3 movementInput = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");

        // Start from current location, and then slightly adjusts the position by adding on the inputs scaled by speed and adjusted for framerate
        Vector3 targetPosition = rb.position + movementInput * moveSpeed * Time.deltaTime;

        // Moves the rigidbody to its target position
        rb.MovePosition(targetPosition);

        // ------- Rotation -------

        // Clockwise rotations
        if (Input.GetKey(KeyCode.E))
        {
            // Start from the current rotation, converting it to a Vector
            Vector3 currentRotation = rb.rotation.eulerAngles;

            // Add to the y-coordinate of the angle to rotate clockwise
            Vector3 targetRotation = currentRotation + Vector3.up * turnSpeed * Time.deltaTime;

            // Need to change the target rotation to a quaternion before passing it to rigidbody
            rb.MoveRotation(Quaternion.Euler(targetRotation));
        }

        // ------- Jumping -------

        // Add a force impulse to the player that pushes them upwards
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround) 
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    // Check if the player has come into contact with the ground.
    // If they have, they should be able to jump.
    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == "Ground") 
        {
            onGround = true;
        }
    }

    // Check if the player has left the ground.
    // If they have, they cannot jump anymore.
    void OnCollisionExit(Collision collision) 
    {
        if (collision.gameObject.tag == "Ground") 
        {
            onGround = false;
        }
    }
}
