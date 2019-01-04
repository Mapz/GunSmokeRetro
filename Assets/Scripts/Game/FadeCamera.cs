using DG.Tweening;
using UnityEngine;
public class FadeCamera : MonoBehaviour {

    private Texture2D _texture;
    private bool _done;
    private float _duration;
    private Color _color;
    private float _start;
    private float _end;

    public float duration { get { return _duration; } set { _duration = value; } }
    public float start { get { return _start; } set { _start = value; } }
    public float end { get { return _end; } set { _end = value; } }

    public TweenCallback _callBack;

    public TweenCallback callBack { get { return _callBack; } set { _callBack = value; } }

    public int loop = 1;

    public void DoFade () {
        _color = new Color (0, 0, 0, _start);
        DOTween.ToAlpha (() => _color, (x) => _color = x, _end, _duration).OnComplete (() => {
            _done = true;
            if (null != _callBack) _callBack ();
        }).SetLoops (loop, LoopType.Yoyo);
    }

    public void OnGUI () {
        if (_done) return;
        if (_texture == null) _texture = new Texture2D (1, 1);
        _texture.SetPixel (0, 0, _color);
        _texture.Apply ();
        GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), _texture);
    }

}