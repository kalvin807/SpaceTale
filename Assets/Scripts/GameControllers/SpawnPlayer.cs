using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Player1 player1;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, false, 60);
        Instantiate(player1, new Vector3(-34f, -30f, 0f), Quaternion.Euler(270f, 0f, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
