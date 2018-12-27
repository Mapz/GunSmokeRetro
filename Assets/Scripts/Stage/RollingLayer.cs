using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RollingLayer : MonoBehaviour, PauseAble {
    public Vector3 moveSpeed;

    private float lastY;

    private bool _pause = false;

    public void Pause (bool isPause) {
        _pause = isPause;
    }

    private void Awake () {
        Application.targetFrameRate = 100;
    }
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (!_pause)
            transform.position -= moveSpeed * Time.deltaTime;
    }
}