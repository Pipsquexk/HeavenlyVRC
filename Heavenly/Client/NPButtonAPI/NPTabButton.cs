using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

namespace NPButtonAPI.API
{
    public class NPTabButton
    {
        public GameObject gameObj;
        public Image backgroundImg;

        public NPTabButton(Action action, string toolTip, Color? backgroundColor = null)
        {
            gameObj = GameObject.Instantiate(NPMenuUtils.TabButton, NPMenuUtils.TabButton.transform.parent);
            gameObj.name = NPButtonAPI.Identifier + "-" + gameObj.transform.name;
            GameObject.DestroyImmediate(gameObj.GetComponent<VRC.UI.Elements.Controls.MenuTab>());

            SetAction(action);
            SetTooltip(toolTip);
        }
        public void SetAction(Action action)
        {
            gameObj.GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            if (action != null)
                gameObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>(action));
        }
        //public void SetSprite(string Path)
        //{
        //    if (File.Exists(Path))
        //        backgroundImg.sprite = NPMenuUtils.LoadSpriteFromDisk(Path);
        //}
        public void SetTooltip(string toolTip = "Testing")
        {
            gameObj.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().text = toolTip;
            gameObj.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().alternateText = toolTip;
        }
    }
}
