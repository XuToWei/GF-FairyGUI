using System.Collections.Generic;
using UnityEditor;
using UnityGameFramework.Editor;

namespace Game.Editor
{
    [CustomEditor(typeof(FairyGuiForm), true)]
    public class FairyGuiFormInspector : GameFrameworkInspector
    {
        private SerializedProperty m_FairyGuiPackageAsset;
        private SerializedProperty m_ComponentName;
        //private SerializedProperty m_LuaClassName;

        private int m_FairyGuiPackageAssetIndex;

        private List<string> m_FairyGuiPackageAssetNames;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            RefreshFairyGuiPackageAsset();
            FairyGuiForm t = (FairyGuiForm)target;
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                if (EditorApplication.isPlaying && IsPrefabInHierarchy(t.gameObject))
                {
                    EditorGUILayout.Popup("FairyGui Package Asset", m_FairyGuiPackageAssetIndex, m_FairyGuiPackageAssetNames.ToArray());
                }
                else
                {
                    int selectedIndex = EditorGUILayout.Popup("FairyGui Package Asset", m_FairyGuiPackageAssetIndex, m_FairyGuiPackageAssetNames.ToArray());
                    if (selectedIndex != m_FairyGuiPackageAssetIndex)
                    {
                        m_FairyGuiPackageAssetIndex = selectedIndex;
                        m_FairyGuiPackageAsset.objectReferenceValue = AssetDatabase.LoadAssetAtPath<FairyGuiPackageAsset>(FairyGuiEditor.FairyGuiPackagesPath + m_FairyGuiPackageAssetNames[selectedIndex]);
                    }
                }
                EditorGUILayout.PropertyField(m_ComponentName);
                //EditorGUILayout.PropertyField(m_LuaClassName);
            }
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        private void RefreshFairyGuiPackageAsset()
        {
            m_FairyGuiPackageAssetNames = FairyGuiEditor.GetFairyGuiPackages();
            if (m_FairyGuiPackageAsset.objectReferenceValue == null)
            {
                m_FairyGuiPackageAssetIndex = -1;
            }
            else
            {
                m_FairyGuiPackageAssetIndex = m_FairyGuiPackageAssetNames.IndexOf(m_FairyGuiPackageAsset.objectReferenceValue.name + ".asset");
            }
        }

        void OnEnable()
        {
            m_FairyGuiPackageAsset = serializedObject.FindProperty("m_FairyGuiPackageAsset");
            m_ComponentName = serializedObject.FindProperty("m_ComponentName");
            //m_LuaClassName = serializedObject.FindProperty("m_LuaClassName");
            RefreshFairyGuiPackageAsset();
        }
    }
}
