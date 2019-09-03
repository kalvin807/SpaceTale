using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchToBossScene : MonoBehaviour
{
    public Animator animator;
    private DialogueManager dialogueManager;

    private int scene;

    // Update is called once per frame
    void Update()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager.ended)
        {
            fadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void fadeToScene(int sceneIndex)
    {
        scene = sceneIndex;
        animator.SetTrigger("goBoss");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(scene);
    }
}
