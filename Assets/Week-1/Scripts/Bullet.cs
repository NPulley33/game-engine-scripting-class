using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;

    private void Awake()
    {
        //using this vs gameObject only destroys the script component
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    //works both ways (when you enter trigger or when smth enters your trigger)
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (other.transform.name == "Enemy") 
        {
            other.GetComponent<EnemyCollisionDemo>().Damage();    
        }
    }
}
