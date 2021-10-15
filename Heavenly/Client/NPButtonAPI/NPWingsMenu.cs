using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace NPButtonAPI.API
{
    public class NPWingsMenu
    {
        public GameObject buttonObj;
        public TextMeshProUGUI buttonText;

        public GameObject backgroundObj;


        public GameObject opMenu;
        public GameObject origMenusMenu;
        public GameObject origMenu;
        public GameObject menuObj;
        public TextMeshProUGUI menuText;

        public GameObject backButtonObj;

        public Dictionary<string, GameObject> buttons = new Dictionary<string, GameObject>();
        public Dictionary<string, NPWingsToggle> toggleButtons = new Dictionary<string, NPWingsToggle>();

        public enum ButtonParent
        {
            Left,
            Right
        }

        public NPWingsMenu(ButtonParent parent, string text, string toolTip, Sprite icon = null, Color? backgroundColor = null, Color? textColor = null)
        {
            switch (parent)
            {
                case ButtonParent.Left:
                    origMenu = NPMenuUtils.LWingsMenu;
                    opMenu = NPMenuUtils.RWingsMenu;
                    menuObj = GameObject.Instantiate(NPMenuUtils.LWingsMenu, NPMenuUtils.LWingsMenu.transform.parent);
                    buttonObj = GameObject.Instantiate(NPMenuUtils.LWingsSingleButton, NPMenuUtils.LWingsSingleButton.transform.parent);
                    break;
                case ButtonParent.Right:
                    origMenu = NPMenuUtils.RWingsMenu;
                    opMenu = NPMenuUtils.LWingsMenu;
                    menuObj = GameObject.Instantiate(NPMenuUtils.RWingsMenu, NPMenuUtils.RWingsMenu.transform.parent);
                    buttonObj = GameObject.Instantiate(NPMenuUtils.RWingsSingleButton, NPMenuUtils.RWingsSingleButton.transform.parent);
                    break;
            }

            buttonObj.name = $"{parent.ToString()}_{NPButtonAPI.Identifier}-WingButton";
            menuObj.name = text + " Menu" + parent.ToString();

            origMenusMenu = origMenu.transform.parent.parent.parent.gameObject;

            menuText = menuObj.transform.Find("WngHeader_H1/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUI>();

            

            buttonText = buttonObj.transform.Find("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>();
            backgroundObj = buttonObj.transform.Find("Container/Background").gameObject;

            buttonObj.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().text = toolTip;
            buttonObj.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().alternateText = toolTip;

            SetMenuText(text);
            SetBtnText(text);

            SetAction(new Action(() =>
            {
                menuObj.GetComponent<UIPage>().Show(true, (UIPage.TransitionType)Enum.Parse(typeof(UIPage.TransitionType), parent.ToString()));
                origMenu.GetComponent<UIPage>().Show(false, UIPage.TransitionType.InPlace);
                origMenu.GetComponent<UIPage>()._pageStack.Add(menuObj.GetComponent<UIPage>());
            }));

            if (icon != null)
            {
                buttonObj.transform.Find("Container/Icon").GetComponent<Image>().sprite = icon;
            }
            else
            {
                GameObject.DestroyImmediate(buttonObj.transform.Find("Container/Icon").gameObject);
            }

            if(textColor != null)
            {
                SetTextColor(textColor.Value);
            }

            if (backgroundColor != null)
            {
                SetBackgroundColor(backgroundColor.Value);
            }

            foreach (Button button in menuObj.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").GetComponentsInChildren<Button>())
            {
                GameObject.Destroy(button.gameObject);
            }

            AddButton("Back", new Action(() =>
            {
                origMenu.GetComponent<UIPage>().RemovePageFromStack(menuObj.GetComponent<UIPage>());
                menuObj.GetComponent<UIPage>().Show(false, (UIPage.TransitionType)Enum.Parse(typeof(UIPage.TransitionType), parent.ToString()));
                origMenu.GetComponent<UIPage>().Show(true, UIPage.TransitionType.InPlace);
            }));

            menuObj.GetComponent<UIPage>().showSequence = opMenu.GetComponent<UIPage>().showSequence;
            menuObj.GetComponent<UIPage>().Name = text + " Menu";

            origMenusMenu.GetComponent<MenuStateController>()._wings = new UnhollowerBaseLib.Il2CppReferenceArray<Wing>(1);
            origMenusMenu.GetComponent<MenuStateController>()._wings[0] = origMenusMenu.GetComponent<Wing>();
            origMenusMenu.GetComponent<MenuStateController>()._uiPages.Add(text, menuObj.GetComponent<UIPage>());
            origMenusMenu.GetComponent<MenuStateController>().enabled = true;

        }

        public NPWingsMenu(NPWingsMenu parent, string text, string toolTip, Sprite icon = null, Color? backgroundColor = null, Color? textColor = null)
        {
            origMenu = parent.menuObj;
            menuObj = GameObject.Instantiate(origMenu.name.Contains("MenuLeft") ? NPMenuUtils.LWingsMenu : NPMenuUtils.RWingsMenu, NPMenuUtils.LWingsMenu.transform.parent);
            buttonObj = GameObject.Instantiate(NPMenuUtils.RWingsSingleButton, origMenu.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));

            buttonObj.name = $"{parent.ToString()}_{NPButtonAPI.Identifier}-WingButton";
            menuObj.name = text + " Menu" + (origMenu.name.Contains("MenuLeft") ? "Left" : "Right");

            origMenusMenu = origMenu.transform.parent.parent.parent.gameObject;

            menuText = menuObj.transform.Find("WngHeader_H1/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUI>();

            buttonText = buttonObj.transform.Find("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>();
            backgroundObj = buttonObj.transform.Find("Container/Background").gameObject;

            buttonObj.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().text = toolTip;
            buttonObj.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().alternateText = toolTip;

            if (icon != null)
            {
                buttonObj.transform.Find("Container/Icon").GetComponent<Image>().sprite = icon;
            }
            else
            {
                GameObject.DestroyImmediate(buttonObj.transform.Find("Container/Icon").gameObject);
            }

            if (textColor != null)
            {
                SetTextColor(textColor.Value);
            }

            if (backgroundColor != null)
            {
                SetBackgroundColor(backgroundColor.Value);
            }

            SetMenuText(text);
            SetBtnText(text);

            SetAction(new Action(() =>
            {
                menuObj.GetComponent<UIPage>().Show(true, UIPage.TransitionType.InPlace);
                origMenu.GetComponent<UIPage>().Show(false, UIPage.TransitionType.InPlace);
                NPMenuUtils.LWingsMenu.GetComponent<UIPage>()._pageStack.Add(menuObj.GetComponent<UIPage>());
            }));

            foreach (Button button in menuObj.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").GetComponentsInChildren<Button>())
            {
                GameObject.Destroy(button.gameObject);
            }

            AddButton("Back", new Action(() =>
            {
                NPMenuUtils.LWingsMenu.GetComponent<UIPage>().RemovePageFromStack(menuObj.GetComponent<UIPage>());
                menuObj.GetComponent<UIPage>().Show(false, UIPage.TransitionType.InPlace);
                origMenu.GetComponent<UIPage>().Show(true, UIPage.TransitionType.InPlace);
            }));

            menuObj.GetComponent<UIPage>().Name = text + " Menu";

            origMenusMenu.GetComponent<MenuStateController>()._wings = new UnhollowerBaseLib.Il2CppReferenceArray<Wing>(1);
            origMenusMenu.GetComponent<MenuStateController>()._wings[0] = origMenusMenu.GetComponent<Wing>();
            origMenusMenu.GetComponent<MenuStateController>()._uiPages.Add(text, menuObj.GetComponent<UIPage>());
            origMenusMenu.GetComponent<MenuStateController>().enabled = true;

        }

        public void SetAction(Action action)
        {
            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            if (action != null)
                buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(action));
        }

        public void AddButton(string name, Action action, Sprite icon = null)
        {
            var tempButton = GameObject.Instantiate(NPMenuUtils.LWingsSingleButton, menuObj.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));
            tempButton.GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            tempButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(action));
            tempButton.transform.Find("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>().text = name;
            tempButton.name = name + " Button";
            if (icon != null)
            {
                tempButton.transform.Find("Container/Icon").GetComponent<Image>().sprite = icon;
            }
            else
            {
                GameObject.DestroyImmediate(tempButton.transform.Find("Container/Icon").gameObject);
            }
            if (name != "Back")
            {
                GameObject.DestroyImmediate(tempButton.transform.Find("Container/Icon_Arrow").gameObject);
            }
            buttons.Add(name, tempButton);
        }

        public void AddToggleButton(string name, string onText, Action onAction, string offText, Action offAction, Sprite icon = null)
        {
            var toggleButton = new NPWingsToggle(this, name, onText, onAction, offText, offAction, icon);
            toggleButtons.Add(name, toggleButton);
        }

        public GameObject GetGameObject()
        {
            return menuObj;
        }

        public void SetMenuText(string txt = "Testing") => menuText.text = txt;
        public void SetBtnText(string txt = "Testing") => buttonText.text = txt;
        public void SetTextColor(Color col) => buttonText.color = col;
        public void SetBackgroundColor(Color col) => backgroundObj.GetComponent<UnityEngine.UI.Image>().color = col;
    }
}
