using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject bossPrefab, finalBossPrefab, enemy1Prefab, enemy2Prefab, enemy3Prefab, enemy4Prefab, enemy5Prefab, bossHelperPrefab, tracingEmiterPrefab;
    private GameObject[] enemies;


    private int spawnBossHelper = 0;
    private int spawnedEnemies = 0;
    private int levels;
    private int[] levelSpawn;
    private float[] levelStartTime;
    private float levelMinTime;
    private int currentLevel;
    private int spawnLeft = 0;
    private int spawnRight = 0;

    private bool canSwitch = false;
    private bool bossDead = false;


    public bool canSwitchScene
    {
        get { return canSwitch; }

        set
        {
            canSwitchScene = value;
        }
    }

    public bool BossDead
    {
        get { return bossDead; }

        set
        {
            bossDead = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main4")
        {

            bossDead = false;
            spawnedEnemies = 0;
            enemies = new GameObject[] { enemy1Prefab, enemy2Prefab, enemy3Prefab, enemy4Prefab, enemy5Prefab };
            Instantiate(finalBossPrefab, transform.position, Quaternion.Euler(-90, 0, 0));

        }
        else if (SceneManager.GetActiveScene().name == "Main2")
        {
            bossDead = false;

            Instantiate(bossPrefab, transform.position, Quaternion.Euler(90f, 0, 180f));

            if (spawnBossHelper == 0)
            {
                Instantiate(bossHelperPrefab, new Vector3(-98, -40, 0), Quaternion.identity);
                spawnBossHelper++;
            }

            if (spawnBossHelper == 1)
            {
                Instantiate(bossHelperPrefab, new Vector3(30, -40, 0), Quaternion.identity);
                spawnBossHelper++;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Main1")
        {
            levels = 2;
            levelMinTime = 40f;
            enemies = new GameObject[] { enemy1Prefab, enemy2Prefab, enemy3Prefab, enemy4Prefab, enemy5Prefab };
            spawnedEnemies = 0;
            levelSpawn = new int[levels];
            levelStartTime = new float[levels];
            for (int i = 0; i < levels; i++)
            {
                levelSpawn[i] = (int)(Mathf.Ceil(Random.value * (i + 1) / 3f) + (i));
                levelStartTime[i] = i * levelMinTime + i + Time.time;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Main3")
        {
            levels = 3;
            levelMinTime = 50f;
            enemies = new GameObject[] { enemy1Prefab, enemy2Prefab, enemy3Prefab, enemy4Prefab, enemy5Prefab };
            spawnedEnemies = 0;
            levelSpawn = new int[levels];
            levelStartTime = new float[levels];
            for (int i = 0; i < levels; i++)
            {
                levelSpawn[i] = (int)(Mathf.Ceil(Random.value * (i + 1) / 3f) + (i));
                levelStartTime[i] = i * levelMinTime + i + Time.time;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {


        if (SceneManager.GetActiveScene().name == "Main2" || SceneManager.GetActiveScene().name == "Main4")
        {
            if (bossDead)
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EnemyBullets");

                for (var i = 0; i < gameObjects.Length; i++)
                    Destroy(gameObjects[i]);

                Player1 player1 = (Player1)FindObjectOfType(typeof(Player1));
                player1.savePlayer();
                canSwitch = true;

            }
            FinalBoss finalBoss = (FinalBoss)FindObjectOfType(typeof(FinalBoss));
            if (SceneManager.GetActiveScene().name == "Main4" && spawnedEnemies == 0 && finalBoss.Live > finalBoss.MaxLive / 2)
            {
                float spawnPlace; // = ((int)(Random.value * 10) % 2) == 0 ? -103 : 35;
                if (((int)(Random.value * 10) % 2) == 0)
                {
                    spawnPlace = -103;
                    if (spawnLeft > spawnRight + 1)
                    {
                        spawnPlace = 35;
                        spawnRight++;
                    }
                    else
                    {
                        spawnLeft++;
                    }
                }
                else
                {
                    spawnPlace = 35;
                    if (spawnRight > spawnLeft + 1)
                    {
                        spawnPlace = -103;
                        spawnLeft++;
                    }
                    else
                    {
                        spawnRight++;
                    }
                }

                Instantiate(enemies[(int)Mathf.Floor(Random.value * enemies.Length)], new Vector3(spawnPlace, transform.position.y - Random.value * 10 * Mathf.Pow(-1, (int)Random.value)), Quaternion.Euler(0, 0, -90));
                spawnedEnemies++;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Main1" || SceneManager.GetActiveScene().name == "Main3")
        {
            for (int i = levels - 1; i >= 0; i--)
            {
                if (Time.time > levelStartTime[i] && spawnedEnemies <= 0)
                {
                    currentLevel = i;
                    break;
                }
            }
            if (spawnedEnemies < levelSpawn[currentLevel] && levelStartTime[levels - 1] + levelMinTime > Time.time)
            {
                // maximum each side spawn 2 enemies consecutively
                float spawnPlace; // = ((int)(Random.value * 10) % 2) == 0 ? -103 : 35;
                if (((int)(Random.value * 10) % 2) == 0)
                {
                    spawnPlace = -103;
                    if (spawnLeft > spawnRight + 1)
                    {
                        spawnPlace = 35;
                        spawnRight++;
                    }
                    else
                    {
                        spawnLeft++;
                    }
                }
                else
                {
                    spawnPlace = 35;
                    if (spawnRight > spawnLeft + 1)
                    {
                        spawnPlace = -103;
                        spawnLeft++;
                    }
                    else
                    {
                        spawnRight++;
                    }
                }

                Instantiate(enemies[(int)Mathf.Floor(Random.value * enemies.Length)], new Vector3(spawnPlace, transform.position.y - Random.value * 10 * Mathf.Pow(-1, (int)Random.value)), Quaternion.Euler(0, 0, -90));
                spawnedEnemies++;
            }
            else if (Time.time > levelStartTime[currentLevel] && Time.time > levelStartTime[Mathf.Clamp(currentLevel + 1, 0, levels - 1)] && spawnedEnemies > 0)
            {
                levelSpawn[currentLevel] = 0;
                levelStartTime[Mathf.Clamp(currentLevel + 1, 0, levels - 1)] = Time.time + 1;
                for (int i = currentLevel + 2; i < levels; i++)
                {
                    levelStartTime[i] = levelStartTime[i - 1] + levelMinTime;
                }
            }
            else if (spawnedEnemies == 0 && levelSpawn[levels - 1] == 0)
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EnemyBullets");

                for (var i = 0; i < gameObjects.Length; i++)
                    Destroy(gameObjects[i]);

                Player1 player1 = (Player1)FindObjectOfType(typeof(Player1));
                player1.savePlayer();
                canSwitch = true; //switch scene
            }
        }

    }


    public void enemyDead()
    {
        spawnedEnemies--;
    }


}
