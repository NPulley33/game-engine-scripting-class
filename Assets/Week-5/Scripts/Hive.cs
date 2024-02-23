using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    //float production rate
    //int starting bees
    //gameobject bee prefab
    //int nectar amount
    //int honey amount
    //float counter/timer
    //bool counting down
    [SerializeField] private float HoneyProductionRate = 8;
    private float Timer;
    private bool isCountingDown;
    private int StartingNumOfBees= 3;
    [SerializeField] private int AmtNectar;
    [SerializeField] private int AmtHoney;
    [SerializeField] private GameObject BeePrefab;

    private void Awake()
    {
        for (int i = 1; i <= StartingNumOfBees; i++) 
        {
            //creates a new instance of a bee object at the hive's location
            GameObject newBee = Instantiate(BeePrefab, transform.position, Quaternion.identity); //look into rotation later
            //initilizes the Bee script
            newBee.GetComponent<Bee>().Initialize(this);
        }

        Timer = HoneyProductionRate;
    }

    //function make honey
    //if has nectar count down & make honey
    //public funct get nectar for bees
    //nectar count ++
    //if not counting down start timer

    // Update is called once per frame
    void Update()
    {
        //checking if we have nectar to make honey and that we aren't already making honey
        if (isCountingDown == false && AmtNectar > 0) //ensures that this does keep firing all the time
        {
            InvokeRepeating("Counter", 1f, 1f);
            isCountingDown = true;
        }
    }

    //called from Bee- increases the amount of nectar the hive has
    [ContextMenu("Get Nectar")]
    public void GetNectar() => AmtNectar++;

    private void MakeHoney()
    {
        //stop the timer
        CancelInvoke("Counter");

        //update attributes
        AmtNectar--;
        AmtHoney++;
        Debug.Log("Made Honey");

        //reset timer 
        Timer = HoneyProductionRate;
        isCountingDown = false;
    }

    private void Counter() 
    {
        Timer--;
        //when timer is up/reaches 0 make honey
        if (Timer <= 0) MakeHoney();
    }
}
