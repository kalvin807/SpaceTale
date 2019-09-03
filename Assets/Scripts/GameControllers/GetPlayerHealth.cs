using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerHealth : MonoBehaviour
{
    private Player1 player1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 == null)
        {
            player1 = (Player1)FindObjectOfType(typeof(Player1));
        }
        if (player1.Live <= 0)
        {
            Destroy(gameObject);
        }
    }
}
