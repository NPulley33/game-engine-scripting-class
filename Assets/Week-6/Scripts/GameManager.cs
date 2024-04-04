using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [SerializeField] UnityEvent ResetEvent;
    private static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static UnityEvent GetResetEvent()
    { 
        return instance.ResetEvent;
    }

    [ContextMenu("Invoke Reset Event")]
    public void InvokeResetEvent()
    { 
        ResetEvent.Invoke();
    }

}
