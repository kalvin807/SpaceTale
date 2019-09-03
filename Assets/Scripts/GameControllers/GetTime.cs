using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTime : MonoBehaviour
{
    public Text timeText;
    private string hh, mm, ss;
    private int h, m, s;
    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "Time: 00:00";
    }

    // Update is called once per frame
    void Update()
    {
        s = (int)Mathf.Floor(Time.time) % 60;
        m = (int)(Mathf.Floor(Time.time) / 60) % 60;
        h = (int)(Mathf.Floor(Time.time) / 3600) % 60;

        ss = s + "";
        mm = m + "";
        hh = h + "";

        if (s < 10)
        {
            ss = "0" + s;
        }
        if (m < 10)
        {
            mm = "0" + m;
        }
        if (h < 10)
        {
            hh = "0" + h;
        }
        if (h <= 0)
        {
            timeText.text = "Time: " + mm + ":" + ss;
        }
        else
        {
            timeText.text = "Time: " + hh + ":" + mm + ":" + ss;
        }
    }
}
