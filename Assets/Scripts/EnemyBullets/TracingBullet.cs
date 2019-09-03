using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingBullet : MonoBehaviour
{
    private float incX;
    private float incY;
    public float angle;
    private Player1 player1;
    private float steps;
    private float deadTime;

    // Start is called before the first frame update
    public void Start()
    {
        deadTime = Time.time + 2;
        player1 = (Player1)FindObjectOfType(typeof(Player1));
    }

    // Update is called once per frame
    void Update()
    {
        angle = findAngle(player1.transform.position, transform.position);
        transform.localRotation = Quaternion.Euler(0, 0, angle - 90);
        steps = Mathf.Sqrt(Mathf.Pow(player1.transform.position.x - transform.position.x, 2) + Mathf.Pow(player1.transform.position.y - transform.position.y, 2)) + 15;
        this.incX = player1.transform.position.x - transform.position.x;
        this.incY = player1.transform.position.y - transform.position.y;
        transform.position = new Vector3(transform.position.x + incX / steps, transform.position.y + incY / steps, 0);

        if (deadTime <= Time.time)
        {
            Destroy(gameObject);
        }
    }


    void OnBecameInvisible()
    {
        Destroy(gameObject, 3);
    }

    float findAngle(Vector3 playPos, Vector3 shooterPos)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(playPos.x - shooterPos.x, 2) + Mathf.Pow(playPos.y - shooterPos.y, 2));
        float dx = playPos.x - shooterPos.x;
        float theta;
        if (playPos.y <= shooterPos.y)
        {
            theta = Mathf.Asin(dx / distance);
            theta = Mathf.Rad2Deg * theta;
        }
        else
        {
            theta = Mathf.Acos(dx / distance);
            theta = Mathf.Rad2Deg * theta + 90;
        }
        return theta;
    }

}
