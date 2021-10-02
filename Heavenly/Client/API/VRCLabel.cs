using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Heavenly.Client.API
{
    public class VRCLabel : RubyButtonAPI.QMButtonBase
    {
        public VRCLabel(RubyButtonAPI.QMNestedButton btnMenu, Vector2 location, String text)
        {
            btnQMLoc = btnMenu.getMenuName();
            initButton(btnQMLoc, location, text);
        }

        public VRCLabel(string btnMenu, Vector2 location, String text)
        {
            btnQMLoc = btnMenu;
            initButton(btnQMLoc, location, text);
        }

        private void initButton(String menu, Vector2 location, String text)
        {
            var origText = RubyButtonAPI.QMStuff.GetQuickMenuInstance().transform.Find($"{menu}/SingleButton(5,2)").GetComponentInChildren<Text>();

            btnType = "VRCLabel";

            button = UnityEngine.Object.Instantiate(origText.gameObject, RubyButtonAPI.QMStuff.GetQuickMenuInstance().transform.Find(btnQMLoc), true);

            //button.GetComponent<Text>().supportRichText = true;
            button.GetComponent<Text>().text = text;

            button.GetComponent<RectTransform>().anchoredPosition += new Vector2(location.x * 420, location.y * 420);
            button.SetActive(true);

            button.name = $"VRCLabel {location.ToString()}";
        }

        public void setText(string buttonText)
        {
            button.GetComponent<Text>().text = buttonText;
        }
    }
}

