using UnityEngine;

public class ImprovedMovement : MonoBehaviour
{
    // By making this private, it will not appear in the editor
    private Rigidbody rb;

    public float moveSpeed;
    public float turnSpeed;
    public float jumpForce;

    public bool onGround;

    public Vector3 spawnPosition;
    
    // By putting this data here, it can be accessed and updated in multiple methods in my script.
    public Vector3 movementInput;
    public Vector3 rotationInput;
    public bool playerAlive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Automatically looks for a Rigidbody component on the current GameObject
        rb = GetComponent<Rigidbody>();

        spawnPosition = transform.position;
        playerAlive = true;
    }

    // Update is called once per frame, used for getting accurate inputs
    void Update()
    {
        // ------- Movement -------
        movementInput = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");

        // ------- Rotation (Keyboard) -------
        if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q))
        {
            rotationInput = Vector3.zero;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationInput = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            rotationInput = Vector3.down;
        }

        // ------- Jumping -------
        // Keep this in Update method for better input detection.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    // FixedUpdate is called once per physics frame, used for interacting with physics objects
    void FixedUpdate()
    {
        // ------- Movement -------
        Vector3 targetPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // ------- Rotation -------
        Vector3 targetRotation = rb.rotation.eulerAngles + rotationInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Euler(targetRotation));

        // ------- Respawning -------
        if (rb.position.y < 0)
        {
            rb.position = spawnPosition;
        }
        if (!playerAlive)
        {
            rb.position = spawnPosition;
            playerAlive = true;
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
        if (collision.gameObject.tag == "Hazard")
        {
            playerAlive = false;
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
