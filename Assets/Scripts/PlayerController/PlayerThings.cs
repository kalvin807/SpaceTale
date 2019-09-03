using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThings : MonoBehaviour
{
    public static PlayerThings Instance;
    public bool shootContinue;
    public float speed;
    public float lspeed;
    public float lastShootTime;
    public float ultTime;
    public float ultCD;
    public float shootCD;
    public float live;
    public int health;
    public int bulletPower;
    public int score;
    public float hmove;
    public float vmove;
    public bool isSlowMove;
    public bool isShoot;
    public bool isUlt;
    public int ultLeft;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
