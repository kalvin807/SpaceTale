using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Player1 : MonoBehaviour
{
    public GameObject bullet;
    private Rigidbody rb;
    public UltEnlarge ult;
    private Vector3 v;
    private bool shootContinue;
    public float speed;
    public float lspeed;
    private Vector3 shootPlace1;
    private Vector3 shootPlace2;
    private float lastShootTime;
    private int ultLeft;
    private float ultTime;
    private float ultCD;
    private float shootCD;
    public ProgressBar pb;
    private ProgressBar pb2;
    private float live;
    private int health;
    private int bulletPower;
    private int score;
    private float hmove;
    private float vmove;
    private bool isSlowMove;
    private bool isShoot;
    private bool isUlt;
    public AudioClip hit;
    private AudioSource source;

    public int UltCD
    {
        get
        {
            if (Time.time - ultTime >= ultCD)
                return 0;
            else
                return ((int)ultCD - (int)(Time.time - ultTime));
        }
    }

    public int Score
    {
        get { return score; }

        set
        {
            score = value;
        }
    }

    public int BulletPower
    {
        get { return bulletPower; }

        set
        {
            bulletPower = value;
        }
    }


    public float Live
    {
        get { return live; }
    }

    public int UltLeft
    {
        get { return ultLeft; }

        set
        {
            ultLeft = value;
        }
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

        if (SceneManager.GetActiveScene().name == "Main1")
        {
            shootContinue = false;
            speed = 40f;
            lspeed = 25f;
            lastShootTime = 0f;
            ultTime = 0f;
            ultCD = 2f;
            shootCD = 0.09f;
            health = 100;
            bulletPower = 1;
            score = 0;
            hmove = 0f;
            vmove = 0f;
            isSlowMove = false;
            isShoot = false;
            isUlt = false;
            live = 1;
            ultLeft = 3;
        }
        else
        {
            loadPlayer();
        }

        source = Camera.main.transform.Find("SE").GetComponent<AudioSource>();
        Vector3 temp = new Vector3(transform.position.x / 0.16f + 640f, transform.position.y / 0.16f + 210f, transform.position.z);
        ultTime = Time.time - ultCD;
        lastShootTime = Time.time - shootCD;
    }

    // Update is called once per frame
    void Update()
    {


        shootPlace1 = transform.position;
        shootPlace1.x -= 2f;
        shootPlace1.z = 0;
        shootPlace2 = transform.position;
        shootPlace2.z = 0;


        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0, 2f / 3f);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void FixedUpdate()
    {
        hmove = Input.GetAxisRaw("Horizontal");
        vmove = Input.GetAxisRaw("Vertical");
        isSlowMove = Input.GetKey(KeyCode.LeftShift);
        isShoot = Input.GetKeyDown("space");
        isUlt = Input.GetKeyDown(KeyCode.F);

        rb = GetComponent<Rigidbody>();
        v = rb.velocity;
        float movespeed;
        movespeed = (isSlowMove) ? lspeed : speed;
        v.x = hmove * movespeed;
        v.y = vmove * movespeed;
        rb.velocity = v;
        if (isShoot)
        {
            shootContinue = shootContinue ? false : true;
        }
        if (shootContinue)
        {
            if (Time.time - lastShootTime >= shootCD)
            {
                Instantiate(bullet, shootPlace1, Quaternion.identity);
                Instantiate(bullet, shootPlace2, Quaternion.identity);
                lastShootTime = Time.time;
            }
        }
        if (isUlt && Time.time - ultTime >= ultCD && ultLeft > 0)
        {
            Instantiate(ult, transform.position, Quaternion.Euler(90f, 90f, 0));
            ultTime = Time.time;
            ultLeft--;
        }
    }
    void OnTriggerEnter(Collider obj)
    {
        //string name = obj.gameObject.name;

        if (obj.gameObject.tag == "EnemyBullets")
        {
            Destroy(obj.gameObject);
            live--;
            source.PlayOneShot(hit);
            GameObject[] gameObjectsEnemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullets");

            for (var i = 0; i < gameObjectsEnemyBullets.Length; i++)
                Destroy(gameObjectsEnemyBullets[i]);

            SpawnEnemy enemySpawner = (SpawnEnemy)FindObjectOfType(typeof(SpawnEnemy));
            Destroy(enemySpawner);

            GameObject[] gameObjectsEnemies = GameObject.FindGameObjectsWithTag("Enemies");

            for (var i = 0; i < gameObjectsEnemies.Length; i++)
                Destroy(gameObjectsEnemies[i]);

            shootContinue = false;
            speed = 0;
            lspeed = 0;
            lastShootTime = Time.time + 999;
            ultTime = Time.time + 999;
            savePlayer();
            SceneManager.LoadScene("GameOver");
        }

        if (obj.gameObject.tag == "PowerUp")
        {
            score += (int)Random.value * 20 + 5;
            int temp = (int)(Random.value * 9);
            if (temp % 3 == 0)
            {
                if (bulletPower < 3)
                {
                    bulletPower++;
                }
                else if (ultLeft < 5)
                {
                    ultLeft++;
                }
            }
            else if (ultLeft < 5)
            {
                ultLeft++;
            }
            Destroy(obj.gameObject);
        }
    }

    public void savePlayer()
    {
        PlayerThings.Instance.shootContinue = shootContinue;
        PlayerThings.Instance.speed = speed;
        PlayerThings.Instance.lspeed = lspeed;
        PlayerThings.Instance.lastShootTime = lastShootTime;
        PlayerThings.Instance.ultTime = ultTime;
        PlayerThings.Instance.ultCD = ultCD;
        PlayerThings.Instance.shootCD = shootCD;
        PlayerThings.Instance.live = live;
        PlayerThings.Instance.health = health;
        PlayerThings.Instance.bulletPower = bulletPower;
        PlayerThings.Instance.score = score;
        PlayerThings.Instance.hmove = hmove;
        PlayerThings.Instance.vmove = vmove;
        PlayerThings.Instance.isSlowMove = isSlowMove;
        PlayerThings.Instance.isShoot = isShoot;
        PlayerThings.Instance.isUlt = isUlt;
        PlayerThings.Instance.ultLeft = ultLeft;
    }

    public void loadPlayer()
    {
        shootContinue = PlayerThings.Instance.shootContinue;
        speed = PlayerThings.Instance.speed;
        lspeed = PlayerThings.Instance.lspeed;
        lastShootTime = PlayerThings.Instance.lastShootTime;
        ultTime = PlayerThings.Instance.ultTime;
        ultCD = PlayerThings.Instance.ultCD;
        shootCD = PlayerThings.Instance.shootCD;
        live = PlayerThings.Instance.live;
        health = PlayerThings.Instance.health;
        bulletPower = PlayerThings.Instance.bulletPower;
        score = PlayerThings.Instance.score;
        hmove = PlayerThings.Instance.hmove;
        vmove = PlayerThings.Instance.vmove;
        isSlowMove = PlayerThings.Instance.isSlowMove;
        isShoot = PlayerThings.Instance.isShoot;
        isUlt = PlayerThings.Instance.isUlt;
        ultLeft = PlayerThings.Instance.ultLeft;
    }
}
