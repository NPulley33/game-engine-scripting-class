using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private GameObject thisItem;

    private void Awake()
    {
        thisItem = gameObject;
    }
    private void Start()
    {
        GameManager.GetResetEvent().AddListener(OnReset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") thisItem.SetActive(false);
    }

    private void OnReset()
    {
        thisItem.SetActive(true);
    }

}
