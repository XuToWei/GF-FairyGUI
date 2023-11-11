using System.Collections.Generic;
using System.IO;
using FairyGUI;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Editor
{
    public class FairyGuiEditor : AssetPostprocessor
    {
        public const string FairyGuiAssetsPath = "Assets/Game/UI/FairyGuis/";
        public const string FairyGuiPackagesPath = "Assets/Game/UI/FairyGuiPackages/";

        private static Dictionary<string, TextAsset> m_FuiDescAssetDict = new Dictionary<string, TextAsset>();
        private static Dictionary<string, List<Object>> m_FuiAssetsDict = new Dictionary<string, List<Object>>();

        public static List<string> GetFairyGuiPackages()
        {
            List<string> results = new List<string>();
            foreach (string asset in Directory.GetFiles(FairyGuiPackagesPath))
            {
                if (asset.EndsWith(".asset"))
                {
                    string[] strArr = asset.Split('/');
                    string name = strArr[strArr.Length - 1];
                    results.Add(name);
                }
            }
            return results;
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (importedAssets.Length > 0)
            {
                if (CheckDir(importedAssets))
                {
                    ImportAsset(importedAssets);
                }
            }

            if (movedAssets.Length > 0)
            {
                if (CheckDir(movedAssets))
                {
                    ImportAsset(movedAssets);
                }
            }

            if (deletedAssets.Length > 0)
            {
                if (CheckDir(deletedAssets))
                {
                    DeletePackageAsset(deletedAssets);
                }
            }


            if (movedFromAssetPaths.Length > 0)
            {
                if (CheckDir(movedFromAssetPaths))
                {
                    DeletePackageAsset(movedFromAssetPaths);
                }
            }
        }

        private static void ImportAsset(string[] importedAssets)
        {
            CollectAsset(importedAssets);
            GenerateFairyGuiPackageAsset();
        }

        private static void DeletePackageAsset(string[] deletedAssets)
        {

            for (int i = 0; i < deletedAssets.Length; i++)
            {
                string importName = deletedAssets[i];
                string[] strArr = importName.Split('_');
                string name = strArr[0];
                int index = name.LastIndexOf("/");
                name = name.Substring(index + 1);
                string filePath = $"{FairyGuiPackagesPath}{name}.asset";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }


        private static void CollectAsset(string[] importedAssets)
        {
            m_FuiDescAssetDict.Clear();
            m_FuiAssetsDict.Clear();
            for (int i = 0; i < importedAssets.Length; i++)
            {
                string importName = importedAssets[i];
                if (!importName.Contains("_"))
                {
                    continue;
                }

                int charIndex = importName.IndexOf("_");
                string prePartName = importName.Substring(0, charIndex);

                int index = prePartName.LastIndexOf("/");
                string name = prePartName.Substring(index + 1);
                string posfixName = importName.Substring(charIndex + 1);
                if (importName.EndsWith(".bytes") && posfixName.StartsWith("fui"))
                {
                    m_FuiDescAssetDict[name] = AssetDatabase.LoadAssetAtPath<TextAsset>(importName);
                }
                else if (importName.EndsWith(".png") && posfixName.StartsWith("atlas") || importName.EndsWith(".wav"))
                {
                    if (!m_FuiAssetsDict.ContainsKey(name))
                    {
                        m_FuiAssetsDict.Add(name, new List<Object>
                        {
                            AssetDatabase.LoadAssetAtPath<Object>(importName)
                        });
                    }
                    else
                    {
                        m_FuiAssetsDict[name].Add(AssetDatabase.LoadAssetAtPath<Object>(importName));
                    }
                }
            }
        }

        private static void GenerateFairyGuiPackageAsset()
        {
            if (m_FuiDescAssetDict.Count <= 0 && m_FuiAssetsDict.Count <= 0)
            {
                return;
            }

            if (!Directory.Exists(FairyGuiPackagesPath))
            {
                Directory.CreateDirectory(FairyGuiPackagesPath);
            }

            List<string> componentNames = new List<string>();
            foreach (var key in m_FuiDescAssetDict.Keys)
            {
                string name = key;
                FairyGuiPackageAsset asset;
                string packageAssetPath = $"{FairyGuiPackagesPath}{name}.asset";
                if (!File.Exists(packageAssetPath))
                {
                    asset = ScriptableObject.CreateInstance<FairyGuiPackageAsset>();
                    asset.name = name;
                    AssetDatabase.CreateAsset(asset, packageAssetPath);
                }
                else
                {
                    asset = AssetDatabase.LoadAssetAtPath<FairyGuiPackageAsset>(packageAssetPath);
                }
                
                UIPackage pkg = UIPackage.AddPackage($"{FairyGuiAssetsPath}{name}");
                componentNames.Clear();
                
                foreach (PackageItem pi in pkg.GetItems())
                {
                    if (pi.type == PackageItemType.Component && pi.exported)
                    {
                        componentNames.Add(pi.name);
                    }
                }
                
                if (m_FuiAssetsDict.ContainsKey(name))
                {
                    asset.Set(name, componentNames.ToArray(), m_FuiDescAssetDict[name], m_FuiAssetsDict[name].ToArray());
                }
                else
                {
                    asset.Set(name, componentNames.ToArray(), m_FuiDescAssetDict[name], null);
                }
                
                UIPackage.RemovePackage(pkg.name);
                EditorUtility.SetDirty(asset);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static bool CheckDir(string[] dirs)
        {
            for (int i = 0; i < dirs.Length; i++)
            {
                string str = dirs[i];
                if (str.StartsWith(FairyGuiAssetsPath))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
