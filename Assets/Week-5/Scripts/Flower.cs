using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private float NectarProductionRate = 5;
    private float Timer;
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
        Timer = NectarProductionRate;
    }

    // Update is called once per frame
    void Update()
    {
        //once the timer reaches the time it takes to make nectar:
        //the timer stops & is reset
        //the color of the flower is changed
        if (Timer <= 0)
        {
            CancelInvoke("Counter");
            HasNectar = true;
            Timer = NectarProductionRate;
            ChangeColor(colorHasNectar);
        }
    }

    //Called from Bee when a bee takes the nectar from a flower
    [ContextMenu("GetNectar")]
    public bool GetNectar() 
    {
        if (HasNectar)
        {
            StartMakingNectar();
            return true;
        } 
        else return false;
    }

    private void StartMakingNectar() {
        HasNectar = false;
        ChangeColor(colorNoNectar);

        //start timer here- could be better here performance wise bc it isn't called as often?
        Timer = NectarProductionRate;
        InvokeRepeating("Counter", 1f, 1f);
    }

    private void Counter() =>  Timer--; 

    //changes the color of the sprite to indicate nectar level
    private void ChangeColor(Color color) { 
        spriteRender.color = color; 
    }
}
