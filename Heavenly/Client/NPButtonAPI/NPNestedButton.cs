using NPButtonAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;

namespace NPButtonAPI.API
{

    public class NPNestedButton
    {
        public NPSingleButton singleButton;

        public GameObject origMenu;

        public GameObject menuObj;


        public NPNestedButton(NPPageMenu menu, string txt, string toolTip, Color? backgroundColor = null, Color? textColor = null)
        {
            origMenu = NPMenuUtils.DashboardMenuPanel;
            menuObj = GameObject.Instantiate(NPMenuUtils.AudioSettingsPanel, NPMenuUtils.AudioSettingsPanel.transform.parent);
            menuObj.name = txt + "Menu";
            menuObj.GetComponent<UIPage>().Name = txt;
            NPMenuUtils.QuickMenu.GetComponent<MenuStateController>().menuRootPages.Append(menuObj.GetComponent<UIPage>());
            //menuObj.GetComponent<UIPage>().showSequence = origMenu.GetComponent<UIPage>().showSequence;

            singleButton = new NPSingleButton(menu, txt, delegate {
                menuObj.SetActive(true);
                origMenu.SetActive(false);
            }, toolTip);
            //SetButtonLocation(Vector2.zero);
        }

        public NPNestedButton(GameObject buttonHolder, string txt, string toolTip, Color? backgroundColor = null, Color? textColor = null)
        {
            origMenu = NPMenuUtils.DashboardMenuPanel;
            menuObj = GameObject.Instantiate(NPMenuUtils.AudioSettingsPanel, NPMenuUtils.AudioSettingsPanel.transform.parent);
            menuObj.name = txt + "Menu";
            menuObj.GetComponent<UIPage>().Name = txt;
            NPMenuUtils.QuickMenu.GetComponent<MenuStateController>().menuRootPages.Append(menuObj.GetComponent<UIPage>());
            //menuObj.GetComponent<UIPage>().showSequence = origMenu.GetComponent<UIPage>().showSequence;

            singleButton = new NPSingleButton(buttonHolder, txt, delegate {
                menuObj.SetActive(true);
                origMenu.SetActive(false);
            }, toolTip);
            //SetButtonLocation(Vector2.zero);
        }

    }
}
