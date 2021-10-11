using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NPButtonAPI.API
{
    public class NPPageMenu
    {

        public GameObject headerObj;
        public GameObject buttonsHolderObj;

        public TextMeshProUGUI menuHeaderText;

        public NPPageMenu(GameObject menu, string name)
        {
            headerObj = GameObject.Instantiate(NPMenuUtils.MenuHeader, menu.transform);
            headerObj.name = "Header_" + name;

            buttonsHolderObj = GameObject.Instantiate(NPMenuUtils.MenuButtonsHolder, menu.transform);
            buttonsHolderObj.name = "Buttons_" + name;

            menuHeaderText = headerObj.transform.Find("LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUI>();

            ClearButtons();

            SetMenuText(name);
        }

        public void ClearButtons()
        {
            foreach (Button button in buttonsHolderObj.GetComponentsInChildren<Button>())
            {
                GameObject.Destroy(button.gameObject);
            }
        }

        public void SetMenuText(string txt)
        {
            menuHeaderText.text = txt;
        }

    }
}
