using UnityEngine;
public class BrightnessSaturationAndContrast : PostEffectBase {

    public Shader briSatConShader;
    private Material briSatConMaterial;

    public Material material {
        get {
            briSatConMaterial = CheckShaderAndCreateMaterial (briSatConShader, briSatConMaterial);
            return briSatConMaterial;
        }
    }

    [Range (0.0f, 3.0f)]
    public float brightness = 1.0f;
    [Range (0.0f, 3.0f)]
    public float saturation = 1.0f;
    [Range (0.0f, 3.0f)]
    public float contrast = 1.0f;

    void OnRenderImage (RenderTexture src, RenderTexture dest) {
        if (material != null) {
            material.SetFloat ("_Brightness", brightness);
            material.SetFloat ("_Saturation", saturation);
            material.SetFloat ("_Contrast", contrast);
            //若材质可用，将参数传递给材质，在调用Graphics.Blit进行处理
            Graphics.Blit (src, dest, material);
        } else {
            //否则将图像直接输出到屏幕，不做任何处理
            Graphics.Blit (src, dest);
        }
    }
}