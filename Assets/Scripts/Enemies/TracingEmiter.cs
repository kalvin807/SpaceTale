using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracingEmiter : MonoBehaviour
{
    public TracingBullet tracingBulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(tracingBulletPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
