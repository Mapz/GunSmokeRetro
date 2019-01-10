using System.Collections;
using UnityEngine;

public class ScreenFlickerEffect : PostEffectBase {

    public Shader shader;
    private Material mat;

    public Material material {
        get {
            mat = CheckShaderAndCreateMaterial (shader, mat);
            return mat;
        }
    }

    [Range (1.0f, 30f)]
    public float speed = 20f;

    [Range (0f, 1f)]
    public float transParent = 1;

    public void setTranparent (bool isTransparent) {
        transParent = isTransparent ? 0 : 1;
    }

    void OnRenderImage (RenderTexture src, RenderTexture dest) {
        if (material != null) {
            material.SetFloat ("_Speed", speed);
            material.SetFloat ("_Transparent", transParent);

            //若材质可用，将参数传递给材质，在调用Graphics.Blit进行处理
            Graphics.Blit (src, dest, material);
        } else {
            //否则将图像直接输出到屏幕，不做任何处理
            Graphics.Blit (src, dest);
        }
    }
}