using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltEnlarge : MonoBehaviour
{
    private float bigTime;
    private int scale;
    private bool shrink;
    // Start is called before the first frame update
    void Start()
    {
        bigTime = Time.time;
        scale = 10;
        shrink = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - bigTime >= 0.01)
        {
            transform.localScale = new Vector3(0, scale, scale);
            if (scale < 200 && shrink == false)
            {
                transform.Rotate(new Vector3(9, 0, 0));
                scale += 5;
            }
            else
            {
                transform.Rotate(new Vector3(-18, 0, 0));
                shrink = true;
                scale -= 10;
                if (scale <= 0)
                {
                    Destroy(gameObject);
                }
            }
            bigTime = Time.time;
        }
    }

    void OnTriggerEnter(Collider obj)
    {

        if (obj.gameObject.tag == "EnemyBullets")
        {
            Destroy(obj.gameObject);
        }
    }
}
