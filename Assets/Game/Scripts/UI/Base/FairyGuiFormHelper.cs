using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game
{
    public class FairyGuiFormHelper : UIFormHelperBase
    {
        public override object InstantiateUIForm(object uiFormAsset)
        {
            return Instantiate((Object)uiFormAsset);
        }
        
        public override IUIForm CreateUIForm(object uiFormInstance, IUIGroup uiGroup, object userData)
        {
            GameObject gameObject = uiFormInstance as GameObject;
            if (gameObject == null)
            {
                Log.Error("UI form instance is invalid.");
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(((MonoBehaviour)uiGroup.Helper).transform);

            FairyGuiForm fairyGuiForm = gameObject.GetComponent<FairyGuiForm>();
            fairyGuiForm.CreateUIForm(uiGroup, userData);
            
            return gameObject.GetOrAddComponent<UIForm>();
        }
        
        public override void ReleaseUIForm(object uiFormAsset, object uiFormInstance)
        {
            FairyGuiForm fairyGuiForm = ((GameObject)uiFormInstance).GetComponent<FairyGuiForm>();
            fairyGuiForm.ReleaseUIForm();
            
            GameEntry.Resource.UnloadAsset(uiFormAsset);
            Destroy((Object)uiFormInstance);
        }
    }
}
