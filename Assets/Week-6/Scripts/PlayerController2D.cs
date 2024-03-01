using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerInputActions PlayerControls;
    private InputAction move; 
    private InputAction fire;

    Vector2 moveDirection = Vector2.zero;
    [SerializeField] float moveSpeed = 5f;

    [SerializeField] GameObject winText;

    //initialize/assign variables
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
    //disables sub actions
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
    }

    void Update()
    {
        //gets input direction
        moveDirection = move.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        //moves the player
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    //called on a fire action- used for combat?
    private void Fire(InputAction.CallbackContext context) {
        Debug.Log("Fire");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        winText.SetActive(true);
    }

}
