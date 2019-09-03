using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y - dt * 10, 0);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
}
