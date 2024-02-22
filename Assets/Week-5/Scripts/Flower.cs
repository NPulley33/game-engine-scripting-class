using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private float NectarProductionRate = 5;
    private float Timer = 0;
    private bool HasNectar;
    //changing the color of the flower based on the state of HasNectar
    private SpriteRenderer spriteRender;
    private Color colorHasNectar = Color.white;
    private Color colorNoNectar = Color.gray;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        HasNectar = true; //put in start?
        spriteRender.color = colorHasNectar;    
    }

    // Update is called once per frame
    void Update()
    {
        //if the flower doesn't have nectar, starts a counter to produce nectar
        if (!HasNectar)
        {
            InvokeRepeating("Counter", 1f, 1f);
        }
        //once the timer reaches the time it takes to make nectar:
        //the timer stops & is reset
        //the color of the flower is changed
        if (Timer >= NectarProductionRate)
        {
            CancelInvoke("Counter");
            HasNectar = true;
            Timer = 0;
            ChangeColor(colorHasNectar);
        }
    }

    public bool FlowerHasNectar() 
    { 
        return HasNectar;
    }
    public bool TakeNectar() 
    {
        if (HasNectar)
        {
            HasNectar = false;
            ChangeColor(colorNoNectar);
            //could start timer here
            //Timer = 0; InvokeRepeating(); etc. 
            //could be better here performance wise bc it isn't called as often?
            return true;
        } 
        else return false;
    }

    private void Counter() {
        Timer++;
    }

    private void ChangeColor(Color color) { 
        spriteRender.color = color; 
    }
}
