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

    private bool playerIsInTrigger;
    private bool playerPressedOpen;
    public bool isOpening;
    private float alpha;

    [SerializeField] GameObject DoorInstructions;
    private bool firstOpen;

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
        //time to open door
        alpha += isOpening ? Time.deltaTime * openSpeedModifier : -Time.deltaTime * openSpeedModifier;
        alpha = Mathf.Clamp01(alpha); //if value is < 0 or > 1 changes to max or min

        door.transform.position = Vector2.Lerp(origin, target, alpha);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //tells the player how to open a door the first time they encounter one until the first time they open one
        if (firstOpen) DoorInstructions.SetActive(true);
        playerIsInTrigger = true;
        PlayerController2D player = collision.GetComponent<PlayerController2D>();

        //checks if the collider is a player object and if the player has keys to be able to open the door
        if (player.tag == "Player" && player.GetKeys() > 0 && playerPressedOpen)
        {
            isOpening = true;
            Debug.Log(playerPressedOpen);

            //get rid of player key
            player.UseKey();
            player.UpdateKeyText();

            firstOpen = false; //never updates again, player knows how to open doors
        }
        //else Debug.Log("Player does not have a key");
    }

    //could get rid of to make doors stay open once unlocked
    //would then have to rotate the doors so that they stay out of the way of other paths
    private void OnTriggerExit2D(Collider2D collision)
    {
        isOpening = false;
        DoorInstructions.SetActive(false);
        playerPressedOpen = false;
        playerIsInTrigger = false;
    }

    private void Open(InputAction.CallbackContext context)
    {
        Debug.Log("Opened");
        if (playerIsInTrigger) playerPressedOpen = true;
    }

}
