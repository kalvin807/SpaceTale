﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public PowerUpPlayer powerUp;
    private SpawnEnemy enemySpawner;
    public EnemyBullet enemyBulletPrefab1;
    private Player1 player1;
    private Vector3 location, target;
    private bool targetSet = false;
    private bool back;
    private int steps = 100;
    private float simpleMoveTime = 0f;
    private float shootTime = 0f;
    private float speed;
    private int moveSteps = 4 * 2;
    private int moveDistance = 20;
    private int shootAngle = 0;
    private int bulletSpawned = 0;
    private float roundTime = 0f;
    private int reverse = 1;
    private bool startMoved = false;
    private float startMovedTime = 0;
    private float health = 100f;
    private int worth;
    private int worthScale = 50;
    private float live;
    private float maxLive;
    private int liveScale = 50;
    private bool dead = false;
    public ProgressBar pb;
    private float randomFloat;

    public float Live
    {
        get { return live; }
    }

    public float MaxLive
    {
        get { return maxLive; }
    }


    public static void SetParent(Transform child, Transform parent)
    {
        child.parent = parent.parent;
        child.localPosition = (parent.localPosition);
        child.localRotation = Quaternion.identity;
        child.localScale = Vector3.one;
    }

    // Start is called before the first frame update
    void Start()
    {
        player1 = (Player1)FindObjectOfType(typeof(Player1));
        maxLive = liveScale;
        live = maxLive;
        worth = worthScale;
        startMoved = false;
        randomFloat = Random.value * moveDistance;
        
        enemySpawner = (SpawnEnemy)FindObjectOfType(typeof(SpawnEnemy));

        Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 270f, transform.position.z / 0.16f);

        ProgressBar obj = Instantiate(pb, temp, Quaternion.identity);
        obj.BarValue = live / maxLive * health;
        obj.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().gameObject.transform, gameObject);

        shootTime = Time.time - 0.2f;
    }

    // Update is called once per frame
    void Update()
    {

        if (this.gameObject.transform.GetChild(0).transform.childCount > 0)
        {
            for (int i = 0; i < this.gameObject.transform.GetChild(0).transform.childCount; i++)
            {
                Destroy(this.gameObject.transform.GetChild(0).transform.GetChild(i).gameObject);
            }

            Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 270f, transform.position.z / 0.16f);

            ProgressBar obj = Instantiate(pb, temp, Quaternion.identity);
            obj.BarValue = live / maxLive * health;
            obj.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().gameObject.transform, gameObject);

        }
        
    }


    void FixedUpdate()
    {
        if (!startMoved)
        {
            speed = 40f;
            startMove();
        }
        else
        {
            speed = 10f;
            if (Time.time - simpleMoveTime >= 5f)
            {
                simpleMove();
            }
            simpleShoot();
        }

    }

    void OnTriggerEnter(Collider obj)
    {

        if (obj.gameObject.tag == "PlayerBullets")
        {
            if (live - player1.BulletPower > 0 && dead == false)
            {
                live -= player1.BulletPower;
            }
            else if (dead == false)
            {
                dead = true;
                player1.Score += worth;
                Destroy(gameObject);
                enemySpawner.enemyDead();
                int temp = (int)(Random.value * 100);
                if (temp % (6 * player1.BulletPower) == 0)
                {
                    Instantiate(powerUp, transform.position, Quaternion.identity);
                }
            }
            Destroy(obj.gameObject);
        }

    }

    void startMove()
    {
        if (transform.position.x < -56.8f - randomFloat)
        {
            float dt = Time.deltaTime;
            transform.position = new Vector3(transform.position.x + dt * speed, transform.position.y, 0);
        }
        else if (transform.position.x > -11.4f + randomFloat)
        {
            float dt = Time.deltaTime;
            transform.position = new Vector3(transform.position.x - dt * speed, transform.position.y, 0);
        }
        else
        {
            startMoved = true;
            back = true;
            targetSet = false;
            startMovedTime = Time.time;
            location = transform.position;
        }
    }

    void horizontalMove()
    {
        float dt = Time.deltaTime;
        if (((Time.time - startMovedTime) % moveSteps) < moveSteps / 4 || ((Time.time - startMovedTime) % moveSteps) >= moveSteps * 3 / 4)
            transform.position = new Vector3(transform.position.x + dt * speed, transform.position.y - dt * 2, 0);
        else
            transform.position = new Vector3(transform.position.x - dt * speed, transform.position.y - dt * 2, 0);
    }

    void simpleMove()
    {
        if (transform.position.x <= location.x - moveDistance || transform.position.x >= location.x + moveDistance || transform.position.y <= location.y - moveDistance || transform.position.y >= location.y + moveDistance)
        {
            back = false;
            steps = 100;
            target = location;
            if (Time.time - simpleMoveTime < 5f)
            {
                simpleMoveTime = Time.time;
            }
        }
        if (back)
        {
            if (!targetSet)
            {
                int goX = (int)(Random.value * moveDistance) % 2;
                int goY = (int)(Random.value * moveDistance) % 2;
                int randomMove = (int)(Random.value * moveDistance) + 1;
                target = new Vector3(transform.position.x + randomMove * ((int) Mathf.Pow(-1, goX)), transform.position.y + randomMove * ((int)Mathf.Pow(-1, goY)));
                targetSet = true;
                steps = 100;
            }
            if (steps > 0)
            {
                goPlace(transform.position, target, steps);
                steps--;
            }
            else
            {
                steps = 100;
                targetSet = false;
                simpleMoveTime = Time.time;
            }
        }
        else
        {
            if (steps > 0)
            {
                goPlace(transform.position, target, steps);
                steps--;
            }
            else
            {
                back = true;
                simpleMoveTime = Time.time;
            }
        }
    }

    void goPlace(Vector3 from, Vector3 to, int step)
    {
        transform.position = new Vector3(from.x + (to.x - from.x) / step, from.y + (to.y - from.y) / step, 0);
    }

    void simpleShoot()
    {
        if (Time.time - shootTime >= 0.2)
        {
            for (int i = 0 + shootAngle; i < 360 + shootAngle; i += 36)
            {
                enemyBulletPrefab1.angle = i * reverse;
                enemyBulletPrefab1.speed = 20f;
                enemyBulletPrefab1.delaySpeed = 40f;
                enemyBulletPrefab1.delayTime = 0.6f + Time.time;
                Instantiate(enemyBulletPrefab1, new Vector3(transform.position.x, transform.position.y - 5), Quaternion.Euler(0, 0, i * reverse - 90));
            }
            shootTime = Time.time;
            bulletSpawned++;
            if (bulletSpawned == 3)
            {
                shootTime = Time.time + 0.6f;
                bulletSpawned = 0;
                shootAngle += 18;
            }
        }
    }

}
