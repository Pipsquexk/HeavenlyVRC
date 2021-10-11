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

        public enum ButtonParent
        {
            Left,
            Right
        }

        public NPWingsMenu(ButtonParent parent, string text, string toolTip, Color backgroundColor, Color textColor)
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
            menuObj.name = text + "Menu";

            origMenusMenu = origMenu.transform.parent.parent.parent.gameObject;

            menuText = menuObj.transform.Find("WngHeader_H1/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUI>();

            GameObject.DestroyImmediate(buttonObj.transform.Find("Container/Icon").gameObject);
            //GameObject.DestroyImmediate(gameObj.transform.Find("Container/Icon_Arrow").gameObject);

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
            SetTextColor(textColor);
            SetBackgroundColor(backgroundColor);
            //menuObj.SetActive(false);

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

            origMenu.GetComponent<UIPage>().showSequence = opMenu.GetComponent<UIPage>().showSequence;
            menuObj.GetComponent<UIPage>().Name = text + "Menu";

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

        public void AddButton(string name, Action action)
        {
            var tempButton = GameObject.Instantiate(NPMenuUtils.LWingsSingleButton, menuObj.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));
            tempButton.GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            tempButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(action));
            tempButton.transform.Find("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>().text = name;
            buttons.Add(name, tempButton);
        }

        public void SetMenuText(string txt = "Testing") => menuText.text = txt;
        public void SetBtnText(string txt = "Testing") => buttonText.text = txt;
        public void SetTextColor(Color col) => buttonText.color = col;
        public void SetBackgroundColor(Color col) => backgroundObj.GetComponent<UnityEngine.UI.Image>().color = col;
    }
}
