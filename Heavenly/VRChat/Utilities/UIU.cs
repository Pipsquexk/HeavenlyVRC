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
        public static QuickMenu GetQuickMenu()
        {
            return QuickMenu.field_Private_Static_QuickMenu_0;
        }

        public static void CloseVRCUI()
        {
            VRCUiManager.prop_VRCUiManager_0.Method_Public_Void_Boolean_Boolean_1(true, false);
        }

        public static Il2CppSystem.Action CloseVRCUIAction()
        {
            return DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(new Action(() => { UIU.CloseVRCUI(); }));
        }
        public static void OpenVRCUIPopup(string title, string body, string acceptText, Action acceptAction, string declineText, Action declineAction = null)
        {
            var newAcceptAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(acceptAction);
            if (declineAction == null)
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, body, acceptText, newAcceptAction, declineText, CloseVRCUIAction());
            }
            else
            {
                var newDeclineAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(declineAction);
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, body, acceptText, newAcceptAction, declineText, newDeclineAction);
            }

        }

        public static void OpenVRCUINotifPopup(string title, string body, string okayText, Action okayAction = null)
        {
            if (okayAction == null)
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_0(title, body, okayText, CloseVRCUIAction());
            }
            else
            {
                var newOkayAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(okayAction);
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_0(title, body, okayText, newOkayAction);
            }

        }

        public static void OpenKeyboardPopup(string title, string placeholderText, System.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text> action)
        {
            var newAction = DelegateSupport.ConvertDelegate<Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>>(action);
            VRCUiPopupManager.prop_VRCUiPopupManager_0.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_Boolean_Int32_0(title, null, InputField.InputType.Standard, false, "Okay", newAction, CloseVRCUIAction(), placeholderText);
        }
    }
}
