using FairyGUI;
using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game
{
    public abstract class FairyGuiForm : UIFormLogic
    {
        public const int DepthFactor = 100;
        
        [SerializeField] private FairyGuiPackageAsset m_FairyGuiPackageAsset;
        [SerializeField] private string m_ComponentName;
        
        /// <summary>
        /// 显示容器
        /// </summary>
        private UIPanel m_CachedUIPanel;

        //组件名称
        public string ComponentName => m_ComponentName;

        public FairyGuiPackageAsset FairyGuiPackageAsset => m_FairyGuiPackageAsset;

        public GComponent UI
        {
            get;
            private set;
        }

        public int Depth => m_CachedUIPanel.sortingOrder;

        private bool m_IsCreated = false;

        public void CreateUIForm(IUIGroup uiGroup, object userData)
        {
            if (m_IsCreated)
                return;
            m_IsCreated = true;
            FairyGuiPackageAsset.Init();
            m_CachedUIPanel = gameObject.GetOrAddComponent<UIPanel>();
            m_CachedUIPanel.packageName = m_FairyGuiPackageAsset.PackageName;
            m_CachedUIPanel.componentName = m_ComponentName;
            gameObject.SetLayerRecursively(Constant.Layer.UILayerId);
            UI = m_CachedUIPanel.ui;
            //屏幕适配
            m_CachedUIPanel.fitScreen = FairyGuiContentScaleHelper.GetFairyUIScaleType() == FairyUIScaleType.Width ? FitScreen.FitWidthAndSetMiddle : FitScreen.FitHeightAndSetCenter;
            //m_CachedUIPanel.fitScreen = FitScreen.FitSize;
            m_CachedUIPanel.ApplyModifiedProperties(false, true);
        }

        public void ReleaseUIForm()
        {
            m_FairyGuiPackageAsset.Clear();
            UI.Dispose();
            UI = null;
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            m_CachedUIPanel.SetSortingOrder(DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup, true);
        }
        

        protected override void InternalSetVisible(bool visible)
        {
            base.InternalSetVisible(visible);
            UI.displayObject.visible = visible;
        }
    }
}
