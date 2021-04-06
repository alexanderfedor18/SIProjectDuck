using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    
    private PlayerControls controls;
    public Rigidbody2D rb;
    public float maxSpeed = 6;
    public float acceleration = 3;

    //x movement direction
    float move;




    //Awake is called before Start
    void Awake()
    {
        //initialize variables
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();

        //sets an action to a certain method
        controls.PlayerActions.Jump.performed += ctx => Jump();
        controls.PlayerActions.Movement.performed += ctx => Movement(ctx.ReadValue<float>());
        controls.PlayerActions.Movement.performed += ctx => move = ctx.ReadValue<float>();
        controls.PlayerActions.Movement.canceled += ctx => move = 0;
        

    }
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {

    }

    //calls for a total of 50 frames every second, used for physics calculations
    void FixedUpdate()
    {

        if (rb != null)
        {
            applyMovement();
        }
        else
            Debug.LogWarning("Rigidbody not attatched to player " + gameObject.name);
    }

    //allows the PlayerControls object to be enabled and disabled
    void OnEnable()
    {
        controls.Enable();
    }
    void OnDisable()
    {
        controls.Disable();
    }

    //uses the move set through the PlayerControls to set the velocity of the player
    void applyMovement()
    {
        //checks if moving left then if moving right
        if (move < 0)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            //rb.AddForce(new Vector2(-maxSpeed, 0), ForceMode2D.Impulse);
        }
        else if (move > 0)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            //rb.AddForce(new Vector2(maxSpeed, 0), ForceMode2D.Impulse);
        } else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }


    }



    /// <summary>
    /// debug code
    /// </summary>
    void Jump()
    {
        Debug.Log("We jumping bois");
    }
    void Movement(float direction)
    {
        Debug.Log("Time to move" + direction);
    }

}
