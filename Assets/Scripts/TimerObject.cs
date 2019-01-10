using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumTimer {
    public Action action;
    public float timeToWait;
    public int m_loop;
    public Func<bool> m_pauseWhile;

    private Coroutine m_coroutine;

    public EnumTimer (Action _action, float _timeToWait, int _loop = 1, Func<bool> pauseCondition = null) {
        action = _action;
        timeToWait = _timeToWait;
        m_loop = _loop;
        m_pauseWhile = pauseCondition;
    }

    public void StartTimeout (MonoBehaviour mb) {
        m_coroutine = mb.StartCoroutine (TimtToDo ());
    }

    IEnumerator TimtToDo () {
        for (int i = 0; i < m_loop; i++) {
            if (null != m_pauseWhile) {
                yield return new WaitWhile (m_pauseWhile);
            }
            yield return new WaitForSecondsRealtime (timeToWait);
            action ();
        }
    }
}