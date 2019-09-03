using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private float speed = 100f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + dt * speed, 0);

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject, 1);
    }
    
}
