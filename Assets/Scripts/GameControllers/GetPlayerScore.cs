using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerScore : MonoBehaviour
{
    public Text scoreText;
    private Player1 player1;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if ((Player1)FindObjectOfType(typeof(Player1)) != null)
        {
            player1 = (Player1)FindObjectOfType(typeof(Player1));
        }
        scoreText.text = "Score: " + player1.Score;
    }
}
