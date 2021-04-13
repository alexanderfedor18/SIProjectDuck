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

    public int health;

    private float timeBetweenShots;


    //private variables
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private BoxCollider2D boxCollider2d;
    [SerializeField]
    private GameObject mainBox;
    //x movement direction
    private float moveInput;
    //if player has jumped
    private bool jumped = false;
    private bool shooting = false;

    //Mouse and Controller Position
    private Vector2 mousePosition;
    private Vector2 controllerPosition;
    private bool isMouseAndKeyboard = true;

    private PlayerConfig playerConfig;
    private PlayerControls controls;

    private Vector2 directionInput;
    private bool dashing;
    public float dashSpeed;
    public float startDashTime;
    private float dashTime;

    private int playerNum;

    private List<int> bullets;
    [SerializeField]
    private float startReloadCooldown = 3;
    private float timeBetweenReload;

    //Awake is called before Start
    void Awake()
    {
        //initialize variables
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        controls = new PlayerControls();
        dashTime = startDashTime;
        timeBetweenReload = startReloadCooldown;
        bullets = new List<int>();
    }
    

    public void InitializePlayer(PlayerConfig pc, int playerNum)
    {
        playerConfig = pc;
        playerConfig.Input.onActionTriggered += Input_onActionTriggered;

        if ("gamepad".Equals(pc.Input.currentControlScheme, System.StringComparison.InvariantCultureIgnoreCase))
        {
            isMouseAndKeyboard = false;
        } else
        {
            isMouseAndKeyboard = true;
        }

        if (playerNum == 1)
        {
            gameObject.layer = LayerMask.NameToLayer("Player1");
            gameObject.tag = "Player1";
            mainBox.GetComponent<SpriteRenderer>().color = new Color(0.246084f, 0.3541024f, 0.6603774f, 1);
        } else
        {
            gameObject.layer = LayerMask.NameToLayer("Player2");
            gameObject.tag = "Player2";
            mainBox.GetComponent<SpriteRenderer>().color = new Color(0.6588235f, 0.2470588f, 0.3321685f, 1);
        }
        this.playerNum = playerNum;
    }

    private void Input_onActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name == controls.PlayerActions.Movement.name)
            OnMovement(obj);
        else if (obj.action.name == controls.PlayerActions.Jump.name)
            OnJump(obj);
        else if (obj.action.name == controls.PlayerActions.MouseAim.name)
            OnMouseMove(obj);
        else if (obj.action.name == controls.PlayerActions.ControllerAim.name)
            OnControllerMove(obj);
        else if (obj.action.name == controls.PlayerActions.Shoot.name)
            OnShoot(obj);
        else if (obj.action.name == controls.PlayerActions.Dash.name)
            OnDash(obj);

    }


    // Update is called once per frame
    void Update()
    {
        if (!isMouseAndKeyboard)

        IsGrounded();
        ApplyShoot();
        if (isMouseAndKeyboard)
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
            ApplyMovement();
            ApplyJump();
            ApplyDash();
        }
        else
            Debug.LogWarning("Rigidbody not attatched to player " + gameObject.name);
        

    }

    //used to update the moveInput float value with which direction the input wants the character to move
    public void OnMovement(InputAction.CallbackContext context)
    {
        directionInput = context.ReadValue<Vector2>();

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

    }


    //updates controller position
    public void OnControllerMove(InputAction.CallbackContext context)
    {
        controllerPosition = context.ReadValue<Vector2>();
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

    public void OnDash(InputAction.CallbackContext context)
    {
        dashing = context.action.triggered;
        dashTime = startDashTime;
    }


    //uses the move set through the PlayerControls to set the velocity of the player
    void ApplyMovement()
    {
        moveInput = directionInput.x;
        //checks if moving left then if moving right
        if (moveInput < 0)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (moveInput > 0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        } else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
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
        if (bullets.Count < 4)
        {
            if (timeBetweenShots <= 0)
            {
                if (shooting)
                {
                    Instantiate(projectile, shotPoint.position, gun.transform.rotation);
                    bullets.Add(1);
                    timeBetweenShots = startTimeBetweenShots;
                }
            }
            else
            {
                timeBetweenShots -= Time.deltaTime;
            }
        } else
        {
            if (timeBetweenReload <= 0)
            {
                bullets.Clear();
                timeBetweenReload = startReloadCooldown;
            }
            else
            {
                timeBetweenReload -= Time.deltaTime;
            }
        }
    }

    private void ApplyDash()
    {
        if (dashing)
        {
            //if the dash has fully finished, ends the dash
            if (dashTime <= 0)
            {
                dashing = false;
                gameObject.tag = "Player" + playerNum;
            } else
            {
                gameObject.tag = "Invulnerable";
                dashTime -= Time.deltaTime;
                if (moveInput < 0)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                } else if (moveInput > 0)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                } else
                {
                    dashing = false;
                }
            }

        }

    } 


    public void TakeDamage(int damage)
    {
        health -= damage;
    }


}
