using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using MelonLoader;
using UnityEngine;
using Heavenly.Client;
using Heavenly.Client.API;
using Heavenly.VRChat;
using Heavenly.VRChat.Handlers;
using Newtonsoft.Json;
using System.Collections;
using VRC.UI;
using VRC.SDKBase;
using UnityEngine.UI;
using VRC.Core;
using RubyButtonAPI;
using Transmtn;
using UnhollowerRuntimeLib;

namespace Heavenly
{
    public class Main : MelonMod
    {

        public GameObject heavenlyFavoriteButton, downloadVrcaButton;

        public static AvatarList favList;

        public static AssetBundle notifBundle;

        public Vector3 defaultGravity = Vector3.zero;

        public static KeyConfig kConfig;

        public static NotifConfig nConfig;

        public bool monke = false, welcomed = false, directFly = false, earrape = false;

        public static bool serialize = false;

        public GameObject monkeGO;

        public override async void OnApplicationStart()
        {
            base.OnApplicationStart();
            Console.Clear();
            MelonUtils.SetConsoleTitle($"Heavenly - v{Info.Version}");
            await CU.FirstStartCheck();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                   ,                                 _        ");
            Console.WriteLine("                  /|   |                            | |       ");
            Console.WriteLine("                   |___|  _   __,        _   _  _   | |       ");
            Console.WriteLine("                   |   |\\|/  /  |  |  |_|/  / |/ |  |/  |   | ");
            Console.WriteLine("                   |   |/|__/\\_/|_/ \\/  |__/  |  |_/|__/ \\_/|/");
            Console.WriteLine("                                                           /| ");
            Console.WriteLine("                                                           \\| ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("                                    I\'m back");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"                        ╔─────────────╤───────────────╗");
            Console.WriteLine($"                        │  Ctrl + {kConfig.FlyKey}   ║   Direct Fly  │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + {kConfig.EarrapeKey}   ║  Earrape Mic  │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + {kConfig.RejoinKey}   ║     Rejoin    │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + M   ║   Monke Mode  │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + S   ║   Serialize   │");
            Console.WriteLine($"                        ╚─────────────╧───────────────╝");
            Console.WriteLine($" ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Patches.ApplyPatches();
            defaultGravity = Physics.gravity;
            
        }

        public override async void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            if (buildIndex == -1)
            {
                if (!nConfig.UseNotifs)
                    return;
                if(!welcomed)
                {
                    MelonCoroutines.Start(Welcome());
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();


            #region Keybinds

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(kConfig.FlyKey.ToLower()))
            {
                directFly = !directFly;
                if (directFly)
                {
                    PU.GetVRCPlayer().GetComponent<CharacterController>().enabled = false;
                    Physics.gravity = Vector3.zero;
                }
                else
                {
                    PU.GetVRCPlayer().GetComponent<CharacterController>().enabled = true;
                    Physics.gravity = defaultGravity;
                }
                CU.Log($"Fly: {(directFly ? "ON" : "OFF") }");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(kConfig.EarrapeKey.ToLower()))
            {
                earrape = !earrape;
                if (earrape)
                {
                    PU.GetVRCPlayer().field_Private_USpeaker_0.field_Private_Single_0 = float.MaxValue;
                }
                else
                {
                    PU.GetVRCPlayer().field_Private_USpeaker_0.field_Private_Single_0 = 1;
                }
                CU.Log($"Earrape: {(earrape ? "ON" : "OFF") }");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(kConfig.RejoinKey.ToLower()))
            {
                Networking.GoToRoom($"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId}");
            }
            
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("s"))
            {
                serialize = !serialize;
                CU.Log($"Serialize: {(serialize ? "ON" : "OFF") }");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("a"))
            {
                foreach (GameObject gO in GameObject.FindObjectsOfType<GameObject>())
                {
                    CU.Log(gO.name);
                }
                MelonCoroutines.Start(Intro());
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("m"))
            {
                monke = !monke;
                if (monke)
                {
                    new PageAvatar()
                    {
                        field_Public_SimpleAvatarPedestal_0 = new VRC.SimpleAvatarPedestal()
                        {
                            field_Internal_ApiAvatar_0 = new ApiAvatar()
                            {
                                id = "avtr_537e009c-c05d-4bac-bafe-d0ce6cfaeadb",
                                name = "MONKI",
                                authorId = "usr_8ef380d0-2815-4a55-af1f-a7b476148eda",
                                authorName = "xKomorebi",
                                imageUrl = "https://api.vrchat.cloud/api/1/file/file_8ce59b3c-9448-4cee-9626-fdcf2b7c9159/1/file",
                                thumbnailImageUrl = "https://api.vrchat.cloud/api/1/image/file_8ce59b3c-9448-4cee-9626-fdcf2b7c9159/1/256"

                            }
                        },
                        field_Public_SimpleAvatarPedestal_1 = new VRC.SimpleAvatarPedestal()
                        {
                            field_Internal_ApiAvatar_0 = new ApiAvatar()
                            {
                                id = "avtr_537e009c-c05d-4bac-bafe-d0ce6cfaeadb",
                                name = "MONKI",
                                authorId = "usr_8ef380d0-2815-4a55-af1f-a7b476148eda",
                                authorName = "xKomorebi",
                                imageUrl = "https://api.vrchat.cloud/api/1/file/file_8ce59b3c-9448-4cee-9626-fdcf2b7c9159/1/file",
                                thumbnailImageUrl = "https://api.vrchat.cloud/api/1/image/file_8ce59b3c-9448-4cee-9626-fdcf2b7c9159/1/256"

                            }
                        }
                    }.ChangeToSelectedAvatar();

                    monkeGO = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>("Monke.prefab"), PU.GetVRCPlayer().transform);
                }
                else
                {
                    GameObject.Destroy(monkeGO);
                }



                CU.Log(monke ? "Monke" : "No Monke");

            }

            #endregion

            #region Bool Toggles

            if (directFly)
            {
                var cam = Camera.main;
                if (Input.GetAxis("Horizontal") != 0)
                {
                    PU.GetVRCPlayer().transform.position += cam.transform.right * (Input.GetAxis("Horizontal") * 13) * Time.deltaTime;
                }

                if (Input.GetAxis("Vertical") != 0)
                {
                    PU.GetVRCPlayer().transform.position += cam.transform.forward * (Input.GetAxis("Vertical") * 13) * Time.deltaTime;
                }
            }

            #endregion

        }

        public IEnumerator Intro()
        {
            var welcomeGO = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"{nConfig.Voice}W.prefab"), Camera.main.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3.5f);
            GameObject.Destroy(welcomeGO);
        }

        public IEnumerator Welcome()
        {
            while (PU.GetVRCPlayer() == null)
            {
                yield return null;
            }
            NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<VRC.Player>(NotificationHandler.JoinNotify));
            NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<VRC.Player>(NotificationHandler.LeaveNotify));
            
            
            
            VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0.field_Private_Api_0.PostOffice.add_OnNotification(DelegateSupport.ConvertDelegate<Il2CppSystem.EventHandler<NotificationEvent>>(new Action<Il2CppSystem.Object, NotificationEvent>(NotificationHandler.NotificationNotify)));
            
            
            
            var welcomeGO = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"{nConfig.Voice}W.prefab"), Camera.main.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3.5f);
            GameObject.Destroy(welcomeGO);
            welcomed = true;
            favList = new AvatarList("Heavenly Favorites");
            if (File.Exists("Heavenly\\HeavenlyFavorites.txt"))
            {
                foreach (HevApiAvatar avi in JsonConvert.DeserializeObject<List<HevApiAvatar>>(File.ReadAllText("Heavenly\\HeavenlyFavorites.txt")))
                {
                    favList.UpdateAvatarList(new ApiAvatar() { id = avi.id, authorId = avi.authorId, name = avi.name, authorName = avi.authorName, imageUrl = avi.imageUrl, thumbnailImageUrl = avi.thumbnailImageUrl });
                }
            }
            else
            {
                var newAvi = new HevApiAvatar(PU.GetVRCPlayer().field_Private_ApiAvatar_0.name, PU.GetVRCPlayer().field_Private_ApiAvatar_0.id, PU.GetVRCPlayer().field_Private_ApiAvatar_0.authorId, PU.GetVRCPlayer().field_Private_ApiAvatar_0.authorName, PU.GetVRCPlayer().field_Private_ApiAvatar_0.imageUrl, PU.GetVRCPlayer().field_Private_ApiAvatar_0.thumbnailImageUrl);
                File.WriteAllText("Heavenly\\HeavenlyFavorites.txt", JsonConvert.SerializeObject(new List<HevApiAvatar>() { newAvi }));
            }

            heavenlyFavoriteButton = GameObject.Instantiate(ButtonHandler.GetChangeAvatarButton(), ButtonHandler.GetChangeAvatarButton().transform.parent);
            downloadVrcaButton = GameObject.Instantiate(ButtonHandler.GetChangeAvatarButton(), ButtonHandler.GetChangeAvatarButton().transform.parent);

            ButtonHandler.GetOriginalAvatarFavoriteButton().GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 25);
            ButtonHandler.GetOriginalAvatarFavoriteButton().GetComponent<RectTransform>().localScale -= new Vector3(0, 0.1f, 0);

            heavenlyFavoriteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 55);
            heavenlyFavoriteButton.GetComponent<RectTransform>().localScale -= new Vector3(0, 0.1f, 0);
            heavenlyFavoriteButton.GetComponentInChildren<Text>().text = "Heavenly (Un)Favorite";

            ButtonHandler.SetButtonColor(heavenlyFavoriteButton, Color.red);
            heavenlyFavoriteButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            heavenlyFavoriteButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() => { favList.AddOrRemoveAvatar(GameObject.FindObjectOfType<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0); }));

            downloadVrcaButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(310, 685);
            downloadVrcaButton.GetComponent<RectTransform>().localScale -= new Vector3(0.1f, 0.1f, 0);
            downloadVrcaButton.GetComponentInChildren<Text>().text = "Download";

            ButtonHandler.SetButtonColor(downloadVrcaButton, Color.magenta);
            downloadVrcaButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            downloadVrcaButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() => { PU.DownloadAvatar(GameObject.FindObjectOfType<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0); }));

            var fuck = GameObject.Instantiate(ButtonHandler.GetQuickMenuNotifTab(), ButtonHandler.GetQuickMenuNotifTab().transform.parent);

            //foreach (Component com in fuck.GetComponentsInChildren<Component>())
            //{
            //    CU.Log(com.ToString());
            //}
        }
    }
}
