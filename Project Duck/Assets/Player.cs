using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2d;
    public float maxSpeed = 6;
    public float acceleration = 3;
    public float jumpHeight = 5;

    //x movement direction
    private float moveInput;
    //if player has jumped
    private bool jumped = false;
    //layer containing the only objects we want checkGrounded raycast to hit
    public LayerMask platformLayerMask;


    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
        Debug.Log(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
        Debug.Log(jumped);
    }






    //Awake is called before Start
    void Awake()
    {
        //initialize variables
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();


    }
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        isGrounded();
    }
    //calls for a total of 50 frames every second, used for physics calculations
    void FixedUpdate()
    {

        if (rb != null)
        {
            applyMovement();
            applyJump();
        }
        else
            Debug.LogWarning("Rigidbody not attatched to player " + gameObject.name);
    }


    //uses the move set through the PlayerControls to set the velocity of the player
    void applyMovement()
    {
        //checks if moving left then if moving right
        if (moveInput < 0)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            //rb.AddForce(new Vector2(-maxSpeed, 0), ForceMode2D.Impulse);
        }
        else if (moveInput > 0)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            //rb.AddForce(new Vector2(maxSpeed, 0), ForceMode2D.Impulse);
        } else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }

    void applyJump()
    {
        if (jumped && isGrounded())
        {
            rb.velocity = Vector2.up*jumpHeight;
            //rb.AddForce(new Vector2(0, jumpHeight));
        }
    }

    //checks if the player is touching the ground
    bool isGrounded()
    {

        
        float extraHeightText = .1f;
        RaycastHit2D rayCasthit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        
        Color rayColor;
        if (rayCasthit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);


        return rayCasthit.collider != null;
    }
    
}
