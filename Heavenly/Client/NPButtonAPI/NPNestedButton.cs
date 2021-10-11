using Harmony;
using NPButtonAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.UI.Elements;

namespace NPButtonAPI.API
{

    public class NPNestedButton
    {
        public NPSingleButton singleButton;

        public GameObject origMenu;

        public GameObject menuObj;


        public NPNestedButton(NPPageMenu menu, string txt, string toolTip, Color? backgroundColor = null, Color? textColor = null)
        {

            

            singleButton = new NPSingleButton(menu, txt, null, toolTip);
            menuObj = GameObject.Instantiate(NPMenuUtils.AudioSettingsPanel, NPMenuUtils.AudioSettingsPanel.transform.parent);
            menuObj.name = txt + "Menu";
            menuObj.GetComponent<UIPage>().Name = txt;
            NPMenuUtils.QuickMenu.GetComponent<MenuStateController>()._uiPages.Add(txt, menuObj.GetComponent<UIPage>());
            NPMenuUtils.QuickMenu.GetComponent<MenuStateController>().menuRootPages.Add(menuObj.GetComponent<UIPage>());
            //SetButtonLocation(Vector2.zero);
        }

        public NPNestedButton(GameObject buttonHolder, string txt, string toolTip, Color? backgroundColor = null, Color? textColor = null)
        {
            singleButton = new NPSingleButton(buttonHolder, txt, null, toolTip);
            menuObj = GameObject.Instantiate(NPMenuUtils.AudioSettingsPanel, NPMenuUtils.AudioSettingsPanel.transform.parent);
            menuObj.name = txt + "Menu";
            menuObj.GetComponent<UIPage>().Name = txt;
            NPMenuUtils.QuickMenu.GetComponent<MenuStateController>()._uiPages.Add(txt, menuObj.GetComponent<UIPage>());
            NPMenuUtils.QuickMenu.GetComponent<MenuStateController>().menuRootPages.Add(menuObj.GetComponent<UIPage>());
            //SetButtonLocation(Vector2.zero);
        }

    }
}
