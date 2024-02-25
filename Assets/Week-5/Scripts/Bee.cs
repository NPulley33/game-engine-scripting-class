using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bee : MonoBehaviour
{
    private Hive SpawnHive;


    public void Initialize(Hive hive)
    {
        SpawnHive = hive;
        CheckAnyFlower();
    }

    [ContextMenu("Check Flower")]
    private void CheckAnyFlower() 
    {
        //search for flowers in level
        //find a random flower to target (so not all bees go to the same flower)
        Flower[] flowers = FindObjectsByType<Flower>(FindObjectsSortMode.None);
        Flower target = flowers[Random.Range(0, flowers.Length)];
        //go to & check flower for nectar
        FlyToFlower(target);
    }

    private void FlyToFlower(Flower target) {

        //flies to targeted flower
        transform.DOMove(target.transform.position, 1.5f).OnComplete(() =>
        {
            //Take nectar from flower
            //If flower has nectar then go back to the hive and give hive nectar
            if (target.GetNectar()) FlyToHive();
            //If flower did not return nectar then go check another flower
            else CheckAnyFlower();

        }).SetEase(Ease.Linear);
    }
    private void FlyToHive() {

        //flies to hive
        transform.DOMove(SpawnHive.transform.position, 1.5f).OnComplete(() =>
        {
            //deposits honey & continues searching flowers
            SpawnHive.GetNectar();
            CheckAnyFlower();

        }).SetEase(Ease.Linear);
    }

}
