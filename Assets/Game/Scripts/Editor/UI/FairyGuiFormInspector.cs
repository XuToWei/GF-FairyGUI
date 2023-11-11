using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityGameFramework.Editor;

namespace Game.Editor
{
    [CustomEditor(typeof(FairyGuiForm), true)]
    public class FairyGuiFormInspector : GameFrameworkInspector
    {
        private SerializedProperty m_FairyGuiPackageAssetSerializedProperty;
        private SerializedProperty m_ComponentNameSerializedProperty;
        //private SerializedProperty m_LuaClassName;

        private int m_FairyGuiPackageAssetIndex;
        private int m_FairyGuiComponentNameIndex;

        private List<string> m_FairyGuiPackageAssetNames;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                int index = EditorGUILayout.Popup("FairyGui Package Asset", m_FairyGuiPackageAssetIndex, m_FairyGuiPackageAssetNames.ToArray());
                if (index != m_FairyGuiPackageAssetIndex)
                {
                    m_FairyGuiComponentNameIndex = -1;
                    m_ComponentNameSerializedProperty.stringValue = string.Empty;
                }
                m_FairyGuiPackageAssetIndex = index;
                if (m_FairyGuiPackageAssetIndex >= 0)
                {
                    FairyGuiPackageAsset packageAsset = AssetDatabase.LoadAssetAtPath<FairyGuiPackageAsset>(FairyGuiEditor.FairyGuiPackagesPath + m_FairyGuiPackageAssetNames[m_FairyGuiPackageAssetIndex]);
                    m_FairyGuiPackageAssetSerializedProperty.objectReferenceValue = packageAsset;
                    if (packageAsset != null)
                    {
                        if (packageAsset.ComponentNames != null && packageAsset.ComponentNames.Length > 0)
                        {
                            m_FairyGuiComponentNameIndex = EditorGUILayout.Popup("Component Name", m_FairyGuiComponentNameIndex, packageAsset.ComponentNames);
                            if (m_FairyGuiComponentNameIndex >= 0)
                            {
                                m_ComponentNameSerializedProperty.stringValue = packageAsset.ComponentNames[m_FairyGuiComponentNameIndex];
                            }
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Component Name not exist! Please Check!");
                        }
                    }
                }
                //EditorGUILayout.PropertyField(m_LuaClassName);
            }
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        void OnEnable()
        {
            m_FairyGuiPackageAssetSerializedProperty = serializedObject.FindProperty("m_FairyGuiPackageAsset");
            m_ComponentNameSerializedProperty = serializedObject.FindProperty("m_ComponentName");
            m_FairyGuiPackageAssetNames = FairyGuiEditor.GetFairyGuiPackages();
            FairyGuiPackageAsset packageAsset = m_FairyGuiPackageAssetSerializedProperty.objectReferenceValue as FairyGuiPackageAsset;
            if (packageAsset == null || !m_FairyGuiPackageAssetNames.Contains($"{packageAsset.PackageName}.asset"))
            {
                m_FairyGuiPackageAssetIndex = -1;
                m_FairyGuiPackageAssetSerializedProperty.objectReferenceValue = null;
            }
            else
            {
                m_FairyGuiPackageAssetIndex = m_FairyGuiPackageAssetNames.IndexOf($"{packageAsset.PackageName}.asset");
                string componentName = m_ComponentNameSerializedProperty.stringValue;
                if (string.IsNullOrEmpty(componentName) || !packageAsset.ComponentNames.Contains(componentName))
                {
                    m_FairyGuiComponentNameIndex = -1;
                    m_ComponentNameSerializedProperty.stringValue = string.Empty;
                }
                else
                {
                    m_FairyGuiComponentNameIndex = packageAsset.ComponentNames.ToList().IndexOf(componentName);
                }
            }
            //m_LuaClassName = serializedObject.FindProperty("m_LuaClassName");
        }
    }
}
