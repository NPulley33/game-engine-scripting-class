using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController3D : MonoBehaviour
{
    public Rigidbody rb;
    public PlayerInputActions PlayerControls;
    private InputAction move;
    private InputAction fire;

    [SerializeField] float moveSpeed = 5.5f;

    //assigns variables
    private void Awake()
    {
        PlayerControls = new PlayerInputActions();
    }
    //enables input action sub actions
    private void OnEnable()
    {
        move = PlayerControls.Player.Move;
        move.Enable();

        fire = PlayerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }
    //disable sub actions
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }
    

    void Update()
    {
        //handles movement
        Vector2 input = move.ReadValue<Vector2>();
        input *= moveSpeed;
        rb.velocity = new Vector3(input.x, rb.velocity.y, input.y);
        
        //find a way to handle rotation via mouse movement? 
    }

    //called on fire action- used for combat
    private void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire");
    }

}
