using Heavenly.Client;
using Heavenly.Client.API;
using Heavenly.Client.Utilities;
using Heavenly.VRChat;
using Heavenly.VRChat.Handlers;
using Heavenly.VRChat.Utilities;
using MelonLoader;
using Newtonsoft.Json;
using RubyButtonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Transmtn;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.SDK3.Video.Components.Base;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using VRC.UI;

namespace Heavenly
{
    public class Main : MelonMod
    {
        public static QMNestedButton mainMenu;

        public static QMSingleButton searchBotButton, forceCloneIdButton, restartButton, restartRejoinButton;
        public static QMToggleButton flyToggleButton, espToggleButton;

        public Vector3 serPos;
        public Quaternion serRot;

        public static GameObject heavenlyFavoriteButton, downloadVrcaButton, searchAvatarsButton;

        public static AvatarList favList;
        public static AvatarList searchList;

        public static AssetBundle notifBundle;

        public static Vector3 defaultGravity = Vector3.zero;

        public static KeyConfig kConfig;

        public static NotifConfig nConfig;

        public static bool monke = false, welcomed = false, directFly = false, earrape = false, sittingOnHead = false;

        public static bool serialize = false;

        public GameObject monkeGO;

        public static Player selectedSit;

        public override void OnApplicationStart()
        {
            Console.Clear();
            MelonUtils.SetConsoleTitle($"Heavenly - v1.0");
            CU.FirstStartCheck();
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
            Console.WriteLine($"                        │  Ctrl + {Main.kConfig.FlyKey}   ║   Direct Fly  │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + {Main.kConfig.EarrapeKey}   ║  Earrape Mic  │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + {Main.kConfig.RejoinKey}   ║     Rejoin    │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + M   ║   Monke Mode  │");
            Console.WriteLine($"                        ╞─────────────╬───────────────╡");
            Console.WriteLine($"                        │  Ctrl + S   ║   Serialize   │");
            Console.WriteLine($"                        ╚─────────────╧───────────────╝");
            Console.WriteLine($" ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Patches.ApplyPatches();
            Main.defaultGravity = Physics.gravity;
            WebsocketHandler.ConnectToWebsockets();
            MelonCoroutines.Start(Main.Welcome());
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);
            if (buildIndex == -1)
            {
                if (!nConfig.UseNotifs)
                    return;
                if (!welcomed)
                {
                    MelonCoroutines.Start(Intro());
                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();


            #region Keybinds

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(kConfig.FlyKey.ToLower()))
            {
                flyToggleButton.setToggleState(!directFly, true);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("u"))
            {
                if (PU.GetSelectedPlayer() != null)
                {
                    selectedSit = PU.GetSelectedPlayer();
                }
                ToggleSitOnHead();
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("h"))
            {
                RU.ReuploadAvatar(PU.GetSelectedPlayer().prop_ApiAvatar_0);
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

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("l"))
            {
                UIU.GetQuickMenu().Method_Public_Void_Player_0(PU.GetPlayer());
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("s"))
            {
                serialize = !serialize;
                if (serialize)
                {
                    serPos = PU.GetVRCPlayer().transform.position;
                    serRot = PU.GetVRCPlayer().transform.rotation;
                }
                else
                {
                    PU.GetVRCPlayer().transform.position = serPos;
                    PU.GetVRCPlayer().transform.rotation = serRot;
                }
                CU.Log($"Serialize: {(serialize ? "ON" : "OFF") }");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("t"))
            {
                if (PU.GetSelectedPlayer() == null)
                    return;

                PU.RequestToTagAlong(PU.GetSelectedPlayer());

                //UIU.OpenVRCUIPopup("Tag Along Request", $"Pip would like to tag along with you!", "Accept",
                //    new Action(() =>
                //    {
                //        WebsocketHandler.tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}:{PU.GetPlayer().field_Private_APIUser_0.id}:{PU.GetPlayer().field_Private_APIUser_0.id}:response:null");
                //    }), "Decline");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("j"))
            {
                Console.Write("Enter URL: ");
                var url = Console.ReadLine();
                foreach (BaseVRCVideoPlayer vRCVideoPlayer in GameObject.FindObjectsOfType<BaseVRCVideoPlayer>())
                {
                    Networking.SetOwner(PU.GetVRCPlayerApi(), vRCVideoPlayer.gameObject);
                    vRCVideoPlayer.EnableAutomaticResync = false;
                    CU.Log("[BaseVRCVideoPlayer] Disabled automatic resync");
                    vRCVideoPlayer.SetTime(0);
                    CU.Log("[BaseVRCVideoPlayer] Set time to 0");
                    vRCVideoPlayer.SetIndexMarker(0);
                    CU.Log("[BaseVRCVideoPlayer] Set index marker to 0");
                    vRCVideoPlayer.Stop();
                    CU.Log("[BaseVRCVideoPlayer] Stopped");
                    vRCVideoPlayer.LoadURL(new VRCUrl(url));
                    CU.Log($"[BaseVRCVideoPlayer] Loaded URL: {url}");
                    vRCVideoPlayer.PlayURL(new VRCUrl(url));
                    CU.Log($"[BaseVRCVideoPlayer] Play URL: {url}");
                    vRCVideoPlayer.Play();
                    CU.Log($"[BaseVRCVideoPlayer] Play");
                    foreach (IUdonBehaviour beh in vRCVideoPlayer._udonBehaviours)
                    {
                        beh.InitializeUdonContent();
                        beh.RunEvent("Fuck you", null);
                        beh.RunProgram("Fuck you");
                        beh.InitializeUdonContent();
                        beh.InitializeUdonContent();
                    }
                }

                foreach (GameObject go in VRC_SceneDescriptor.Instance.DynamicPrefabs)
                {
                    Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, go.name, new Vector3(-100, -100, -100), Quaternion.identity);
                    CU.Log(go.name);
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("p"))
            {
                foreach (BaseVRCVideoPlayer vRCVideoPlayer in GameObject.FindObjectsOfType<BaseVRCVideoPlayer>())
                {
                    Networking.SetOwner(PU.GetVRCPlayerApi(), vRCVideoPlayer.gameObject);
                    vRCVideoPlayer.Play();
                    CU.Log($"[BaseVRCVideoPlayer] Play");
                }

                var outNumber = "NULL";
                foreach (UdonBehaviour beh in GameObject.FindObjectsOfType<UdonBehaviour>())
                {
                    beh.publicVariables.TryGetVariableValue("solution", out outNumber);
                    if (outNumber != null)
                    {
                        Console.WriteLine(outNumber);
                    }
                }
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
            while (PU.GetVRCPlayer() == null)
            {
                yield return null;
            }
            var welcomeGO = GameObject.Instantiate(Main.notifBundle.LoadAsset<GameObject>($"{nConfig.Voice}W.prefab"), Camera.main.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3.5f);
            GameObject.Destroy(welcomeGO);
        }

        public static IEnumerator Welcome()
        {

            while (PU.GetVRCPlayer() == null)
            {
                yield return null;
            }
            NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<VRC.Player>(NotificationHandler.JoinNotify));
            NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<VRC.Player>(NotificationHandler.LeaveNotify));



            VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0.field_Private_Api_0.PostOffice.add_OnNotification(DelegateSupport.ConvertDelegate<Il2CppSystem.EventHandler<NotificationEvent>>(new Action<Il2CppSystem.Object, NotificationEvent>(NotificationHandler.NotificationNotify)));



            welcomed = true;
            favList = new AvatarList("Heavenly Favorites");
            searchList = new AvatarList("Heavenly Search");
            if (File.Exists("Heavenly\\HeavenlyFavorites.txt"))
            {
                foreach (HevApiAvatar avi in JsonConvert.DeserializeObject<List<HevApiAvatar>>(File.ReadAllText("Heavenly\\HeavenlyFavorites.txt")))
                {
                    favList.UpdateAvatarFavList(avi.ToApiAvatar());
                    yield return null;
                }
            }
            else
            {
                var newAvi = new HevApiAvatar(PU.GetVRCPlayer().field_Private_ApiAvatar_0.name, PU.GetVRCPlayer().field_Private_ApiAvatar_0.id, PU.GetVRCPlayer().field_Private_ApiAvatar_0.authorId, PU.GetVRCPlayer().field_Private_ApiAvatar_0.authorName, PU.GetVRCPlayer().field_Private_ApiAvatar_0.imageUrl, PU.GetVRCPlayer().field_Private_ApiAvatar_0.thumbnailImageUrl);
                File.WriteAllText("Heavenly\\HeavenlyFavorites.txt", JsonConvert.SerializeObject(new List<HevApiAvatar>() { newAvi }));
            }

            ButtonHandler.GetUIBackground().GetComponentInChildren<RectTransform>().localScale += new Vector3(0.15f, 0, 0);
            ButtonHandler.GetUIBackground().GetComponentInChildren<RectTransform>().anchoredPosition -= new Vector2(105f, 0);

            heavenlyFavoriteButton = GameObject.Instantiate(ButtonHandler.GetChangeAvatarButton(), ButtonHandler.GetChangeAvatarButton().transform.parent);
            downloadVrcaButton = GameObject.Instantiate(ButtonHandler.GetChangeAvatarButton(), ButtonHandler.GetChangeAvatarButton().transform.parent);
            searchAvatarsButton = GameObject.Instantiate(ButtonHandler.GetChangeAvatarButton(), ButtonHandler.GetChangeAvatarButton().transform.parent);

            ButtonHandler.GetOriginalAvatarFavoriteButton().GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 25);
            ButtonHandler.GetOriginalAvatarFavoriteButton().GetComponent<RectTransform>().localScale -= new Vector3(0, 0.1f, 0);

            heavenlyFavoriteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 55);
            heavenlyFavoriteButton.GetComponent<RectTransform>().localScale -= new Vector3(0, 0.1f, 0);
            heavenlyFavoriteButton.GetComponentInChildren<Text>().text = "Heavenly (Un)Favorite";

            ButtonHandler.SetButtonColor(heavenlyFavoriteButton, Color.red);
            heavenlyFavoriteButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            heavenlyFavoriteButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() => { favList.AddOrRemoveFavAvatar(GameObject.FindObjectOfType<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0); }));

            downloadVrcaButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(285, 162.5f);
            downloadVrcaButton.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.1f, 0);
            downloadVrcaButton.GetComponentInChildren<Text>().text = "Download vrca";

            ButtonHandler.SetButtonColor(downloadVrcaButton, Color.red);
            downloadVrcaButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            downloadVrcaButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() => { PU.DownloadAvatar(GameObject.FindObjectOfType<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0); }));

            searchAvatarsButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(1125, 685);
            searchAvatarsButton.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.1f, 0);
            searchAvatarsButton.GetComponentInChildren<Text>().text = "Search";


            ButtonHandler.SetButtonColor(searchAvatarsButton, Color.red);
            searchAvatarsButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();

            searchAvatarsButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
            {
                UIU.OpenKeyboardPopup("Search Avatars", "Enter Author/Avatar name.....", (str, LK, tex) =>
                {
                    MelonCoroutines.Start(searchList.AddSearchAvatars(CU.SearchAvatars(str)));
                });
            }));


            ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();

            ButtonHandler.GetCloneAvatarButton().GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
            {
                PU.ForceClone(UIU.GetQuickMenu().field_Private_Player_0.prop_ApiAvatar_0);
            }));

            foreach (Button button in ButtonHandler.GetShortcutMenu().GetComponentsInChildren<Button>())
            {
                ButtonHandler.SetButtonColor(button.gameObject, Color.red);
                button.GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HexButton.png");
                yield return null;
            }

            foreach (Button button in ButtonHandler.GetUserInteractMenu().GetComponentsInChildren<Button>())
            {
                ButtonHandler.SetButtonColor(button.gameObject, Color.red);
                yield return null;
            }

            ButtonHandler.SetButtonColor(QMStuff.ToggleButtonTemplate(), Color.red);
            ButtonHandler.GetBlockButton().GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HexButton.png");
            ButtonHandler.GetBlockButtonOFF().GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HexToggleOFF.png");
            ButtonHandler.GetBlockButtonON().GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HexToggleON.png");
            ButtonHandler.GetBlockButtonOFF().GetComponentInChildren<Image>().color = Color.red;
            ButtonHandler.GetBlockButtonON().GetComponentInChildren<Image>().color = Color.red;

            //var fuck = GameObject.Instantiate(ButtonHandler.GetQuickMenuNotifTab(), ButtonHandler.GetQuickMenuNotifTab().transform.parent);
            mainMenu = new QMNestedButton("ShortcutMenu", 2.50f, 0.86f, "", "Main Menu", Color.red);
            mainMenu.getMainButton().getGameObject().GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HeavenlyButton.png");
            searchBotButton = new QMSingleButton(mainMenu, 1, 0, "Search For\nPlayer", delegate { }, "Search for a player in public worlds", Color.red);

            forceCloneIdButton = new QMSingleButton(mainMenu, 2, 0, "Force Clone\nby ID", delegate
            {
                UIU.OpenKeyboardPopup("Force Clone ID", "Enter Avatar ID.....", (str, LK, tex) =>
                {
                    PU.ForceCloneByID(str);
                });
            }, "Force clone an avatar by the avatar ID", Color.red);

            restartButton = new QMSingleButton(mainMenu, 3, 0, "Restart Game", delegate { CU.RestartGame(); }, "Restart VRChat", Color.red);
            restartRejoinButton = new QMSingleButton(mainMenu, 4, 0, "Restart and\n Rejoin Game", delegate { CU.RestartRejoinGame(); }, "Restart VRChat and rejoin your current lobby", Color.red);

            flyToggleButton = new QMToggleButton("ShortcutMenu", 0.25f, 0.4225f, "Fly ON", delegate
            {
                directFly = true;
                PU.GetVRCPlayer().GetComponent<CharacterController>().enabled = false;
                Physics.gravity = Vector3.zero;
            },
            "Fly OFF", delegate
            {
                PU.GetVRCPlayer().GetComponent<CharacterController>().enabled = true;
                Physics.gravity = defaultGravity;
                directFly = false;
            },
            "Restart VRChat and rejoin your current lobby", Color.red);

            espToggleButton = new QMToggleButton("ShortcutMenu", 0.25f, 1.275f, "ESP ON", delegate
            {

            },
            "ESP OFF", delegate
            {

            },
            "Restart VRChat and rejoin your current lobby", Color.red);


            ButtonHandler.GetGoHomeButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 60);
            ButtonHandler.GetUIElementsButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 120);
            ButtonHandler.GetAvatarButton().GetComponent<RectTransform>().anchoredPosition -= new Vector2(105, 180);
            ButtonHandler.GetRespawnButton().GetComponent<RectTransform>().anchoredPosition -= new Vector2(105, 120);
            ButtonHandler.GetCameraButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(210, 120);
            ButtonHandler.GetSocialButton().GetComponent<RectTransform>().anchoredPosition -= new Vector2(210, 0);
            ButtonHandler.GetEmoteButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(105, 300);
            ButtonHandler.GetEmojiButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 120);
            ButtonHandler.GetSettingsButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 60);
            ButtonHandler.GetReportWorldButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(-735, 240);
            ButtonHandler.GetQuickMenuBackground().GetComponent<RectTransform>().localScale += new Vector3(0.5f, 0, 0);

            ButtonHandler.GetGalleryButton().GetComponent<RectTransform>().anchoredPosition -= new Vector2(105, 180);
            ButtonHandler.GetLearnMoreButton().GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HexHighlightButton.png");
            ButtonHandler.SetButtonColor(ButtonHandler.GetLearnMoreButton(), Color.red);

            ButtonHandler.GetSitButton().GetComponent<RectTransform>().anchoredPosition += new Vector2(735, -120);
            ButtonHandler.GetSitOFFText().transform.localPosition -= new Vector3(37.5f, 0, 0);
            ButtonHandler.GetSitONText().transform.localPosition -= new Vector3(37.5f, 0, 0);
            ButtonHandler.GetSitOFF().GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HexToggleOFF.png");
            ButtonHandler.GetSitON().GetComponentInChildren<Image>().sprite = notifBundle.LoadAsset<Sprite>("HexToggleON.png");
            ButtonHandler.GetSitOFF().GetComponentInChildren<Image>().color = Color.red;
            ButtonHandler.GetSitON().GetComponentInChildren<Image>().color = Color.red;
            GameObject.Destroy(ButtonHandler.GetSitOFFImage());
            GameObject.Destroy(ButtonHandler.GetSitONImage());

            ButtonHandler.GetMicControls().GetComponent<RectTransform>().anchoredPosition -= new Vector2(200, 0);

            GameObject.Destroy(ButtonHandler.GetGalleryButtonVRCTag());

            VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Public_VRCUiCursor_0.field_Public_Color_0 = Color.red / 1.4f;
            VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Public_VRCUiCursor_0.field_Public_Color_1 = Color.red / 1.4f;
            VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.field_Public_VRCUiCursor_0.field_Public_Color_2 = Color.red / 1.4f;

        }

        public static void ToggleSitOnHead()
        {
            sittingOnHead = !sittingOnHead;
            MelonCoroutines.Start(HeadSit());
        }

        public static IEnumerator HeadSit()
        {
            while (sittingOnHead)
            {
                if (selectedSit == null)
                    yield break;

                PU.GetVRCPlayer().transform.position = selectedSit.field_Private_VRCPlayerApi_0.GetBonePosition(UnityEngine.HumanBodyBones.Head) + new Vector3(0, 0.1f, 0);
                yield return null;
            }
        }
    }
}
