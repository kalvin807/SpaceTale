using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerPower : MonoBehaviour
{
    public Text powerText;
    private Player1 player1;

    // Start is called before the first frame update
    void Start()
    {
        powerText.text = "Power: 0/3";
    }

    // Update is called once per frame
    void Update()
    {
        if ((Player1)FindObjectOfType(typeof(Player1)) != null)
        {
            player1 = (Player1)FindObjectOfType(typeof(Player1));
        }
        powerText.text = "Power: " + player1.BulletPower + "/3";
    }
}
