using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUltCD : MonoBehaviour
{
    public Text ultText;
    private Player1 player1;

    // Start is called before the first frame update
    void Start()
    {
        ultText.text = "Ult CD: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if ((Player1)FindObjectOfType(typeof(Player1)) != null)
        {
            player1 = (Player1)FindObjectOfType(typeof(Player1));
        }
        ultText.text = "Ult CD: " + player1.UltCD;
    }
}
