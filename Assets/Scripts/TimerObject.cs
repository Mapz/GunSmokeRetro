using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnumTimer {
    public Action action;
    public float timeToWait;

    public EnumTimer (Action _action, float _timeToWait) {
        action = _action;
        timeToWait = _timeToWait;
    }

    public void StartTimeout (MonoBehaviour mb) {
        mb.StartCoroutine (TimtToDo ());
    }

    IEnumerator TimtToDo () {
        yield return new WaitForSecondsRealtime (timeToWait);
        action ();
    }
}