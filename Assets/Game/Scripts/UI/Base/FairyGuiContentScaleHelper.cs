using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace Game
{
    public enum FairyUIScaleType
    {
        Width,
        Height
    }

    public class FairyGuiContentScaleHelper : MonoBehaviour
    {
        private UIContentScaler m_UIContentScaler;

        private static int _designResolutionX = 1080;
        private static int _designResolutionY = 1920;

        private void Awake()
        {
            m_UIContentScaler = gameObject.GetOrAddComponent<UIContentScaler>();
            m_UIContentScaler.designResolutionX = _designResolutionX;
            m_UIContentScaler.designResolutionY = _designResolutionY;
            m_UIContentScaler.scaleMode = UIContentScaler.ScaleMode.ScaleWithScreenSize;
            m_UIContentScaler.screenMatchMode = GetFairyUIScaleType() == FairyUIScaleType.Width ? UIContentScaler.ScreenMatchMode.MatchWidth : UIContentScaler.ScreenMatchMode.MatchHeight;
            m_UIContentScaler.ApplyChange();
        }

        public static FairyUIScaleType GetFairyUIScaleType()
        {
            Rect safeArea = Screen.safeArea;
            return safeArea.width / safeArea.height < _designResolutionX * 1.0f / _designResolutionY ? FairyUIScaleType.Width : FairyUIScaleType.Height;
        }
    }
}
