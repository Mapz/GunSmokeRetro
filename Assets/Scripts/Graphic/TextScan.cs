using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TextScan : MonoBehaviour {
    private Material _mat;

    void Start () {
        // 此处单独重新创建一个mat，避免材质的shader参数互通
        _mat = Instantiate (GetComponent<Text> ().material);
        GetComponent<Text> ().material = _mat;
        _mat.SetFloat ("_YPos", GetComponent<RectTransform> ().position.y);
        _mat.SetFloat ("_ScanHeight", GetComponent<RectTransform> ().rect.height + 3);
    }

}