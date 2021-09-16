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
        public static GameObject GetChangeAvatarButton()
        {
            return GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Change Button");
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
    }
}
