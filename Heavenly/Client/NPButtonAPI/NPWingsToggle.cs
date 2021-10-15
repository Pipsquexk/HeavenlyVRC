using NPButtonAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NPButtonAPI.API
{
    public class NPWingsToggle
    {

        public GameObject buttonObj;
        public TextMeshProUGUI text;

        public bool state = false;

        public Action onAction;
        public Action offAction;

        public string onText;
        public string offText;

        public NPWingsToggle(NPWingsMenu menu, string name, string onText, Action onAction, string offText, Action offAction, Sprite icon = null)
        {
            buttonObj = GameObject.Instantiate(NPMenuUtils.LWingsSingleButton, menu.menuObj.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup"));
            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            this.onAction = onAction;
            this.offAction = offAction;
            this.onText = onText;
            this.offText = offText;
            buttonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(new Action(() => 
            {
                state = !state;
                SetToggleState(state, true);
            })));


            text = buttonObj.transform.Find("Container/Text_QM_H3").GetComponent<TextMeshProUGUI>();
            buttonObj.name = name + " Button";
            if (icon != null)
            {
                buttonObj.transform.Find("Container/Icon").GetComponent<Image>().sprite = icon;
            }
            else
            {
                GameObject.DestroyImmediate(buttonObj.transform.Find("Container/Icon").gameObject);
            }
            if (name != "Back")
            {
                GameObject.DestroyImmediate(buttonObj.transform.Find("Container/Icon_Arrow").gameObject);
            }
            menu.buttons.Add(name, buttonObj);
        }

        public void SetToggleState(bool state, bool shouldInvoke)
        {
            if (state)
            {
                SetTextColor(Color.white);
                text.text = onText;
                if (shouldInvoke)
                {
                    onAction();
                }

            }
            else
            {
                SetTextColor(Color.grey);
                text.text = offText;
                if (shouldInvoke)
                {
                    offAction();
                }
            }
        }

        public void SetTextColor(Color color)
        {
            text.color = color; 
        }
    }
}
