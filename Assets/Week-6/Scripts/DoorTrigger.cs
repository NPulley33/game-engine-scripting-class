using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] float openSpeedModifier;

    Vector2 origin; //door's position when open
    Vector2 target; //door's position when closed

    //booleans to determan logic for opening a door with a key press
    private bool playerIsInTrigger;
    private bool playerPressedOpen;

    public bool isOpening;
    private float alpha;

    //used to tell a player how to open a door for the first time
    [SerializeField] GameObject DoorInstructions;
    private bool firstOpen;

    //gets input for a player to open the door
    public PlayerInputActions PlayerControls;
    private InputAction open;

    private void Awake()
    {
        PlayerControls = new PlayerInputActions();
        origin = door.transform.position;
        target = origin + (Vector2.up * 2);
        firstOpen = true;
        playerIsInTrigger = false;
        playerPressedOpen = false;
    }
    //enabaling & disabling player input for door open only
    private void OnEnable()
    {
        open = PlayerControls.Player.Open;
        open.Enable();
        open.performed += Open;
    }
    private void OnDisable()
    {
        open.Disable();
    }

    private void Update()
    {
        //alpha is time to open door
        //handles how to open the door if the door is able to be opened
        alpha += isOpening ? Time.deltaTime * openSpeedModifier : -Time.deltaTime * openSpeedModifier;
        alpha = Mathf.Clamp01(alpha); //if value is < 0 or > 1 changes to max or min

        door.transform.position = Vector2.Lerp(origin, target, alpha);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //tells the player how to open a door the first time they encounter one until the first time they open one
        if (firstOpen) DoorInstructions.SetActive(true);

        playerIsInTrigger = true; //used for Open player input
        PlayerController2D player = collision.GetComponent<PlayerController2D>();

        //checks if the collider is a player object and if the player has keys to be able to open the door
        //and if the player pressed the button to open the door
        if (player.tag == "Player" && player.GetKeys() > 0 && playerPressedOpen)
        {
            isOpening = true;

            //get rid of player key
            player.UseKey();
            player.UpdateKeyText();

            firstOpen = false; //never updates again, player knows how to open doors
        }
    }

    //could get rid of to make doors stay open once unlocked
    //would then have to rotate the doors so that they stay out of the way of other paths
    private void OnTriggerExit2D(Collider2D collision)
    {
        //resetting all bools used to determine if the door can open
        isOpening = false;
        DoorInstructions.SetActive(false);
        playerPressedOpen = false;
        playerIsInTrigger = false;
    }

    //takes the player input for opening the door and updates the bool 
    private void Open(InputAction.CallbackContext context)
    {
        if (playerIsInTrigger) playerPressedOpen = true;
    }

}
