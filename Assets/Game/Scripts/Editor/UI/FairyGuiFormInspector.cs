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
        private int m_FairyGuiComponentNameIndex;

        private List<string> m_FairyGuiPackageAssetNames;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            RefreshFairyGuiPackageAsset();
            FairyGuiForm t = (FairyGuiForm)target;
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                int selectedIndex = EditorGUILayout.Popup("FairyGui Package Asset", m_FairyGuiPackageAssetIndex, m_FairyGuiPackageAssetNames.ToArray());
                if (selectedIndex != m_FairyGuiPackageAssetIndex)
                {
                    m_FairyGuiPackageAssetIndex = selectedIndex;
                    m_FairyGuiPackageAsset.objectReferenceValue = AssetDatabase.LoadAssetAtPath<FairyGuiPackageAsset>(FairyGuiEditor.FairyGuiPackagesPath + m_FairyGuiPackageAssetNames[selectedIndex]);
                }

                if (m_FairyGuiPackageAssetIndex >= 0)
                {
                    FairyGuiPackageAsset packageAsset = (FairyGuiPackageAsset)m_FairyGuiPackageAsset.objectReferenceValue;
                    if (packageAsset.ComponentNames != null && packageAsset.ComponentNames.Length > 0)
                    {
                        selectedIndex = EditorGUILayout.Popup("Component Name", m_FairyGuiComponentNameIndex, packageAsset.ComponentNames);
                        if (selectedIndex != m_FairyGuiComponentNameIndex)
                        {
                            m_FairyGuiComponentNameIndex = selectedIndex;
                            m_ComponentName.stringValue = packageAsset.ComponentNames[m_FairyGuiComponentNameIndex];
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Component Name not exist! Please Check!");
                    }
                }
                
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
