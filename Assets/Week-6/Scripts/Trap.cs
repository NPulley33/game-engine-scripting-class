using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Sprite trapPassive;
    public Sprite trapActivated;
    private SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            //semi-animated sprite to show a change in state/show that player will be damaged 
            //if they go over a trap (visual feedback other than an out of the line of sight value change)
            spr.sprite = trapActivated;

            DOVirtual.DelayedCall(0.5f, () => {
                //add what to do
                spr.sprite = trapPassive;
            });
        }
    }

}
