using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;

using TMPro;
using UnityEngine.UI;

namespace NPButtonAPI.API
{
    public class NPSingleButton
    {
        public GameObject buttonObj;
        public GameObject backgroundObj;
        public Image buttonIcon;
        public TextMeshProUGUI text;

        public NPSingleButton(NPPageMenu menu, string txt, Action action, string toolTip, Color? backgroundColor = null, Color? textColor = null)
        {
            buttonObj = GameObject.Instantiate(NPMenuUtils.SingleButton, menu.buttonsHolderObj.transform);
            buttonObj.name = NPMenuUtils.SingleButton.transform.parent.name + "-" + NPMenuUtils.SingleButton.name;
            text = buttonObj.transform.Find("Text_H4").GetComponent<TMPro.TextMeshProUGUI>();
            //backgroundObj = buttonObj.transform.Find("/Background").gameObject;
            buttonIcon = buttonObj.transform.Find("Icon").GetComponent<Image>();

            GameObject.Destroy(buttonIcon);

            SetTextLocation(Vector2.zero);
            SetText(txt);
            //SetButtonLocation(Vector2.zero);
        }

        public NPSingleButton(GameObject buttonHolder, string txt, Action action, string toolTip, Color? backgroundColor = null, Color? textColor = null)
        {
            buttonObj = GameObject.Instantiate(NPMenuUtils.SingleButton, buttonHolder.transform);
            buttonObj.name = NPMenuUtils.SingleButton.transform.parent.name + "-" + NPMenuUtils.SingleButton.name;
            text = buttonObj.transform.Find("Text_H4").GetComponent<TMPro.TextMeshProUGUI>();
            //backgroundObj = buttonObj.transform.Find("/Background").gameObject;
            buttonIcon = buttonObj.transform.Find("Icon").GetComponent<Image>();

            GameObject.Destroy(buttonIcon);

            SetTextLocation(Vector2.zero);
            SetText(txt);
            //SetButtonLocation(Vector2.zero);
        }

        public void SetTextColor(Color color)
        {
            text.color = color;
        }

        public void SetText(string txt = "Default")
        {
            text.text = txt;
        }

        public void SetTextLocation(Vector2 loc)
        {
            text.GetComponent<RectTransform>().anchoredPosition = (loc + new Vector2(0, 60));
        }

        public void SetButtonColor(Color newCol)
        {
            backgroundObj.GetComponent<Image>().color = newCol;
        }

        public void Toggle()
        {
            buttonObj.SetActive(!buttonObj.active);
        }

    }
}
