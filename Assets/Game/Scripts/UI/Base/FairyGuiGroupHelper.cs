using System;
using FairyGUI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game
{
    public class FairyGuiGroupHelper : UIGroupHelperBase
    {
        public const int DepthFactor = 10000;
        
        private int m_Depth = 0;

        public override void SetDepth(int depth)
        {
            m_Depth = depth * DepthFactor;
        }
    }
}
