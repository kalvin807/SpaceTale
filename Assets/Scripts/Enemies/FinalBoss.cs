using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public EnemyBullet enemyBulletPrefab1, enemyBulletPrefab2, enemyBulletPrefab3, enemyBulletPrefab4, stone, enemySphereBulletPrefab1;
    public EnemyBuddy1 enemyBuddy;
    public TracingBullet tracingBulletPrefab;
    private EnemyBullet[] realBullets;
    private Player1 player1;
    public PowerUpPlayer powerUp;
    private float sectorDelay = 0.9f;
    private float simpleDelay = 0.6f;
    private float circle1Delay = 1.4f;
    private float circle2Delay = 1.1f;
    private float realDelay = 1f;
    private float hardDelay = 0.3f;
    private float normalDelay = 0.3f;
    private float traceDelay = 0.5f;

    private float rainTime = 0;
    private float rainNormalTime = 0;
    private float circleTime = 0;
    private float circleTime2 = 0;
    private float spawnTime = 0;
    private float shootTime = 0;
    private float shootTraceTime = 0;
    private float realRainTime = 0;
    private float sectorTime = 0;

    private Vector3 location, target;
    private bool targetSet = false;
    private bool back;
    private int steps = 100;
    private float simpleMoveTime = 0f;
    private int moveDistance = 20;

    private float circleAngle = 180f;
    private float buddyAngle = 0f;
    private float speed = 40f;
    private int moveSteps = 4 * 2;
    private bool startMoved = false;
    private float startMovedTime = 0;
    private float randomFloat;
    private float health = 100f;
    private float live;
    private float maxLive;
    private int liveScale = 1600;
    private int worth;
    private int worthScale = 15000;
    public ProgressBar pb;
    private SpawnEnemy enemySpawner;
    private bool dead = false;
    private float moveDelay = 10f;
    private bool only = false;
    
    
    private float swirlShootTime = 0;
    private float swirlDelay = 0.2f;
    private int swirlBullet = 0;

    private float dDelay = 15f;
    private int iDelay = 0;
    private bool bulletSpawn = false;
    private float roundTime = 0f;
    private float delayTime = 0f;
    private float fasterTime = 0f;

    private bool finSimple = false;
    private int k = 0;
    private float sphereTime = 0f;
    private float threeTime = 0f;
    private int sphereShooted = 0;

    private float flowerTime = 0f;
    private bool shooted = false;
    private int flower = 2;

    private float simple2Time = 0f;
    private int bulletSpawned = 0;

    private int swirlShootAngle = 0;
    private int delayShootAngle = 0;
    private int fasterShootAngle = 0;
    private int threeSphereShootAngle = 0;
    private int flowerShootAngle = 0;
    private int simple2ShootAngle = 0;

    private int swirlReverse = 1;
    private int delayReverse = 1;
    private int fasterReverse = 1;
    private int threeSphereReverse = 1;
    private int simple2Reverse = 1;

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
        realBullets = new EnemyBullet[] { enemyBulletPrefab1, enemyBulletPrefab2, enemyBulletPrefab3, enemyBulletPrefab4 };
        dead = false;
        maxLive = liveScale;
        worth = worthScale;
        live = maxLive;

        only = false;
        startMoved = false;
        randomFloat = Random.value * moveDistance;

        enemySpawner = (SpawnEnemy)FindObjectOfType(typeof(SpawnEnemy));

        Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 280f, transform.position.z / 0.15f);

        ProgressBar obj = Instantiate(pb, temp, Quaternion.identity);
        obj.MaxLive = maxLive;
        obj.Live = live;
        obj.BarValue = (live / maxLive) * health;
        obj.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().gameObject.transform, gameObject);

        sectorTime = Time.time - sectorDelay;
        shootTime = Time.time - simpleDelay;
        circleTime = Time.time - circle1Delay;
        circleTime2 = Time.time - circle2Delay;
        realRainTime = Time.time - realDelay;
        rainTime = Time.time - hardDelay;
        rainNormalTime = Time.time - normalDelay;
        shootTraceTime = Time.time - traceDelay;

        swirlShootTime = Time.time - swirlDelay;

        simpleMoveTime = Time.time - 5f;
        roundTime = Time.time - 1.5f;
        delayTime = Time.time - 0.03f;

        sphereTime = Time.time - 0.3f;
        threeTime = Time.time - 0.5f;

        flowerTime = Time.time - 0.3f;

        simple2Time = Time.time - 0.2f;
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

            Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 280f, transform.position.z / 0.15f);

            ProgressBar obj = Instantiate(pb, temp, Quaternion.identity);
            obj.MaxLive = maxLive;
            obj.Live = live;
            obj.BarValue = live / maxLive * health;
            obj.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().gameObject.transform, gameObject);

        }
    }

    void FixedUpdate()
    {
        if (!startMoved)
        {
            startMove();
        }
        else
        {

            speed = 10f;
            if (Time.time - simpleMoveTime >= moveDelay)
            {
                simpleMove();
            }
            if (live / maxLive * health >= 80)
            {
                flowerShoot();
                simple2Shoot();
            }
            else if (live / maxLive * health >= 60)
            {
                if (!bulletSpawn)
                {
                    fasterShoot();
                }
                else if (Time.time - roundTime >= 1.5f)
                {
                    bulletSpawn = false;
                }
                crazyCircle();
            }
            else if (live / maxLive * health >= 40)
            {
                if (!only)
                    onlyMe();
                spawnBuddy(false);
                if (!bulletSpawn)
                {
                    //two choose 1
                    delayShoot();
                }
                else if (Time.time - roundTime >= 1.5f)
                {
                    bulletSpawn = false;
                }
            }
            else if (live / maxLive * health >= 20)
            {
                swirlShoot();
            }
            else
            {
                spawnBuddy(true);
                if (!finSimple)
                {
                    threeShoot();
                }
                else
                {
                    sphereShoot();
                }
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
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(powerUp, new Vector3(transform.position.x - i * 5 * (Random.value + 1), transform.position.y - i * 5 * (Random.value + 1)), Quaternion.identity);
                }
                dead = true;
                player1.Score += worth;
                Destroy(gameObject);
                enemySpawner.BossDead = true;
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EnemyBullets");

                for (var i = 0; i < gameObjects.Length; i++)
                    Destroy(gameObjects[i], 1);
            }
            Destroy(obj.gameObject);
        }

    }

    void simpleMove()
    {
        if (transform.position.x <= location.x - moveDistance || transform.position.x >= location.x + moveDistance || transform.position.y <= location.y - moveDistance / 5 || transform.position.y >= location.y + moveDistance / 5)
        {
            back = false;
            steps = 100;
            target = location;
            if (Time.time - simpleMoveTime < moveDelay)
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

    public void startMove()
    {
        float dt = Time.deltaTime;
        if (transform.position.x < -34f)
        {
            transform.position = new Vector3(transform.position.x + dt * speed, transform.position.y, 0);
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

    public void crossMove()
    {
        float dt = Time.deltaTime;
        if (((Time.time) % (moveSteps * 2)) < moveSteps)
        {
            speed = 10f;
            if (((Time.time) % moveSteps) < moveSteps / 4 || ((Time.time) % moveSteps) >= moveSteps * 3 / 4)
                transform.position = new Vector3(transform.position.x + dt * speed, transform.position.y, 0);
            else
                transform.position = new Vector3(transform.position.x - dt * speed, transform.position.y, 0);
        }
        else
        {
            speed = 5f;
            if (((Time.time) % moveSteps) < moveSteps / 4 || ((Time.time) % moveSteps) >= moveSteps * 3 / 4)
                transform.position = new Vector3(transform.position.x, transform.position.y + dt * speed, 0);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y - dt * speed, 0);
        }
    }

    public void horizontalMove()
    {
        float dt = Time.deltaTime;
        speed = 10f;
        if (((Time.time) % moveSteps) < moveSteps / 4 || ((Time.time) % moveSteps) >= moveSteps * 3 / 4)
            transform.position = new Vector3(transform.position.x + dt * speed, transform.position.y, 0);
        else
            transform.position = new Vector3(transform.position.x - dt * speed, transform.position.y, 0);
    }

    public void verticalMove()
    {
        float dt = Time.deltaTime;
        speed = 5f;
        if (((Time.time) % moveSteps) < moveSteps / 4 || ((Time.time) % moveSteps) >= moveSteps * 3 / 4)
            transform.position = new Vector3(transform.position.x, transform.position.y + dt * speed, 0);
        else
            transform.position = new Vector3(transform.position.x, transform.position.y - dt * speed, 0);
    }

    public void spawnBuddy(bool hard)
    {
        if (((Time.time) - spawnTime >= 1f) && (((Mathf.Floor((Time.time))) % 4 == 1) || (hard && ((Mathf.Floor((Time.time))) % 3 == 1))))
        {
            for (float i = 0 + buddyAngle; i < 360 + buddyAngle; i += 72)
            {
                enemyBuddy.angle = i + 180;
                Instantiate(enemyBuddy, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.identity);
            }
            spawnTime = (Time.time);
            buddyAngle = (buddyAngle + 43) % 360;
        }
    }

    public void sectorShoot()
    {
        if ((Time.time) - sectorTime >= sectorDelay)
        {
            for (int i = 0 - 60; i <= 60; i += 15)
            {
                enemyBulletPrefab2.angle = i;
                enemyBulletPrefab2.speed = 15f;
                enemyBulletPrefab2.delaySpeed = 50f;
                enemyBulletPrefab2.delayTime = sectorDelay + Time.time;
                Instantiate(enemyBulletPrefab2, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.Euler(0, 0, i - 90));
            }
            sectorTime = (Time.time);
        }
    }

    public void simpleShoot()
    {
        if ((Time.time) - shootTime >= simpleDelay)
        {
            enemyBulletPrefab3.angle = 0;
            enemyBulletPrefab3.speed = 20f;
            enemyBulletPrefab3.delaySpeed = 50f;
            enemyBulletPrefab3.delayTime = simpleDelay + Time.time;
            Instantiate(enemyBulletPrefab3, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.Euler(0, 0, -90));
            shootTime = (Time.time);
        }
    }

    public void crazyCircle()
    {
        if ((Time.time) - circleTime >= circle1Delay)
        {
            for (float i = circleAngle; i < 360 + circleAngle; i += 24)
            {
                enemyBulletPrefab3.angle = i;
                enemyBulletPrefab3.speed = 20f;
                enemyBulletPrefab3.delaySpeed = 50f;
                enemyBulletPrefab3.delayTime = circle1Delay + Time.time - 0.5f;
                Instantiate(enemyBulletPrefab3, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.Euler(0, 0, i - 90));
            }
            circleTime = (Time.time);
        }

        if ((Time.time) - circleTime2 >= circle2Delay)
        {
            for (float i = 0 - circleAngle; i < 360 - circleAngle; i += 15)
            {
                enemyBulletPrefab1.angle = i;
                enemyBulletPrefab1.speed = 30f;
                enemyBulletPrefab1.delaySpeed = 15f;
                enemyBulletPrefab1.delayTime = circle2Delay + Time.time;
                Instantiate(enemyBulletPrefab1, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.Euler(0, 0, i - 90));
            }
            circleTime2 = (Time.time);
        }

        circleAngle = (circleAngle + 5) % 360;
    }

    public void realRain()
    {
        if ((Time.time) - realRainTime >= realDelay)
        {
            for (float i = -100f; i <= 34f; i += 10 + Random.value * 10)
            {
                if (i >= transform.position.x + 10 || i <= transform.position.x - 10)
                {
                    stone.angle = 0;
                    stone.speed = 30f;
                    stone.delaySpeed = 45f;
                    stone.delayTime = realDelay + 0.5f + Time.time;
                    stone.rotateX = Random.value * 10;
                    stone.rotateY = Random.value * 10;
                    stone.rotateZ = Random.value * 10;
                    Instantiate(stone, new Vector3(i, 77, 0), Quaternion.Euler(Random.value * 360, Random.value * 360, Random.value * 360));
                }
            }
            realRainTime = (Time.time);
        }
    }

    public void rainHard()
    {
        Vector3 temp = transform.position;
        Vector3 playerPos = player1.transform.position;
        //Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.15f + 230f, transform.position.z / 0.15f);
        if ((Time.time) - rainTime >= hardDelay)
        {
            for (float i = transform.position.x - 20f; i <= transform.position.x + 20f; i += 10)
            {
                temp.x = i;
                temp.y = transform.position.y - 10;
                enemyBulletPrefab3.angle = findAngle(playerPos, temp);
                enemyBulletPrefab3.speed = 30f;
                enemyBulletPrefab3.delaySpeed = 0;
                enemyBulletPrefab3.delayTime = 0;
                Instantiate(enemyBulletPrefab3, temp, Quaternion.Euler(0, 0, enemyBulletPrefab3.angle - 90));
            }
            rainTime = (Time.time);
        }
    }

    public void rainNormal()
    {
        Vector3 temp = transform.position;
        Vector3 playerPos = player1.transform.position;
        if ((Time.time) - rainNormalTime >= normalDelay)
        {
            enemyBulletPrefab4.angle = findAngle(playerPos, temp);
            enemyBulletPrefab4.speed = 50f;
            enemyBulletPrefab4.delaySpeed = 20f;
            enemyBulletPrefab4.delayTime = normalDelay + Time.time;
            Instantiate(enemyBulletPrefab4, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.Euler(0, 0, enemyBulletPrefab4.angle - 90));
            rainNormalTime = (Time.time);
        }
    }

    public void shootTrace()
    {
        if ((Time.time) - shootTraceTime >= traceDelay)
        {
            Instantiate(tracingBulletPrefab, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.Euler(0, 0, -90));
            shootTraceTime = (Time.time);
        }
    }

    public void swirlShoot()
    {
        if (Time.time - swirlShootTime >= swirlDelay)
        {
            for (float i = 0 + swirlShootAngle; i < 360 + swirlShootAngle; i += 45f)
            {
                EnemyBullet goBullet = realBullets[swirlBullet];
                goBullet.angle = i * swirlReverse;
                goBullet.speed = Mathf.Clamp(15f * swirlBullet, 15, 50);
                goBullet.delaySpeed = Mathf.Clamp(30f * swirlBullet, 30, 100);
                goBullet.delayTime = Mathf.Clamp(0.3f * swirlBullet, 0.2f, 1f) + Time.time;
                Instantiate(goBullet, new Vector3(transform.position.x, transform.position.y - 15), Quaternion.Euler(0, 0, i * swirlReverse - 90));
                swirlBullet = (swirlBullet + 1) % realBullets.Length;
            }
            
            if (swirlShootAngle + 17 >= 720)
            {
                swirlReverse *= -1;
            }
            swirlShootAngle = (swirlShootAngle + 17) % 720;
            swirlShootTime = Time.time;
        }
    }

    public void delayShoot()
    {
        if (Time.time - delayTime >= 0.03 && !bulletSpawn)
        {
            int trueAngle = iDelay * delayReverse + delayShootAngle;
            enemyBulletPrefab4.angle = trueAngle;
            enemyBulletPrefab4.speed = 20f;
            enemyBulletPrefab4.delaySpeed = 0f;
            enemyBulletPrefab4.delayTime = 0.9f - 0.03f * (iDelay / 15f) + Time.time + 0.2f * ((360f - iDelay) / 360f);
            Instantiate(enemyBulletPrefab4, new Vector3(transform.position.x - 1f + dDelay * Mathf.Sin(Mathf.Deg2Rad * trueAngle), transform.position.y + 1f - dDelay * Mathf.Cos(Mathf.Deg2Rad * trueAngle)), Quaternion.Euler(0, 0, trueAngle - 90));
            dDelay += Mathf.Log10((360f - iDelay) * 10 / 360f);
            iDelay += 15;
            if (iDelay == 360)
            {
                iDelay = 0;
                dDelay = 15f;
                bulletSpawn = true;
                delayReverse *= -1;
                roundTime = Time.time;
                delayShootAngle = (delayShootAngle + 23) % 360;
            }
            delayTime = Time.time;
        }
    }

    public void fasterShoot()
    {
        if (Time.time - fasterTime >= 0.03 && !bulletSpawn)
        {
            int trueAngle = iDelay * fasterReverse + fasterShootAngle;
            enemyBulletPrefab4.angle = trueAngle;
            enemyBulletPrefab4.speed = 30f;
            enemyBulletPrefab4.delaySpeed = 5f;
            enemyBulletPrefab4.delayTime = 0.9f;
            Instantiate(enemyBulletPrefab4, new Vector3(transform.position.x, transform.position.y - 15), Quaternion.Euler(0, 0, trueAngle - 90));
            dDelay += Mathf.Log10((360f - iDelay) * 10 / 360f);
            iDelay += 15;
            if (iDelay == 360)
            {
                iDelay = 0;
                dDelay = 15f;
                bulletSpawn = true;
                fasterReverse *= -1;
                roundTime = Time.time;
                fasterShootAngle = (fasterShootAngle + 23) % 360;
            }
            fasterTime = Time.time;
        }
    }

    public void threeShoot()
    {
        if (Time.time - threeTime >= 0.5)
        {
            for (int i = 0 + threeSphereShootAngle + k * threeSphereReverse; i < 360 + threeSphereShootAngle + k * threeSphereReverse; i += 72)
            {
                enemyBulletPrefab4.angle = i;
                enemyBulletPrefab4.speed = 30f;
                enemyBulletPrefab4.delaySpeed = 15f;
                enemyBulletPrefab4.delayTime = Time.time + 1.5f - 0.3f * (k / 10f);
                Instantiate(enemyBulletPrefab4, new Vector3(transform.position.x, transform.position.y - 15), Quaternion.Euler(0, 0, i - 90));

            }
            threeTime = Time.time;
            k += 13;
            if (k == 39)
            {
                k = 0;
                shootTime = Time.time + 1.5f;
                threeSphereShootAngle = (threeSphereShootAngle + 67) % 360;
                finSimple = true;
                threeSphereReverse *= -1;
            }
        }
    }

    public void sphereShoot()
    {
        if (Time.time - sphereTime >= 0.3)
        {
            sphereShooted++;
            sphereTime = Time.time;
            if (sphereShooted >= 3)
            {
                sphereShooted = 0;
                finSimple = false;
            }
        }
    }

    public void flowerShoot()
    {
        if (Time.time - flowerTime >= 0.3)
        {
            if (!shooted)
            {
                shooted = true;
                for (int i = 0 + flowerShootAngle; i < 360 + flowerShootAngle; i += 60)
                {
                    enemyBulletPrefab2.angle = i;
                    enemyBulletPrefab2.speed = 20f;
                    //enemyBulletPrefab2.delayTime = 0;
                    enemyBulletPrefab2.delaySpeed = 40f;
                    enemyBulletPrefab2.delayTime = Time.time + 0.9f;
                    Instantiate(enemyBulletPrefab2, new Vector3(transform.position.x, transform.position.y - 15), Quaternion.Euler(0, 0, i - 90));
                }
                //shootTime = (Time.time - startMovedTime);
                flowerTime = Time.time;
            }

            if (flower <= 3)
            {
                float subAngle;
                subAngle = (flower == 2) ? 10f : 20f;
                for (int j = 0 + flowerShootAngle; j < 360 + flowerShootAngle; j += 60)
                {
                    enemyBulletPrefab2.angle = j + subAngle;
                    enemyBulletPrefab2.speed = 20f;
                    enemyBulletPrefab2.delayTime = Time.time + 0.9f - 0.3f * (flower - 1);
                    Instantiate(enemyBulletPrefab2, new Vector3(transform.position.x, transform.position.y - 15), Quaternion.Euler(0, 0, j + subAngle - 90));
                }
                for (int j = 0 + flowerShootAngle; j < 360 + flowerShootAngle; j += 60)
                {
                    enemyBulletPrefab2.angle = j - subAngle;
                    enemyBulletPrefab2.speed = 20f;
                    enemyBulletPrefab2.delayTime = Time.time + 0.9f - 0.3f * (flower - 1);
                    Instantiate(enemyBulletPrefab2, new Vector3(transform.position.x, transform.position.y - 15), Quaternion.Euler(0, 0, j - subAngle - 90));
                }

                flower++;
                flowerTime = Time.time;

                if (flower > 3)
                {
                    shooted = false;
                    flower = 2;
                    flowerShootAngle = (flowerShootAngle + 20) % 360;
                    flowerTime = Time.time + 1.2f;
                }

            }
        }
    }

    public void simple2Shoot()
    {
        if (Time.time - simple2Time >= 0.2)
        {
            for (int i = 0 + simple2ShootAngle; i < 360 + simple2ShootAngle; i += 30)
            {
                enemyBulletPrefab4.angle = i * simple2Reverse;
                enemyBulletPrefab4.speed = 50f;
                enemyBulletPrefab4.delaySpeed = 20f;
                enemyBulletPrefab4.delayTime = 1f + Time.time;
                Instantiate(enemyBulletPrefab4, new Vector3(transform.position.x, transform.position.y - 15), Quaternion.Euler(0, 0, i * simple2Reverse - 90));
            }
            simple2Time = Time.time;
            bulletSpawned++;
            if (bulletSpawned == 4)
            {
                simple2Time = Time.time + 0.6f;
                bulletSpawned = 0;
                simple2ShootAngle = (simple2ShootAngle + 15) % 360;
            }
        }
    }

    void onlyMe()
    {
        GameObject[] gameObjectsEnemies = GameObject.FindGameObjectsWithTag("Enemies");

        for (var i = 0; i < gameObjectsEnemies.Length; i++) {
            if (gameObjectsEnemies[i] != gameObject)
                Destroy(gameObjectsEnemies[i]);
        }
        only = true;
    }

    public float findAngle(Vector3 playPos, Vector3 shooterPos)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(playPos.x - shooterPos.x, 2) + Mathf.Pow(playPos.y - shooterPos.y, 2));
        float dx = playPos.x - shooterPos.x;
        float theta;
        if (playPos.y <= shooterPos.y)
        {
            theta = Mathf.Asin(dx / distance);
            theta = Mathf.Rad2Deg * theta;
        }
        else
        {
            theta = Mathf.Acos(dx / distance);
            theta = Mathf.Rad2Deg * theta + 90;
        }
        return theta;
    }
}
