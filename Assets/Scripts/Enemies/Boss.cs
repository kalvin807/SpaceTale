using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public EnemyBullet enemyBulletPrefab1, enemyBulletPrefab2, enemyBulletPrefab3, enemyBulletPrefab4, stone;
    public EnemyBuddy1 enemyBuddy;
    public TracingBullet tracingBulletPrefab;
    public PowerUpPlayer powerUp;
    private Player1 player1;
    private float sectorDelay = 1.2f;
    private float simpleDelay = 1f;
    private float circle1Delay = 2.4f;
    private float circle2Delay = 2.1f;
    private float realDelay = 1f;
    private float hardDelay = 0.3f;
    private float normalDelay = 2f;
    private float traceDelay = 1.5f;

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
    private float speed = 45f;
    private int moveSteps = 4 * 2;
    private bool startMoved = false;
    private float startMovedTime = 0;
    private float randomFloat;
    private float health = 100f;
    private float live;
    private float maxLive;
    private int liveScale = 800;
    private int worth;
    private int worthScale = 5000;
    public ProgressBar pb;
    private SpawnEnemy enemySpawner;
    private bool dead = false;


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
        dead = false;
        maxLive = liveScale;
        worth = worthScale;
        live = maxLive;


        startMoved = false;
        randomFloat = Random.value * moveDistance;

        enemySpawner = (SpawnEnemy)FindObjectOfType(typeof(SpawnEnemy));

        Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 340f, transform.position.z / 0.15f);


        ProgressBar obj = Instantiate(pb, temp, Quaternion.identity);
        obj.MaxLive = maxLive;
        obj.Live = live;
        obj.BarValue = live / maxLive * health;
        obj.transform.SetParent(gameObject.GetComponentInChildren<Canvas>().gameObject.transform, gameObject);


        sectorTime = Time.time - sectorDelay;
        shootTime = Time.time - simpleDelay;
        circleTime = Time.time - circle1Delay;
        circleTime2 = Time.time - circle2Delay;
        realRainTime = Time.time - realDelay;
        rainTime = Time.time - hardDelay;
        rainNormalTime = Time.time - normalDelay;
        shootTraceTime = Time.time - traceDelay;

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

            Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 340f, transform.position.z / 0.15f);

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
        else if (live / maxLive * health >= 75)
        {

            horizontalMove();
            realRain();

        }
        else if (live / maxLive * health >= 50)
        {
            horizontalMove();
            sectorShoot();
        }
        else if (live / maxLive * health >= 25)
        {
            horizontalMove();
            crazyCircle();
            spawnBuddy(false);
        }
        else
        {
            if (Time.time - simpleMoveTime >= 5f)
            {
                simpleMove();
            }
            rainNormal();
            spawnBuddy(true);
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
                for (int i = 0; i < 2; i++)
                {
                    Instantiate(powerUp, new Vector3(transform.position.x - i * 5 * (Random.value + 1), transform.position.y - i * 5 * (Random.value + 1)), Quaternion.identity);
                }
                dead = true;
                player1.Score += worth;
                Destroy(gameObject);
                enemySpawner.BossDead = true;

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

    public void spawnBuddy(bool hard)
    {
        if (((Time.time) - spawnTime >= 1f) && (((Mathf.Floor((Time.time))) % 4 == 1) || (hard && ((Mathf.Floor((Time.time))) % 3 == 1))))
        {
            for (float i = 0 + buddyAngle; i < 360 + buddyAngle; i += 75)
            {
                enemyBuddy.angle = i + 180;
                Instantiate(enemyBuddy, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.identity);
            }
            spawnTime = (Time.time);
            buddyAngle = (buddyAngle + 36) % 360;
        }
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

    public void sectorShoot()
    {
        if ((Time.time) - sectorTime >= sectorDelay)
        {
            for (int i = 0 - 60; i <= 60; i += 15)
            {
                enemyBulletPrefab2.angle = i;
                enemyBulletPrefab2.speed = 30f;
                enemyBulletPrefab2.delaySpeed = 50f;
                enemyBulletPrefab2.delayTime = sectorDelay + Time.time - 0.8f;
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
            enemyBulletPrefab3.speed = 25f;
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
                enemyBulletPrefab3.speed = 10f;
                enemyBulletPrefab3.delaySpeed = 30f;
                enemyBulletPrefab3.delayTime = circle1Delay + Time.time;
                Instantiate(enemyBulletPrefab3, new Vector3(transform.position.x, transform.position.y - 10), Quaternion.Euler(0, 0, i - 90));
            }
            circleTime = (Time.time);
        }

        if ((Time.time) - circleTime2 >= circle2Delay)
        {
            for (float i = 0 - circleAngle; i < 360 - circleAngle; i += 45)
            {
                enemyBulletPrefab1.angle = i;
                enemyBulletPrefab1.speed = 40f;
                enemyBulletPrefab1.delaySpeed = 20f;
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
            for (float i = -100f; i <= 34f; i += 20 + Random.value * 10)
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
            enemyBulletPrefab4.speed = 25f;
            enemyBulletPrefab4.delaySpeed = 50f;
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
