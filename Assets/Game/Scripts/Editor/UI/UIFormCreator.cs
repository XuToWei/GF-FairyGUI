using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityGameFramework.Editor;

namespace Game.Editor
{
    //创建通用的脚本转发层，测试代码先删除
    /*
    public static class UIFormCreator
    {
        private static readonly string UIFormAssetPath = "Assets/Game/UI/UIForms/";
        
        [MenuItem("Assets/Create/UIForm", false, 80)]
        private static void CreateUIForm()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<DoCreatePrefab>(), "New UIForm.prefab", EditorGUIUtility.FindTexture("Prefab Icon"), (string) null);
        }

        private class DoCreatePrefab : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                GameObject instanceRoot = new GameObject("New UIForm");
                instanceRoot.SetLayerRecursively(Constant.Layer.UILayerId);
                LuaUIForm uiForm = instanceRoot.AddComponent<LuaUIForm>();
                uiForm.CreateSet(Path.GetFileNameWithoutExtension(pathName));
                Object o = (Object) PrefabUtility.SaveAsPrefabAsset(instanceRoot, pathName, out bool _);
                Object.DestroyImmediate((Object) instanceRoot);
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }
    }
    */
}
