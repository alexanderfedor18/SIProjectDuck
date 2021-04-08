using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private PlayerInput parentInput;
    private Vector2 mousePosition;
    private Vector2 controllerPosition;
    private bool isMouse;


    // Start is called before the first frame update
    void Start()
    {
        parentTransform = GetComponent<Transform>();
        parentInput = GetComponent<PlayerInput>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isMouse)
        {
            //if input is coming from a mouse, changes the mouse position to a point relative to player and rotates the gun
            Vector3 relativePosition = Camera.main.ScreenToWorldPoint(mousePosition) - parentTransform.position;
            rotateGun(new Vector2(relativePosition.x, relativePosition.y));
        } else
        {
            rotateGun(controllerPosition);
        }

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
    
    private void rotateGun(Vector2 position)
    {
        float rotation = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        Quaternion temp = Quaternion.Euler(0f, 0f, rotation);
        transform.rotation = temp;
        
    }


}
