using FairyGUI;
using UnityEngine;

namespace Game
{
    public class TestUIForm : FairyGuiForm
    {

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            UI.GetChild("bagBtn").asButton.onClick.Set(delegate(EventContext context)
            {
                Debug.Log("点击了按钮！！！");
            });
        }
    }
}