using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
    public bool ended = false;

	public Animator animator;

	private Queue<string> sentences;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
        if (SceneManager.GetActiveScene().name == "Dialogue4")
        {
            GameObject.Find("realMainChar").GetComponent<RawImage>().color = new Color(0.15f, 0.15f, 0.15f);
        }

    }

	public void StartDialogue (ref Dialogue dialogue)
	{
        animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
        if (sentences.Count == 0)
		{
			EndDialogue();
            ended = true;
			return;
		}

        

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
        AudioSource audioData1 = GameObject.Find("Click").GetComponent<AudioSource>();
        audioData1.Play(0);

        if (SceneManager.GetActiveScene().name == "Dialogue4")
        {
            if (sentence[0] == '*')
            {
                nameText.text = "";
                GameObject.Find("mainChar").GetComponent<RawImage>().color = new Color(0.15f, 0.15f, 0.15f);
                GameObject.Find("realMainChar").GetComponent<RawImage>().color = new Color(0.15f, 0.15f, 0.15f);
            }
            else if (sentence[0] == ';')
            {
                nameText.text = "Boss";
                GameObject.Find("mainChar").GetComponent<RawImage>().color = Color.white;
                GameObject.Find("realMainChar").GetComponent<RawImage>().color = new Color(0.15f, 0.15f, 0.15f);
            }
            else
            {
                nameText.text = "Ray";
                GameObject.Find("mainChar").GetComponent<RawImage>().color = new Color(0.15f, 0.15f, 0.15f);
                GameObject.Find("realMainChar").GetComponent<RawImage>().color = Color.white;
            }
        }
        else
        {
            if (sentence[0] == '*')
            {
                nameText.text = "";
                GameObject.Find("mainChar").GetComponent<RawImage>().color = new Color(0.15f, 0.15f, 0.15f);
            }
            else if (sentence[0] == ';')
            {
                nameText.text = "Boss";
                GameObject.Find("mainChar").GetComponent<RawImage>().color = Color.white;
            }
            else
            {
                nameText.text = "Ray";
                GameObject.Find("mainChar").GetComponent<RawImage>().color = Color.white;
            }
        }

        dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
            if (letter != ';')
            {
                dialogueText.text += letter;
            }
			yield return null;
		}
	}

    void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
	}

}
