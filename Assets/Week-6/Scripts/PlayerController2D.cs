using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerInputActions PlayerControls;
    private InputAction move; 
    private InputAction fire;

    Vector2 moveDirection = Vector2.zero;
    [SerializeField] float moveSpeed = 5f;

    //UI elements/Text
    [SerializeField] GameObject winText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI keysText;
    [SerializeField] TextMeshProUGUI gemsText;

    //player health and pickups
    private int Health;
    private int Keys;
    private int Gems;


    //initialize/assign variables
    private void Awake()
    {
        PlayerControls = new PlayerInputActions();
        Health = 10;
        Keys = 0;
        keysText.text = "Keys: 0";
        gemsText.text = "Gems: 0";
        healthText.text = $"Health: {Health * 10}%";
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

    private void Fire(InputAction.CallbackContext context) {
        Debug.Log("Fire");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //determines what the player is intereacting with and determines what it should do accordingly
        switch (collision.tag) {
            case "Finish":
                winText.SetActive(true);
                break;
            case "Key":
                Keys++;
                Destroy(collision.gameObject);
                UpdateKeyText();
                break;
            case "Trap":
                Damage();
                break;
            case "Gem":
                Gems++;
                Destroy(collision.gameObject);
                gemsText.text = $"Gems: {Gems}";
                break;
        }
    }

    //update UI text
    public void UpdateKeyText() { keysText.text = $"Keys: {Keys}"; }
    public void Damage() 
    {
        Health--;
        healthText.text = $"Health: {Health * 10}%";
        if (Health <= 0) {
            Restart();
        }
    }
    //If player dies/loses all health, the scene resets
    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public int GetKeys() => Keys; //returns the number of keys a player has, used to open doors
    public void UseKey() => Keys--; //uses a key when a player goes through a door

}
