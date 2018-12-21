using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RollingLayer : MonoBehaviour {
    public Vector3 moveSpeed;

    private float lastY;

    private void Awake () {
        Application.targetFrameRate = 100;
    }
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        transform.position -= moveSpeed * Time.deltaTime;
    }
}