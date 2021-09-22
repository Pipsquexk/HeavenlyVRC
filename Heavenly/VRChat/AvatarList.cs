using Heavenly.Client;
using Heavenly.Client.API;
using Heavenly.VRChat.Utilities;
using MelonLoader;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace Heavenly.VRChat
{
    public class AvatarList
    {

        public GameObject gameObject;
        public GameObject originObject = GameObject.Find("/UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/Content/Public Avatar List");

        public UiAvatarList vrcAvatarList;

        public Text text;

        public string name = "NULL";

        public List<HevApiAvatar> hAvatars = new List<HevApiAvatar>();

        public Il2CppSystem.Collections.Generic.List<ApiAvatar> avatars = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();

        public AvatarList(string name)
        {
            gameObject = GameObject.Instantiate(originObject, originObject.transform.parent);
            text = gameObject.transform.Find("Button").GetComponentInChildren<Text>();
            vrcAvatarList = gameObject.GetComponent<UiAvatarList>();
            gameObject.transform.SetSiblingIndex(0);

            vrcAvatarList.clearUnseenListOnCollapse = false;
            vrcAvatarList.isOffScreen = false;
            vrcAvatarList.field_Public_EnumNPublicSealedvaInPuMiFaSpClPuLi11Unique_0 = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLi11Unique.SpecificList;
            gameObject.SetActive(true);
            text.text = name;
            gameObject.name = name + " List";
            this.name = name;
        }

        public void RefreshList()
        {
            vrcAvatarList.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0<ApiAvatar>(avatars);
        }

        public void UpdateAvatarFavList(ApiAvatar avatar)
        {

            vrcAvatarList.isOffScreen = false;
            vrcAvatarList.enabled = true;

            avatars.Insert(0, avatar);

            hAvatars.Insert(0, new HevApiAvatar(avatar.name, avatar.id, avatar.authorId, avatar.authorName, avatar.thumbnailImageUrl, avatar.assetUrl));

            vrcAvatarList.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0<ApiAvatar>(avatars);

            text.text = $"{name} - {avatars.Count}";

        }

        public IEnumerator AddSearchAvatars(List<HevApiAvatar> hevAvatars)
        {
            vrcAvatarList.isOffScreen = false;
            vrcAvatarList.enabled = true;

            Il2CppSystem.Collections.Generic.List<ApiAvatar> resAvis = new Il2CppSystem.Collections.Generic.List<ApiAvatar>();

            foreach (HevApiAvatar avi in hevAvatars)
            {
                resAvis.Add(avi.ToApiAvatar());
                yield return null;
            }

            vrcAvatarList.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0<ApiAvatar>(resAvis);

            text.text = $"{name} - {resAvis.Count} Results";

        }

        public void AddOrRemoveFavAvatar(ApiAvatar avatar)
        {
            vrcAvatarList.isOffScreen = false;
            vrcAvatarList.enabled = true;

            var favTxt = File.ReadAllText("Heavenly\\HeavenlyFavorites.txt");

            if (favTxt.Contains(avatar.id))
            {
                foreach (ApiAvatar av in avatars.ToArray())
                {
                    if (avatar.id == av.id)
                    {
                        avatars.Remove(avatar);
                    }
                }
                hAvatars.RemoveAll(x => x.id == avatar.id);
                var revisedList = JsonConvert.SerializeObject(hAvatars);
                File.WriteAllText("Heavenly\\HeavenlyFavorites.txt", revisedList);
                vrcAvatarList.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0<ApiAvatar>(avatars);
                text.text = $"{name} - {avatars.Count}";
                return;
            }

            avatars.Insert(0, avatar);

            hAvatars = JsonConvert.DeserializeObject<List<HevApiAvatar>>(favTxt);
            hAvatars.Insert(0, new HevApiAvatar(avatar.name, avatar.id, avatar.authorId, avatar.authorName, avatar.thumbnailImageUrl, avatar.assetUrl));
            var apiList = JsonConvert.SerializeObject(hAvatars);

            File.WriteAllText("Heavenly\\HeavenlyFavorites.txt", apiList);

            vrcAvatarList.Method_Protected_Void_List_1_T_Int32_Boolean_VRCUiContentButton_0<ApiAvatar>(avatars);

            text.text = $"{name} - {avatars.Count}";

        }

    }
}
