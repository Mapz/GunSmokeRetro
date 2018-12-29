using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnumTimer {
    public Action action;
    public float timeToWait;
    public int m_loop;

    public EnumTimer (Action _action, float _timeToWait, int _loop = 1) {
        action = _action;
        timeToWait = _timeToWait;
        m_loop = _loop;
    }

    public void StartTimeout (MonoBehaviour mb) {
        mb.StartCoroutine (TimtToDo ());
    }

    IEnumerator TimtToDo () {
        for (int i = 0; i < m_loop; i++) {
            yield return new WaitForSecondsRealtime (timeToWait);
            action ();
        }
    }
}