using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] float openSpeedModifier;

    Vector2 origin; //door's position when open
    Vector2 target; //door's position when closed

    public bool isOpening;
    private float alpha;

    private void Awake()
    {
        origin = door.transform.position;
        target = origin + (Vector2.up * 2);
    }
    private void Update()
    {
        //time to open door
        alpha += isOpening ? Time.deltaTime * openSpeedModifier : -Time.deltaTime * openSpeedModifier;
        alpha = Mathf.Clamp01(alpha); //if value is < 0 or > 1 changes to max or min

        door.transform.position = Vector2.Lerp(origin, target, alpha);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if the collider is a player object and if the player has keys to be able to open the door
        if (collision.tag == "Player" && collision.GetComponent<PlayerController2D>().GetKeys() > 0)
        {
            isOpening = true;
            Debug.Log("Player has a key");
        }
        else Debug.Log("Player does not have a key");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isOpening = false;
    }

}
