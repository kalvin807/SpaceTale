﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public PowerUpPlayer powerUp;
    private SpawnEnemy enemySpawner;
    public EnemyBullet enemyBulletPrefab1, enemySphereBulletPrefab1;
    private Player1 player1;
    private bool back;
    private bool targetSet = false;
    private Vector3 location, target;
    private int moveDistance = 20;
    private int steps = 100;
    private float simpleMoveTime = 0f;
    private float shootTime = 0f;
    private float sphereTime = 0f;
    private int sphereShooted = 0;
    private float speed = 20f;
    private int moveSteps = 4 * 2;
    private int shootAngle = 0;
    private int reverse = 1;
    private bool startMoved = false;
    private bool shooted = false;
    private bool finSimple = false;
    private float startMovedTime = 0;
    private float health = 100f;
    private int k = 0;
    private int worth;
    private int worthScale = 140;
    private float live;
    private float maxLive;
    private int liveScale = 80;
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

        Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 270f, transform.position.z / 0.15f);

        ProgressBar obj = Instantiate(pb, temp, Quaternion.identity);
        obj.BarValue = live / maxLive * health;
        obj.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().gameObject.transform, gameObject);

        shootTime = Time.time - 0.5f;
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

            Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 270f, transform.position.z / 0.15f);

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
            if (!finSimple)
            {
                simpleShoot();
            }
            else
            {
                sphereShoot();
            }
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
                if (temp % (4 * player1.BulletPower) == 0)
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

    public void horizontalMove()
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
                target = new Vector3(transform.position.x + randomMove * ((int)Mathf.Pow(-1, goX)), transform.position.y + randomMove * ((int)Mathf.Pow(-1, goY)));
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

    public void simpleShoot()
    {
        if (Time.time - shootTime >= 0.5)
        {
            for (int i = 0 + shootAngle + k * reverse; i < 360 + shootAngle + k * reverse; i += 40)
            {
                enemyBulletPrefab1.angle = i;
                enemyBulletPrefab1.speed = 30f;
                enemyBulletPrefab1.delaySpeed = 15f;
                enemyBulletPrefab1.delayTime = Time.time + 1.5f - 0.3f * (k / 10f);
                Instantiate(enemyBulletPrefab1, new Vector3(transform.position.x, transform.position.y - 5), Quaternion.Euler(0, 0, i - 90));

            }
            shootTime = Time.time;
            k += 13;
            if (k == 39)
            {
                k = 0;
                shootTime = Time.time + 1.5f;
                shootAngle = (shootAngle + 23) % 360;
                finSimple = true;
                reverse *= -1;
            }
        }
    }

    public void sphereShoot()
    {
        if (Time.time - sphereTime >= 0.3)
        {
            sphereShooted++;
            for (int i = 0 + shootAngle + 20; i < 360 + shootAngle + 20; i += 45)
            {
                enemySphereBulletPrefab1.angle = i;
                enemySphereBulletPrefab1.speed = 50f;
                enemySphereBulletPrefab1.delaySpeed = 20f;
                enemySphereBulletPrefab1.delayTime = Time.time + 1f - 0.2f * sphereShooted;
                Instantiate(enemySphereBulletPrefab1, new Vector3(transform.position.x, transform.position.y - 5), Quaternion.identity);
            }
            sphereTime = Time.time;
            if (sphereShooted >= 3)
            {
                sphereShooted = 0;
                finSimple = false;
            }
        }
    }
}
