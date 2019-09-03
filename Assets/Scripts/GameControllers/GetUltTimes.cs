using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUltTimes : MonoBehaviour
{
    public Text ultText;
    private Player1 player1;

    // Start is called before the first frame update
    void Start()
    {
        ultText.text = "Ult left: 0/5";
    }

    // Update is called once per frame
    void Update()
    {
        if ((Player1)FindObjectOfType(typeof(Player1)) != null)
        {
            player1 = (Player1)FindObjectOfType(typeof(Player1));
        }
        ultText.text = "Ult left: " + player1.UltLeft + "/5";
    }
}
