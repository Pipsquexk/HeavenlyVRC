using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NPButtonAPI.API
{
    public static class NPMenuUtils
    {
        public static GameObject QuickMenu
        {
            get
            {
                return Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>()[0].gameObject;
            }
        }

        public static GameObject UserInteractMenu
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_SelectedUser_Remote").gameObject;
            }
        }
        public static GameObject AudioSettingsPanel
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_AudioSettings").gameObject;
            }
        }
        public static GameObject DashboardMenu
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup").gameObject;
            }
        }
        public static GameObject MenuHeader
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickLinks").gameObject;
            }
        }
        public static GameObject MenuButtonsHolder
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks").gameObject;
            }
        }

        public static GameObject SingleButton
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn").gameObject;
            }
        }

        public static GameObject TabButton
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Dashboard").gameObject;
            }
        }

        public static GameObject ToggleButton
        {
            get
            {
                return UserInteractMenu.transform.Find("Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_Mute").gameObject;
            }
        }

        public static GameObject VRCPlusCancer
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners").gameObject;
            }
        }

        public static GameObject StupidBanner
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners").gameObject;
            }
        }
        public static GameObject RWingsSingleButton
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/Wing_Right/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Emotes").gameObject;
            }
        }
        public static GameObject RWingsMenu
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/Wing_Right/Container/InnerContainer/WingMenu").gameObject;
            }
        }
        public static GameObject LWingsSingleButton
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/Wing_Left/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Emotes").gameObject;
            }
        }
        public static GameObject Tooltip
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/ToolTipPanel").gameObject;
            }
        }

        //public static Sprite LoadSpriteFromDisk(string path)
        //{
        //    if (string.IsNullOrEmpty(path))
        //    {
        //        return null;
        //    }

        //    byte[] data = File.ReadAllBytes(path);

        //    if (data == null || data.Length <= 0)
        //    {
        //        return null;
        //    }

        //    Texture2D tex = new Texture2D(512, 512);

        //    if (!Il2CppImageConversionManager.LoadImage(tex, data))
        //    {
        //        return null;
        //    }

        //    Sprite sprite = Sprite.CreateSprite(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, 0, new Vector4(), false);

        //    sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

        //    return sprite;
        //}

        public static void ShowTooltip(string text = "<color=red>YOU A BITCH</color>")
        {
            Tooltip.GetComponent<UiTooltipPanel>().tooltipText.text = text;
            Tooltip.SetActive(true);
            MelonLoader.MelonCoroutines.Start(HideTooltip());
        }
        private static IEnumerator HideTooltip()
        {
            yield return new WaitForSeconds(5f);
            Tooltip.SetActive(false);
            Tooltip.GetComponent<UiTooltipPanel>().tooltipText.text = "";
        }
        public static GameObject LWingsMenu
        {
            get
            {
                return QuickMenu.transform.Find("Container/Window/Wing_Left/Container/InnerContainer/WingMenu").gameObject;
            }
        }
    }
}
