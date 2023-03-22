using System.Collections.Generic;
using FairyGUI;
using GameFramework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public class FairyGuiPackageAsset : ScriptableObject
    {
        private static readonly Dictionary<string, int> PackageUsingDic = new Dictionary<string, int>();

        [SerializeField] private string m_PackageName;
        [SerializeField] private string[] m_ComponentNames;
        [SerializeField] private TextAsset m_DescAsset;
        [SerializeField] private Object[] m_AssetObjects;

        public string PackageName => m_PackageName;
        public string[] ComponentNames => m_ComponentNames;

        public GObject CreateObject(string componentName)
        {
            return UIPackage.CreateObject(m_PackageName, componentName);
        }

        public void Set(string packageName,string[] componentNames, TextAsset textAsset, Object[] assetObjects)
        {
            m_PackageName = packageName;
            m_ComponentNames = componentNames;
            m_DescAsset = textAsset;
            m_AssetObjects = assetObjects;
        }
        

        public void Init()
        {
            if (!PackageUsingDic.ContainsKey(m_PackageName))
            {
                PackageUsingDic.Add(m_PackageName, 1);
            }
            else
            {
                PackageUsingDic[m_PackageName] += 1;
            }
            if (UIPackage.GetById(m_PackageName) == null && UIPackage.GetByName(m_PackageName) == null)
            {
                UIPackage.AddPackage(m_DescAsset.bytes, "", LoadAssetFunc);
            }
        }

        public void Clear()
        {
            PackageUsingDic[m_PackageName] -= 1;
            if (PackageUsingDic[m_PackageName] == 0)
            {
                if (UIPackage.GetById(m_PackageName) != null || UIPackage.GetByName(m_PackageName) != null)
                {
                    UIPackage.RemovePackage(m_PackageName);
                }
            }
        }

        private object LoadAssetFunc(string name, string extension, System.Type type, out DestroyMethod destroyMethod)
        {
            destroyMethod = DestroyMethod.Unload;
            if (m_AssetObjects == null || m_AssetObjects.Length < 1)
                return null;
            name = Utility.Text.Format("{0}_{1}", m_PackageName, name);
            foreach (Object assetObject in m_AssetObjects)
            {
                if (string.Equals(name, assetObject.name))
                {
                    return assetObject;
                }
            }
            return null;
        }
    }
}
