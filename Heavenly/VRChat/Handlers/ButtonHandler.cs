using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Heavenly.VRChat.Handlers
{
    public static class ButtonHandler
    {

        public static GameObject GetUIBackground()
        {
            return GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/Background");
        }
        public static GameObject GetQuickMenuBackground()
        {
            return GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_Background/Panel");
        }
        public static GameObject GetCloneAvatarButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/CloneAvatarButton");
        }
        public static GameObject GetUserInteractMenu()
        {
            return GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu");
        }
        public static GameObject GetShortcutMenu()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu");
        }
        public static GameObject GetChangeAvatarButton()
        {
            return GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Change Button");
        }
        public static GameObject GetViewUserOnVRChatWebsiteButton()
        {
            return GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/ViewUserOnVRChatWebsiteButton");
        }
        public static GameObject GetOriginalAvatarFavoriteButton()
        {
            return GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Favorite Button");
        }
        public static GameObject GetAvatarStatsButton()
        {
            return GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Stats Button");
        }
        public static GameObject GetQuickMenuNotifTab()
        {
            return GameObject.Find("/UserInterface/QuickMenu/QuickModeTabs/NotificationsTab");
        }

        public static GameObject GetGoHomeButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GoHomeButton");
        }

        public static GameObject GetUIElementsButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/UIElementsButton");
        }

        public static GameObject GetAvatarButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/AvatarButton");
        }
        public static GameObject GetRespawnButton()
        { 
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/RespawnButton");
        }
        public static GameObject GetCameraButton()
        { 
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/CameraButton");
        }
        public static GameObject GetSitButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton");
        }
        public static GameObject GetSitOFF()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF");
        }
        public static GameObject GetSitON()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/ON");
        }
        public static GameObject GetSitOFFText()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Text_seated");
        }
        public static GameObject GetSitONText()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Text_standing");
        }
        public static GameObject GetSitOFFImage()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Image");
        }
        public static GameObject GetSitONImage()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Seated");
        }
        public static GameObject GetSocialButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SocialButton");
        }
        public static GameObject GetEmoteButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/EmoteButton");
        }
        public static GameObject GetEmojiButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/EmojiButton");
        }
        public static GameObject GetSettingsButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SettingsButton");
        }
        public static GameObject GetReportWorldButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/ReportWorldButton");
        }
        public static GameObject GetGalleryButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GalleryButton");
        }
        public static GameObject GetGalleryButtonVRCTag()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GalleryButton/VRC+");
        }
        public static GameObject GetLearnMoreButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GalleryButton/Hover Over/Learn More");
        }
        public static GameObject GetBlockButton()
        {
            return GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/BlockButton");
        }
        public static GameObject GetBlockButtonON()
        {
            return GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/BlockButton/Toggle_States_Visible/ON");
        }
        public static GameObject GetBlockButtonOFF()
        {
            return GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/BlockButton/Toggle_States_Visible/OFF");
        }
        public static GameObject GetMicControls()
        {
            return GameObject.Find("/UserInterface/QuickMenu/MicControls");
        }

        public static void SetButtonColor(GameObject gameObject, Color color)
        {
            gameObject.GetComponentInChildren<Button>().colors = new ColorBlock()
            {
                colorMultiplier = 1f,
                normalColor = color / 1.4f,
                highlightedColor = color * 1.5f,
                pressedColor = Color.grey,
                selectedColor = color / 1.1f,
                disabledColor = color / 1.5f
            };
        }

        public static void SetImageColor(GameObject gameObject, Color color)
        {
            gameObject.GetComponentInChildren<Image>().color = color;
        }
    }
}
