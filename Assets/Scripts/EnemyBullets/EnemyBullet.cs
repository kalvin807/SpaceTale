using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 40f;
    public float angle;
    public float delayTime;
    public float delaySpeed;
    public float rotateX;
    public float rotateY;
    public float rotateZ;
    private float incX;
    private float incY;
    private float dt;

    // Start is called before the first frame update
    public void Start()
    {
        this.incX = Mathf.Cos(Mathf.Deg2Rad * (angle - 90));
        this.incY = Mathf.Sin(Mathf.Deg2Rad * (angle - 90));
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        if (delayTime >= (Time.time))
        {
            transform.position = new Vector3(transform.position.x + dt * delaySpeed * incX, transform.position.y + dt * delaySpeed * incY, 0);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + dt * speed * incX, transform.position.y + dt * speed * incY, 0);
        }
        if ( (transform.position.y < -37.6) || (transform.position.y > 77.6) || (transform.position.x < -102.3) || (transform.position.x > 34.1))
        {
            Destroy(gameObject, 1);
        }
        transform.Rotate(new Vector3(rotateX, rotateY, rotateZ));
    }


    void OnBecameInvisible()
    {
        Destroy(gameObject, 1);
    }
    
}
