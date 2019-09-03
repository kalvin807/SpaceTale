using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour
{
    public Animator animator;

    private int scene;

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy enemySpawner = (SpawnEnemy)FindObjectOfType(typeof(SpawnEnemy));
        if (enemySpawner != null && enemySpawner.canSwitchScene)
        {        
            GameObject[] objArr;
            objArr = GameObject.FindGameObjectsWithTag("EnemyBullets");
            foreach ( GameObject obj in objArr){
                Destroy(obj);
            }
            fadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void fadeToScene(int sceneIndex)
    {
        scene = sceneIndex;
        animator.SetTrigger("fout");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(scene);
    }
}
