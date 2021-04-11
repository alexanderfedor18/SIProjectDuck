using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    //public variables

    public float moveSpeed;
    public float jumpHeight;
    public float startTimeBetweenShots;
    //layer containing the only objects we want checkGrounded raycast to hit
    public LayerMask platformLayerMask;
    public GameObject gun;
    public GameObject projectile;
    public Transform shotPoint;


    private float timeBetweenShots;


    //private variables
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private BoxCollider2D boxCollider2d;
    //x movement direction
    private float moveInput;
    //if player has jumped
    private bool jumped = false;
    private bool shooting = false;

    //Mouse and Controller Position
    private Vector2 mousePosition;
    private Vector2 controllerPosition;
    private bool isMouse;



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

        IsGrounded();
        ApplyShoot();
        if (isMouse)
        {
            Vector3 relativePosition = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
            RotateGun(new Vector2(relativePosition.x, relativePosition.y));
        } else
        {
            if (controllerPosition != new Vector2(0,0))
            RotateGun(controllerPosition);
        }


    }

    //calls for a total of 50 frames every second, used for physics calculations
    void FixedUpdate()
    {
        if (rb != null)
        {
            applyMovement();
            ApplyJump();
        }
        else
            Debug.LogWarning("Rigidbody not attatched to player " + gameObject.name);
        

    }

    //used to update the moveInput float value with which direction the input wants the character to move
    public void OnMovement(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<float>();
    }

    //used to update the jumped bool to check if the player has jumped
    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
        
    }


    //updates mouse position
    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        isMouse = true;

    }


    //updates controller position
    public void OnControllerMove(InputAction.CallbackContext context)
    {
        controllerPosition = context.ReadValue<Vector2>();
        isMouse = false;
    }


    public void OnShoot(InputAction.CallbackContext context)
    {

        
        if (context.ReadValue<float>() > 0)
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }
        
        
    }

    //uses the move set through the PlayerControls to set the velocity of the player
    void applyMovement()
    {
        //checks if moving left then if moving right
        if (moveInput < 0)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (moveInput > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }


    void ApplyJump()
    {
        if (jumped && IsGrounded())
        {
            rb.velocity = Vector2.up*jumpHeight;
            //rb.AddForce(new Vector2(0, jumpHeight));
        }
        jumped = false;
    }

    //checks if the player is touching the ground using a boxCast
    bool IsGrounded()
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

        //draws box showing boxcast
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);


        return rayCasthit.collider != null;
    }



    private void RotateGun(Vector2 position)
    {
        float rotation = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        Quaternion temp = Quaternion.Euler(0f, 0f, rotation);
        gun.transform.rotation = temp;
        
    }

    private void ApplyShoot()
    {
        if (timeBetweenShots <= 0)
        {
            if (shooting)
            {
                Instantiate(projectile, shotPoint.position, gun.transform.rotation);
                timeBetweenShots = startTimeBetweenShots;
            }
        } else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }


}