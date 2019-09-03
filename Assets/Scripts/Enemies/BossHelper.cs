using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelper : MonoBehaviour
{
    public EnemyBullet enemyBulletPrefab1;
    public TracingBullet tracingBulletPrefab;
    private Player1 player1;
    private float rainNormalTime = 0;
    private float speed = 30f;
    private int moveSteps = 4 * 2;
    private bool startMoved = false;
    private float startMovedTime = 0;
    private float randomFloat;
    private float health = 100f;
    private float rainDelay;
    private float live;
    const float maxLive = 100f;
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
        dead = false;
        live = maxLive;
        player1 = (Player1)FindObjectOfType(typeof(Player1));
        startMoved = false;
        randomFloat = Random.value * 10;
        rainDelay = 3f;
        rainNormalTime = Time.time - rainDelay;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, findAngle(transform.position, player1.transform.position) + 90);

        Boss boss = (Boss)FindObjectOfType(typeof(Boss));
        if (boss.Live >= boss.MaxLive * 3.0 / 4.0)
        {
            rainDelay = 1.5f;
        }
        else
        {
            rainDelay = 3f;
        }

        if (boss.Live < boss.MaxLive / 2)
        {
            Destroy(gameObject);
        }

        if (!startMoved)
        {
            startMove();
        }
        else
        {
            rainNormal();
        }
    }

    public void startMove()
    {
        if (transform.position.y < -30)
        {
            float dt = Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y + dt * speed, 0);
        }
        else
        {
            startMoved = true;
            startMovedTime = Time.time;
        }
    }

    public void shootTrace()
    {

        Vector3 temp = transform.position;
        Vector3 playerPos = player1.transform.position;
        float angle = findAngle(playerPos, temp);
        if ((Time.time) - rainNormalTime >= 1)
        {

            Instantiate(tracingBulletPrefab, new Vector3(transform.position.x + 5 * Mathf.Cos(Mathf.Deg2Rad * (angle - 90)), transform.position.y + 5 * Mathf.Sin(Mathf.Deg2Rad * (angle - 90))), Quaternion.identity);
            rainNormalTime = (Time.time);
        }

    }

    public void rainNormal()
    {
        Vector3 temp = transform.position;
        Vector3 playerPos = player1.transform.position;
        if ((Time.time) - rainNormalTime >= rainDelay)
        {
            enemyBulletPrefab1.angle = findAngle(playerPos, temp);
            enemyBulletPrefab1.speed = 50f;
            enemyBulletPrefab1.delayTime = 0;
            float angle = findAngle(playerPos, temp);
            Instantiate(enemyBulletPrefab1, new Vector3(transform.position.x + 5 * Mathf.Cos(Mathf.Deg2Rad * (angle - 90)), transform.position.y + 5 * Mathf.Sin(Mathf.Deg2Rad * (angle - 90))), Quaternion.Euler(0, 0, enemyBulletPrefab1.angle - 90));
            rainNormalTime = (Time.time);
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
