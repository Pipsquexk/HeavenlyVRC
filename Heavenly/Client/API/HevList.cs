using Heavenly.Client.Utilities;
using Heavenly.VRChat.Handlers;
using Heavenly.VRChat.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Heavenly.Client.API
{
    public class HevList
    {
        protected int entryCount = 0;
        public GameObject originPanelPrefab = Main.otherBundle.LoadAsset<GameObject>("Panel.prefab");
        public GameObject originEntryPrefab = Main.otherBundle.LoadAsset<GameObject>("Entry.prefab");
        protected GameObject gameObject;
        protected Transform entryList;
        protected Material debugMat = ButtonHandler.GetAvatarButton().GetComponentInChildren<Image>().material;

        public HevList(string menu, string name)
        {
            gameObject = GameObject.Instantiate(originPanelPrefab, UIU.GetQuickMenu().transform.Find(menu));

            gameObject.transform.localPosition = Vector3.zero;

            gameObject.transform.localPosition += new Vector3(0, (gameObject.transform.localScale.y / 2) + 625, -1);

            gameObject.GetComponent<Image>().material = debugMat;

            entryList = gameObject.transform.Find("Entries");
            entryList.gameObject.GetComponent<Image>().material = debugMat;

        }

        public void AddEntry(string text)
        {
            if(originEntryPrefab == null || entryList == null)
            {
                CU.Log("originEntryPrefab " + originEntryPrefab == null ? "NULL" : "NOT NULL");
                CU.Log("entryList " + entryList == null ? "NULL" : "NOT NULL");
            }

            var newEntry = GameObject.Instantiate<GameObject>(originEntryPrefab, entryList);
            newEntry.GetComponent<Image>().material = debugMat;

            newEntry.transform.Find("MainText").GetComponent<Text>().text = $"[<color=lime>{DateTime.Now.ToString("HH:mm:ss")}</color>] {text}";
            newEntry.transform.Find("MainText").GetComponent<Text>().material = debugMat;

            entryCount++;

            if (entryCount > 22)
            {
                GameObject.Destroy(entryList.GetChild(0).gameObject);
            }
        }
    }
}
