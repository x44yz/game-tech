using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AI.Utility;

public class TimeSystem : MonoBehaviour
{
    public const float ONEDAY_SECONDS = 24 * 3600f;
    public static TimeSystem Inst = null;

    public float timeSpd;
    public TMP_Text txtTime;
    
    [Header("RUNTIME")]
    public float daySecs;

    private void Awake()
    {
        Inst = this;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        daySecs += timeSpd * dt;
        if (daySecs > ONEDAY_SECONDS)
            daySecs -= ONEDAY_SECONDS;

        int hour = (int)(daySecs / 3600f);
        int min = (int)((daySecs - hour * 3600) / 60f);
        txtTime.text = $"{hour}:{min}";
    }
}
