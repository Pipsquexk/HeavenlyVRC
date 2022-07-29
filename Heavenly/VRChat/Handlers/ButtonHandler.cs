using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace Heavenly.VRChat.Handlers
{
    public static class ButtonHandler
    {
        public static GameObject GetUIBackground() => GameObject.Find("/UserInterface/MenuContent/Backdrop/Backdrop/Background");

        public static GameObject GetQuickMenuBackground() => GameObject.Find("/UserInterface/QuickMenu/QuickMenu_NewElements/_Background/Panel");

        public static GameObject GetCloneAvatarButton() => GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/CloneAvatarButton");

        public static GameObject GetUserInteractMenu() => GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu");

        public static GameObject GetShortcutMenu() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu");

        public static GameObject GetChangeAvatarButton() => GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Change Button");

        public static GameObject GetViewUserOnVRChatWebsiteButton() => GameObject.Find("/UserInterface/MenuContent/Screens/UserInfo/ViewUserOnVRChatWebsiteButton");

        public static GameObject GetOriginalAvatarFavoriteButton() => GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Favorite Button");

        public static GameObject GetAvatarStatsButton() => GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Stats Button");

        public static GameObject GetQuickMenuNotifTab() => GameObject.Find("/UserInterface/QuickMenu/QuickModeTabs/NotificationsTab");

        public static GameObject GetGoHomeButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GoHomeButton");

        public static GameObject GetUIElementsButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/UIElementsButton");

        public static GameObject GetAvatarButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/AvatarButton");

        public static GameObject GetRespawnButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/RespawnButton");

        public static GameObject GetCameraButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/CameraButton");

        public static GameObject GetSitButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton");

        public static GameObject GetSitOFF() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF");

        public static GameObject GetSitON() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/ON");

        public static GameObject GetSitOFFText() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Text_seated");

        public static GameObject GetSitONText() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Text_standing");

        public static GameObject GetSitOFFImage() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Image");

        public static GameObject GetSitONImage() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SitButton/Toggle_States_StandingEnabled/OFF/Seated");

        public static GameObject GetSocialButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SocialButton");

        public static GameObject GetEmoteButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/EmoteButton");

        public static GameObject GetEmojiButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/EmojiButton");

        public static GameObject GetSettingsButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/SettingsButton");

        public static GameObject GetReportWorldButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/ReportWorldButton");

        public static GameObject GetGalleryButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GalleryButton");

        public static GameObject GetGalleryButtonVRCTag() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GalleryButton/VRC+");

        public static GameObject GetLearnMoreButton() => GameObject.Find("/UserInterface/QuickMenu/ShortcutMenu/GalleryButton/Hover Over/Learn More");

        public static GameObject GetBlockButton() => GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/BlockButton");

        public static GameObject GetBlockButtonON() => GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/BlockButton/Toggle_States_Visible/ON");

        public static GameObject GetBlockButtonOFF() => GameObject.Find("/UserInterface/QuickMenu/UserInteractMenu/BlockButton/Toggle_States_Visible/OFF");

        public static GameObject GetMicControls() => GameObject.Find("/UserInterface/QuickMenu/MicControls");

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
