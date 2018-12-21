using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
public class PropertyEditWindow : EditorWindow {

    [Serializable]
    private struct PropertyData {
        public string name;
        public GridInformationType valueType;
        public string valueRaw;
        public Vector3Int position;

        public bool toRemove;

        public PropertyData (Vector3Int p, string n, GridInformationType t, string vr) {
            name = n;
            valueType = t;
            position = p;
            valueRaw = vr;
            toRemove = false;
        }
        public PropertyData (Vector3Int p, string n, GridInformationType t, string vr, bool rv) {
            name = n;
            valueType = t;
            position = p;
            valueRaw = vr;
            toRemove = rv;
        }

    }

    private PropertyData[] m_PositionProperties;

    private GridInformation gi;

    private Vector3Int m_position;
    public static PropertyEditWindow Initialize (Vector3Int position, ref GridInformation gridInformation) {
        PropertyEditWindow window = (PropertyEditWindow) PropertyEditWindow.GetWindow (typeof (PropertyEditWindow));
        window.gi = gridInformation;
        window.m_PositionProperties = InitData (gridInformation.getPropertiesInAGrid (position));
        window.m_position = position;
        window.Show ();
        return window;
    }

    static PropertyData[] InitData (Dictionary<GridInformation.GridInformationKey, GridInformation.GridInformationValue> PositionProperties) {
        PropertyData[] ret;
        if (PositionProperties.Count == 0) {
            ret = null;
        } else {
            ret = new PropertyData[PositionProperties.Count];
            int i = 0;
            foreach (var item in PositionProperties) {
                ret[i] = (new PropertyData (
                    item.Key.position,
                    item.Key.name,
                    item.Value.type,
                    item.Value.data.ToString ()
                ));
                i++;
            }
        }

        return ret;
    }

    void AddProperty () {
        if (m_PositionProperties != null) {
            Array.Resize<PropertyData> (ref m_PositionProperties, m_PositionProperties.Length + 1);
        } else {
            m_PositionProperties = new PropertyData[1];
        }

        m_PositionProperties[m_PositionProperties.Length - 1] = new PropertyData (this.m_position, "填写属性名称", GridInformationType.String, "填写值");
    }

    void Save () {
        for (int i = 0; i < m_PositionProperties.Length; i++) {
            PropertyData data = m_PositionProperties[i];
            if (data.toRemove) {
                gi.ErasePositionProperty (data.position, data.name);
                continue;
            }
            switch (data.valueType) {
                case GridInformationType.Integer:
                    gi.SetPositionProperty (data.position, data.name, Int32.Parse (data.valueRaw));
                    break;
                case GridInformationType.String:
                    gi.SetPositionProperty (data.position, data.name, data.valueRaw);
                    break;
                case GridInformationType.Float:
                    gi.SetPositionProperty (data.position, data.name, float.Parse (data.valueRaw));
                    break;
                case GridInformationType.Double:
                    gi.SetPositionProperty (data.position, data.name, double.Parse (data.valueRaw));
                    break;
            }
        }
        m_PositionProperties = InitData (gi.getPropertiesInAGrid (m_position));
    }

    void OnGUI () {
        EditorGUI.BeginChangeCheck ();

        GUI.enabled = false;
        EditorGUILayout.Vector3IntField ("位置：", m_position, null);
        int count = EditorGUILayout.DelayedIntField ("属性数量", m_PositionProperties != null ? m_PositionProperties.Length : 0);
        GUI.enabled = true;

        if (count < 0)
            count = 0;

        if (count == 0) {
            if (GUILayout.Button ("增加属性")) {
                AddProperty ();
            }
            return;

        } else {

            if (m_PositionProperties == null || m_PositionProperties.Length != count) {
                Array.Resize<PropertyData> (ref m_PositionProperties, count);
            }

            EditorGUILayout.LabelField ("属性列表.");
            EditorGUILayout.Space ();

            for (int i = 0; i < count; i++) {
                var toRemove = m_PositionProperties[i].toRemove;
                if (toRemove) {
                    continue;
                }
                var name = EditorGUILayout.TextField ("名称：", m_PositionProperties[i].name);
                var valueType = (GridInformationType) EditorGUILayout.EnumPopup ("类型：", m_PositionProperties[i].valueType);
                var value = EditorGUILayout.TextField ("值：", m_PositionProperties[i].valueRaw);
                if (GUILayout.Button ("去除属性")) {
                    toRemove = true;
                }
                m_PositionProperties[i] = new PropertyData (
                    m_position,
                    name,
                    valueType,
                    value,
                    toRemove
                );
            }

            EditorGUILayout.Space ();

            if (GUILayout.Button ("增加属性")) {
                AddProperty ();
            }

            if (GUILayout.Button ("保存")) {
                this.Save ();
                this.ShowNotification (new GUIContent ("保存成功"));
            }

            EditorGUI.EndChangeCheck ();
            // EditorUtility.SetDirty (m_PositionProperties);
        }
    }
}

#endif