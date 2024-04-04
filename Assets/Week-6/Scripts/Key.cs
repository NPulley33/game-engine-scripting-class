using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    private Vector3 origin;

    private void Awake()
    {
        origin = transform.position;
        GameManager.GetResetEvent().AddListener(OnReset);
    }

    private void OnReset()
    { 
        
    }

}
