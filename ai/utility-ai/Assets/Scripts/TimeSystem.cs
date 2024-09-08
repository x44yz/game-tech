using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AI.Utility;

public class TimeSystem : MonoBehaviour
{
    public const float ONEDAY_MINUTES = 24 * 60f;
    public static TimeSystem Inst = null;

    public float timeSpd;
    public TMP_Text txtTime;
    [Range(0.1f, 10f)]
    public float timeScale;
    
    [Header("RUNTIME")]
    public float dayMins;
    public float deltaMins;
    public bool paused;
    public float totalMins;

    private void Awake()
    {
        Inst = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            paused = !paused;

        if (paused)
            return;

        float dt = Time.deltaTime;
        deltaMins = timeSpd * timeScale * dt;

        dayMins += deltaMins;
        totalMins += deltaMins;

        if (dayMins > ONEDAY_MINUTES)
            dayMins -= ONEDAY_MINUTES;

        int hour = (int)(dayMins / 60f);
        int min = (int)(dayMins - hour * 60);
        txtTime.text = $"{hour}:{min}";
    }
}
