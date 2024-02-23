using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    private Hive SpawnHive;


    public void Initialize(Hive hive)
    {
        SpawnHive = hive;

    }

    //description
    [ContextMenu("Check Flower")]
    private void CheckAnyFlower() 
    {
        //search for flowers in level
        //find a random flower to target (so not all bees go to the same flower)
        Flower[] flowers = FindObjectsByType<Flower>(FindObjectsSortMode.None);
        Flower target = flowers[Random.Range(0, flowers.Length)];

        //flies to targeted flower
        transform.DOMove(target.transform.position, 1f).OnComplete(() =>
        {
            //Take nectar from flower
            if (target.GetNectar()) 
            {
                //If flower has nectar then go back to the hive and give hive nectar
                transform.DOMove(SpawnHive.transform.position, 1f);
                SpawnHive.GetNectar();
            }
            //If flower did not return nectar then go check another flower

        }).SetEase(Ease.Linear);
    }

}
