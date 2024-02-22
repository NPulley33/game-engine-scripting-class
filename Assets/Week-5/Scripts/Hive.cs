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
            newBee.GetComponent<Bee>().Initialize(this);
        }
    }

    //function make honey
    //if has nectar count down & make honey
    //public funct get nectar for bees
    //nectar count ++
    //if not counting down start timer

    // Start is called before the first frame update
    void Start()
    {
        //instantiate # of bees in starting bees int
        //initilize funct for bees (pass self as ref)
    }

    // Update is called once per frame
    void Update()
    {
        //if has nectar count down to create honey
            //remove 1 nectar & add 1 honey
            //check for more nectar- if yes restart timer
    }
}
