using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectBase : MonoBehaviour {
        protected void CheckResources () {
            bool isSupported = CheckSupport ();
            if (isSupported == false) {
                NotSupport ();
            }
        }

        protected bool CheckSupport () {
            if (SystemInfo.supportsImageEffects == false || SystemInfo.supportsRenderTextures == false) {
                return false;
            }
            return true;
        }

        protected void NotSupport () {
            enabled = false;
        }
        // Use this for initialization
        void Start () {
            //检查资源和条件是否支持屏幕后处理
            CheckResources ();
        }

        protected Material CheckShaderAndCreateMaterial (Shader shader, Material material) {
            if (shader == null) {
                return null;
            }
            if (shader.isSupported && material && material.shader == shader) {
                return material;
            }
            if (!shader.isSupported) {
                return null;
            } else {
                material = new Material (shader);
                material.hideFlags = HideFlags.DontSave;
                if (material)
                    return material;
                else
                    return null;
            }
        }
    }