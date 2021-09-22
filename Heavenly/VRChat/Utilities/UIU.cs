using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

namespace Heavenly.VRChat.Utilities
{
    public static class UIU
    {
        public static void CloseVRCUI()
        {
            VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_Boolean_1(true, false);
        }

        public static Il2CppSystem.Action CloseVRCUIAction()
        {
            return DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(new Action(() => { UIU.CloseVRCUI(); }));
        }
        public static void OpenVRCUIPopup(string title, string body, string acceptText, Il2CppSystem.Action acceptAction, string declineText, Il2CppSystem.Action declineAction = null)
        {
            if (declineAction == null)
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, body, acceptText, acceptAction, declineText, CloseVRCUIAction());
            }
            else
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, body, acceptText, acceptAction, declineText, declineAction);
            }

        }

        public static void OpenKeyboardPopup(string title, string placeholderText, Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text> action)
        {
            VRCUiPopupManager.prop_VRCUiPopupManager_0.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_Boolean_Int32_0(title, null, InputField.InputType.Standard, false, "Okay", action, CloseVRCUIAction(), placeholderText);
        }
    }
}
