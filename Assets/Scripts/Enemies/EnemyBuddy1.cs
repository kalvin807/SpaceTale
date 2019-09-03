using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuddy1 : MonoBehaviour
{
    public EnemyBullet enemyBulletPrefab1, enemyBulletPrefab2;
    public float speed = 20f;
    public float angle;
    private float incX;
    private float incY;
    private float stopTime;

    // Start is called before the first frame update
    public void Start()
    {
        this.incX = Mathf.Cos(Mathf.Deg2Rad * (angle - 90));
        this.incY = Mathf.Sin(Mathf.Deg2Rad * (angle - 90));
        stopTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if (stopTime + 1 > Time.time)
        {
            transform.position = new Vector3(transform.position.x + dt * speed * incX, transform.position.y + dt * speed * incY, 0);
        }
        else
        {
            crazyCircle();
            Destroy(gameObject);
        }
    }


    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void crazyCircle()
    {
        for (float i = 0; i < 360; i += 72)
        {
            enemyBulletPrefab1.angle = i;
            enemyBulletPrefab1.speed = 20f;
            enemyBulletPrefab1.delayTime = 0f;
            Instantiate(enemyBulletPrefab1, new Vector3(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, i - 90));
        }
        
        for (float i = 0 + 23; i < 360 + 23; i += 60)
        {
            enemyBulletPrefab2.angle = i;
            enemyBulletPrefab2.speed = 35f;
            enemyBulletPrefab2.delayTime = 0f;
            Instantiate(enemyBulletPrefab2, new Vector3(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, i - 90));
        }
    }
}
