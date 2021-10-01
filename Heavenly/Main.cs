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
using VRC.UI;

namespace Heavenly
{
    public class Main : MelonMod
    {

        public static HighlightsFXStandalone friendsFX, trustedFX, knownFX, userFX, newUserFX, visitorFX;

        public static QMNestedButton mainMenu, gameMenu, photonMenu, avatarMenu, voiceMenu, sitMenu, espMenu, tagAlongMenu;
        public static QMNestedButton selectedMenu, selectedAvatarMenu, selectedTagAlongMenu;

        public static QMSingleButton searchBotButton, forceCloneIdButton, restartButton, restartRejoinButton, cancelTagAlongButton;
        public static QMSingleButton selectedDownloadVrcaButton, selectedTagAlongButton;

        public static List<QMSingleButton> sitOptionButtons = new List<QMSingleButton>();
        public static QMSingleButton headSitOptionButton, torsoSitOptionButton, lHandSitOptionButton, lFootSitOptionButton, rHandSitOptionButton, rFootSitOptionButton;
        public static UnityEngine.HumanBodyBones sitBone = HumanBodyBones.Head;

        public static QMToggleButton flyToggleButton, espToggleButton, serializeToggleButton, serializePosToggleButton, playerESPToggleButton, itemESPToggleButton, triggerESPToggleButton;

        public static VRCSlider voiceGainSlider, othersVoiceGainSlider;

        public static Vector3 serPos;
        public static Quaternion serRot;

        public static GameObject heavenlyFavoriteButton, avatarDownloadVrcaButton, searchAvatarsButton, socialReqTagAlongButton;

        public static AvatarList favList;
        public static AvatarList searchList;

        public static AssetBundle notifBundle;

        public static Vector3 defaultGravity = Vector3.zero;

        public static KeyConfig kConfig;

        public static NotifConfig nConfig;

        public static bool 
            monke = false, 
            welcomed = false, 
            directFly = false, 
            earrape = false, 
            sittingOnPlayer = false, 
            playerESP = false, 
            itemESP = false, 
            triggerESP = false, 
            esp = false;

        public static bool serialize = false, 
            useResetPosSer;

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
                serializeToggleButton.setToggleState(!serialize, true);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("t"))
            {
                if (PU.GetSelectedPlayer() == null)
                    return;

                PU.RequestToTagAlong(PU.GetSelectedPlayer().field_Private_APIUser_0.id);

                //UIU.OpenVRCUIPopup("Tag Along Request", $"Pip would like to tag along with you!", "Accept",
                //    new Action(() =>
                //    {
                //        WebsocketHandler.tagAlongSocket.Send($"{PU.GetPlayer().field_Private_APIUser_0.displayName}:{PU.GetPlayer().field_Private_APIUser_0.id}:{PU.GetPlayer().field_Private_APIUser_0.id}:response:null");
                //    }), "Decline");
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("j"))
            {
                //Console.Write("Enter URL: ");
                //var url = Console.ReadLine();
                //foreach (BaseVRCVideoPlayer vRCVideoPlayer in GameObject.FindObjectsOfType<BaseVRCVideoPlayer>())
                //{
                //    Networking.SetOwner(PU.GetVRCPlayerApi(), vRCVideoPlayer.gameObject);
                //    vRCVideoPlayer.EnableAutomaticResync = false;
                //    CU.Log("[BaseVRCVideoPlayer] Disabled automatic resync");
                //    vRCVideoPlayer.SetTime(0);
                //    CU.Log("[BaseVRCVideoPlayer] Set time to 0");
                //    vRCVideoPlayer.SetIndexMarker(0);
                //    CU.Log("[BaseVRCVideoPlayer] Set index marker to 0");
                //    vRCVideoPlayer.Stop();
                //    CU.Log("[BaseVRCVideoPlayer] Stopped");
                //    vRCVideoPlayer.LoadURL(new VRCUrl(url));
                //    CU.Log($"[BaseVRCVideoPlayer] Loaded URL: {url}");
                //    vRCVideoPlayer.PlayURL(new VRCUrl(url));
                //    CU.Log($"[BaseVRCVideoPlayer] Play URL: {url}");
                //    vRCVideoPlayer.Play();
                //    CU.Log($"[BaseVRCVideoPlayer] Play");
                //    foreach (IUdonBehaviour beh in vRCVideoPlayer._udonBehaviours)
                //    {
                //        beh.InitializeUdonContent();
                //        beh.RunEvent("Fuck you", null);
                //        beh.RunProgram("Fuck you");
                //        beh.InitializeUdonContent();
                //        beh.InitializeUdonContent();
                //    }
                //}

                foreach (GameObject go in PU.GetAllGameObjects())
                {
                    if (go.transform.parent != null && go.transform.parent.parent != null)
                    {
                        CU.Log($"{go.transform.parent.parent.name}/{go.transform.parent.name}/{go.name}");
                    }
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

            friendsFX = HighlightsFX.prop_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
            trustedFX = HighlightsFX.prop_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
            knownFX = HighlightsFX.prop_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
            userFX = HighlightsFX.prop_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
            newUserFX = HighlightsFX.prop_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
            visitorFX = HighlightsFX.prop_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();


            //friendsFX.field_Protected_Material_0.SetColor("_HighlightColor", Color.yellow);
            //trustedFX.field_Protected_Material_0.SetColor("_HighlightColor", Color.magenta);
            //knownFX.field_Protected_Material_0.SetColor("_HighlightColor", (Color.red + (Color.yellow * 2)) / 3);
            //userFX.field_Protected_Material_0.SetColor("_HighlightColor", Color.green);
            //newUserFX.field_Protected_Material_0.SetColor("_HighlightColor", Color.blue);
            //visitorFX.field_Protected_Material_0.SetColor("_HighlightColor", Color.white);


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
            socialReqTagAlongButton = GameObject.Instantiate(ButtonHandler.GetViewUserOnVRChatWebsiteButton(), ButtonHandler.GetViewUserOnVRChatWebsiteButton().transform.parent);
            avatarDownloadVrcaButton = GameObject.Instantiate(ButtonHandler.GetChangeAvatarButton(), ButtonHandler.GetChangeAvatarButton().transform.parent);
            searchAvatarsButton = GameObject.Instantiate(ButtonHandler.GetChangeAvatarButton(), ButtonHandler.GetChangeAvatarButton().transform.parent);

            ButtonHandler.GetOriginalAvatarFavoriteButton().GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 25);
            ButtonHandler.GetOriginalAvatarFavoriteButton().GetComponent<RectTransform>().localScale -= new Vector3(0, 0.1f, 0);

            heavenlyFavoriteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 55);
            heavenlyFavoriteButton.GetComponent<RectTransform>().localScale -= new Vector3(0, 0.1f, 0);
            heavenlyFavoriteButton.GetComponentInChildren<Text>().text = "Heavenly (Un)Favorite";


            ButtonHandler.SetButtonColor(heavenlyFavoriteButton, Color.red);
            heavenlyFavoriteButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            heavenlyFavoriteButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() => { favList.AddOrRemoveFavAvatar(GameObject.FindObjectOfType<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0); }));

            avatarDownloadVrcaButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(285, 162.5f);
            avatarDownloadVrcaButton.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.1f, 0);
            avatarDownloadVrcaButton.GetComponentInChildren<Text>().text = "Download vrca";
            avatarDownloadVrcaButton.GetComponentInChildren<Text>().GetComponent<RectTransform>().localScale += new Vector3(0.1f, 0, 0);

            ButtonHandler.SetButtonColor(avatarDownloadVrcaButton, Color.red);
            avatarDownloadVrcaButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            avatarDownloadVrcaButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() => { PU.DownloadAvatar(GameObject.FindObjectOfType<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0); }));

            searchAvatarsButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(1125, 685);
            searchAvatarsButton.GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0.1f, 0);
            searchAvatarsButton.GetComponentInChildren<Text>().text = "Search";

            socialReqTagAlongButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(-320, 550);
            socialReqTagAlongButton.GetComponent<RectTransform>().localScale += new Vector3(0.2f, -0.1f, 0);
            socialReqTagAlongButton.GetComponentInChildren<Text>().text = "Request Tag Along";
            socialReqTagAlongButton.GetComponentInChildren<Text>().GetComponent<RectTransform>().localScale -= new Vector3(0.2f, 0, 0);

            ButtonHandler.SetButtonColor(socialReqTagAlongButton, Color.red);
            socialReqTagAlongButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            socialReqTagAlongButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() => { PU.RequestToTagAlong(GameObject.FindObjectOfType<PageUserInfo>().field_Public_APIUser_0.id); }));


            ButtonHandler.SetButtonColor(searchAvatarsButton, Color.red);
            searchAvatarsButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();

            searchAvatarsButton.GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
            {
                UIU.OpenKeyboardPopup("Search Avatars", "Enter Author/Avatar name.....", (str, LK, tex) =>
                {
                    searchList.searchQuery = str;
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

            #region Shortcut Menu

            #region Main Menu

            gameMenu = new QMNestedButton(mainMenu, 0.25f, 0.4225f, "Game", "VRChat Client Options", Color.red);

            #region Game Menu

            restartButton = new QMSingleButton(gameMenu, 0.25f, 0.4225f, "Restart Game", delegate { CU.RestartGame(); }, "Restart VRChat", Color.red);
            restartRejoinButton = new QMSingleButton(gameMenu, 0.25f, 1.275f, "Restart and\n Rejoin Game", delegate { CU.RestartRejoinGame(); }, "Restart VRChat and rejoin your current lobby", Color.red);

            #endregion

            photonMenu = new QMNestedButton(mainMenu, 0.25f, 1.275f, "Photon", "Photon Options", Color.red);

            #region Photon Menu

            serializeToggleButton = new QMToggleButton(photonMenu, 0.25f, 0.4225f, "Serialize\nON", delegate
            {
                serPos = PU.GetVRCPlayer().transform.position;
                serRot = PU.GetVRCPlayer().transform.rotation;
                serialize = true;
            },
            "Serialize\nOFF", delegate
            {
                if (useResetPosSer)
                {
                    PU.GetVRCPlayer().transform.position = serPos;
                    PU.GetVRCPlayer().transform.rotation = serRot;
                }
                serialize = false;
            },
            "Toggle serialize (stay in one position for others while you move)", Color.red);

            serializePosToggleButton = new QMToggleButton(photonMenu, 1f, 0f, "Reset Pos\nON", delegate
            {
                useResetPosSer = true;
            },
            "Reset Pos\nOFF", delegate  
            {
                useResetPosSer = false;
            },
            "Reset to your original postion when turning serialize off", Color.red);

            searchBotButton = new QMSingleButton(photonMenu, 0.25f, 1.275f, "Search For\nPlayer", delegate { }, "Search for a player in public worlds", Color.red);

            #endregion

            avatarMenu = new QMNestedButton(mainMenu, 1f, 0.86f, "Avatar", "Current Avatar Options", Color.red);

            #region Avatar Menu

            forceCloneIdButton = new QMSingleButton(avatarMenu, 0.25f, 0.4225f, "Force Clone\nby ID", delegate
            {
                UIU.OpenKeyboardPopup("Force Clone ID", "Enter Avatar ID.....", (str, LK, tex) =>
                {
                    PU.ForceCloneByID(str);
                });
            }, "Force clone an avatar by the avatar ID", Color.red);

            #endregion

            voiceMenu = new QMNestedButton(mainMenu, 1f, 0f, "Voice", "Voice Options", Color.red);

            #region Voice Options

            voiceGainSlider = new VRCSlider(voiceMenu, new Vector2(-0.3f, 19), new Vector4(1, 500, 2, 1), "Voice Gain", new Action<float>((e) => { USpeaker.field_Internal_Static_Single_1 = e; }));
            othersVoiceGainSlider = new VRCSlider(voiceMenu, new Vector2(-0.3f, 16), new Vector4(1, 500, 2, 1), "Others Voice Gain", new Action<float>((e) => { USpeaker.field_Internal_Static_Single_0 = e; }));

            #endregion

            sitMenu = new QMNestedButton(mainMenu, 1f, 1.72f, "Sit", "Sit On Player Options", Color.red);

            #region Sit Menu

            headSitOptionButton = new QMSingleButton(sitMenu, 2.50f, 0, "Head", delegate
            {
                sitBone = HumanBodyBones.Head;
                foreach (QMSingleButton sitOptionButton in sitOptionButtons)
                {
                    sitOptionButton.setBackgroundColor(Color.cyan);
                }
                headSitOptionButton.setBackgroundColor(Color.red);
            }, "Sit on head", Color.red);

            torsoSitOptionButton = new QMSingleButton(sitMenu, 2.50f, 0.86f, "Torso", delegate
            {
                sitBone = HumanBodyBones.Hips;
                foreach (QMSingleButton sitOptionButton in sitOptionButtons)
                {
                    sitOptionButton.setBackgroundColor(Color.cyan);
                }
                torsoSitOptionButton.setBackgroundColor(Color.red);
            }, "Sit on Torso/Hips", Color.cyan);

            lHandSitOptionButton = new QMSingleButton(sitMenu, 1.75f, 0.4225f, "Left Hand", delegate
            {
                sitBone = HumanBodyBones.LeftHand;
                foreach (QMSingleButton sitOptionButton in sitOptionButtons)
                {
                    sitOptionButton.setBackgroundColor(Color.cyan);
                }
                lHandSitOptionButton.setBackgroundColor(Color.red);
            }, "Sit on Left Hand", Color.cyan);

            lFootSitOptionButton = new QMSingleButton(sitMenu, 1.75f, 1.275f, "Left Foot", delegate
            {
                sitBone = HumanBodyBones.LeftFoot;
                foreach (QMSingleButton sitOptionButton in sitOptionButtons)
                {
                    sitOptionButton.setBackgroundColor(Color.cyan);
                }
                lFootSitOptionButton.setBackgroundColor(Color.red);
            }, "Sit on Left Foot", Color.cyan);

            rHandSitOptionButton = new QMSingleButton(sitMenu, 3.25f, 0.4225f, "Right Hand", delegate
            {
                sitBone = HumanBodyBones.RightHand;
                foreach (QMSingleButton sitOptionButton in sitOptionButtons)
                {
                    sitOptionButton.setBackgroundColor(Color.cyan);
                }
                rHandSitOptionButton.setBackgroundColor(Color.red);
            }, "Sit on Right Hand", Color.cyan);

            rFootSitOptionButton = new QMSingleButton(sitMenu, 3.25f, 1.275f, "Right Foot", delegate
            {
                sitBone = HumanBodyBones.RightFoot;
                foreach (QMSingleButton sitOptionButton in sitOptionButtons)
                {
                    sitOptionButton.setBackgroundColor(Color.cyan);
                }
                rFootSitOptionButton.setBackgroundColor(Color.red);
            }, "Sit on Right Foot", Color.cyan);

            sitOptionButtons.AddRange(new[] { headSitOptionButton, torsoSitOptionButton, lHandSitOptionButton, lFootSitOptionButton, rHandSitOptionButton, rFootSitOptionButton });

            #endregion

            espMenu = new QMNestedButton(mainMenu, 1.75f, 0.4225f, "ESP", "ESP Options", Color.red);

            #region ESP Menu

            playerESPToggleButton = new QMToggleButton(espMenu, 0.25f, 0.4225f, "Players\nON", delegate
            {
                playerESP = true;
                if (esp)
                {
                    foreach (Player p in PU.GetAllPlayers())
                    {
                        if (p.transform.Find("SelectRegion"))
                        {
                            if (p.field_Private_APIUser_0.isFriend)
                            {
                                Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasVeteranTrustLevel)
                            {
                                Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasTrustedTrustLevel)
                            {
                                Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasKnownTrustLevel)
                            {
                                Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasBasicTrustLevel)
                            {
                                Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else
                            {
                                Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                        }
                    }
                }
            }, "Players\nOFF", delegate
            {
                playerESP = false;
                if (esp)
                {
                    foreach (Player p in PU.GetAllPlayers())
                    {
                        if (p.transform.Find("SelectRegion"))
                        {
                            if (p.field_Private_APIUser_0.isFriend)
                            {
                                Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                            }
                            else if (p.field_Private_APIUser_0.hasVeteranTrustLevel)
                            {
                                Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                            }
                            else if (p.field_Private_APIUser_0.hasTrustedTrustLevel)
                            {
                                Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                            }
                            else if (p.field_Private_APIUser_0.hasKnownTrustLevel)
                            {
                                Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                            }
                            else if (p.field_Private_APIUser_0.hasBasicTrustLevel)
                            {
                                Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                            }
                            else
                            {
                                Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                            }
                        }
                    }
                }
            }, "Toggle Player ESP", Color.red);

            itemESPToggleButton = new QMToggleButton(espMenu, 0.25f, 1.275f, "Items ON", delegate
            {
                itemESP = true;

                if (esp)
                {
                    foreach (VRC_Pickup pickup in PU.GetAllPickups())
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(pickup.GetComponentInChildren<MeshRenderer>(), true);
                    }
                }
            }, "Items OFF", delegate
            {
                itemESP = false;
                if (esp)
                {
                    foreach (VRC_Pickup pickup in PU.GetAllPickups())
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(pickup.GetComponentInChildren<MeshRenderer>(), false);
                    }
                }
            }, "Toggle Item ESP", Color.red);

            triggerESPToggleButton = new QMToggleButton(espMenu, 1f, 0.86f, "Triggers\nON", delegate
            {
                triggerESP = true;
                if (esp)
                {
                    foreach (VRC_Trigger trigger in PU.GetAllTriggers())
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(trigger.GetComponentInChildren<MeshRenderer>(), true);
                    }
                }
            }, "Triggers\nOFF", delegate
            {
                triggerESP = false;
                if (esp)
                {
                    foreach (VRC_Trigger trigger in PU.GetAllTriggers())
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(trigger.GetComponentInChildren<MeshRenderer>(), false);
                    }
                }
            }, "Toggle Trigger ESP", Color.red);
            #endregion

            tagAlongMenu = new QMNestedButton(mainMenu, 1.75f, 1.275f, "Tag\nAlong", "Tag Along Options", Color.red);

            #region Tag Along Menu

            cancelTagAlongButton = new QMSingleButton(tagAlongMenu, 0.25f, 0.4225f, "Cancel\nTag Along", delegate
            {
                
            }, "Cancel anyone you are being tagged along with, and who you are tagging along with", Color.red);

            #endregion

            #endregion

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
            "Toggle Direct Fly", Color.red);

            espToggleButton = new QMToggleButton("ShortcutMenu", 0.25f, 1.275f, "ESP ON", delegate
            {
                esp = true;

                HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor("_HighlightColor", Color.red);
                friendsFX.field_Protected_Material_0.SetColor("_HighlightColor", VRCPlayer.field_Internal_Static_Color_1);
                trustedFX.field_Protected_Material_0.SetColor("_HighlightColor", VRCPlayer.field_Internal_Static_Color_6);
                knownFX.field_Protected_Material_0.SetColor("_HighlightColor", VRCPlayer.field_Internal_Static_Color_5);
                userFX.field_Protected_Material_0.SetColor("_HighlightColor", VRCPlayer.field_Internal_Static_Color_4);
                newUserFX.field_Protected_Material_0.SetColor("_HighlightColor", VRCPlayer.field_Internal_Static_Color_3);
                visitorFX.field_Protected_Material_0.SetColor("_HighlightColor", VRCPlayer.field_Internal_Static_Color_2);

                if (Main.playerESP)
                {
                    foreach (Player p in PU.GetAllPlayers())
                    {
                        if (p.transform.Find("SelectRegion"))
                        {
                            if (p.field_Private_APIUser_0.isFriend)
                            {
                                Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasVeteranTrustLevel)
                            {
                                Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasTrustedTrustLevel)
                            {
                                Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasKnownTrustLevel)
                            {
                                Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else if (p.field_Private_APIUser_0.hasBasicTrustLevel)
                            {
                                Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                            else
                            {
                                Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                            }
                        }
                    }
                }

                if (Main.itemESP)
                {
                    foreach (VRC_Pickup pickup in PU.GetAllPickups())
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(pickup.GetComponentInChildren<MeshRenderer>(), true);
                    }
                }

                if (Main.triggerESP)
                {
                    foreach (VRC_Trigger trigger in PU.GetAllTriggers())
                    {
                        HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(trigger.GetComponentInChildren<MeshRenderer>(), true);
                    }
                }

            }, "ESP OFF", delegate
            {
                esp = false;

                foreach (Player p in PU.GetAllPlayers())
                {
                    if (p.transform.Find("SelectRegion"))
                    {
                        if (p.field_Private_APIUser_0.isFriend)
                        {
                            Main.friendsFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                        else if (p.field_Private_APIUser_0.hasVeteranTrustLevel)
                        {
                            Main.trustedFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                        else if (p.field_Private_APIUser_0.hasTrustedTrustLevel)
                        {
                            Main.knownFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                        else if (p.field_Private_APIUser_0.hasKnownTrustLevel)
                        {
                            Main.userFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                        else if (p.field_Private_APIUser_0.hasBasicTrustLevel)
                        {
                            Main.newUserFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                        else
                        {
                            Main.visitorFX.Method_Public_Void_Renderer_Boolean_0(p.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                    }
                }

                foreach (VRC_Pickup pickup in PU.GetAllPickups())
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(pickup.GetComponentInChildren<MeshRenderer>(), false);
                }

                foreach (VRC_Trigger trigger in PU.GetAllTriggers())
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(trigger.GetComponentInChildren<MeshRenderer>(), false);
                }
            }, "Toggle ESP", Color.red);


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

            #endregion

            #region UserInteractMenu

            selectedMenu = new QMNestedButton("UserInteractMenu", 4, 1, "Options", "Selected options", Color.red);

            #region Selected Menu

            selectedAvatarMenu = new QMNestedButton(selectedMenu, 0.25f, 0.4225f, "Avatar", "Avatar Options", Color.red);

            #region Selected Avatar Menu

            selectedDownloadVrcaButton = new QMSingleButton(selectedAvatarMenu, 0.25f, 0.4225f, "Download vrca", delegate { PU.DownloadAvatar(PU.GetSelectedPlayer().prop_ApiAvatar_0); }, "Download selected player's avatar vrca", Color.red);

            #endregion

            selectedTagAlongMenu = new QMNestedButton(selectedMenu, 0.25f, 1.275f, "Tag Along", "Tag Along Options", Color.red);

            #region Selected Tag Along Menu

            selectedTagAlongButton = new QMSingleButton(selectedTagAlongMenu, 0.25f, 0.4225f, "Request\nTag Along", delegate { PU.RequestToTagAlong(PU.GetSelectedPlayer().field_Private_APIUser_0.id); }, "Request to 'Tag Along' with selected player to other worlds", Color.red);

            #endregion

            #endregion

            #endregion

            playerESPToggleButton.setToggleState(true, true);

        }

        public static void ToggleSitOnHead()
        {
            sittingOnPlayer = !sittingOnPlayer;
            Physics.gravity = sittingOnPlayer ? Vector3.zero : defaultGravity;
            MelonCoroutines.Start(SitOnPlayer());
        }

        public static IEnumerator SitOnPlayer()
        {
            while (sittingOnPlayer)
            {
                if (selectedSit == null)
                    yield break;

                if (sitBone == HumanBodyBones.Head)
                {
                    PU.GetVRCPlayer().transform.position = selectedSit.field_Private_VRCPlayerApi_0.GetBonePosition(sitBone) + new Vector3(0, 0.1f, 0);
                }
                else
                {
                    PU.GetVRCPlayer().transform.position = selectedSit.field_Private_VRCPlayerApi_0.GetBonePosition(sitBone);
                }
                yield return null;
            }
        }
    }
}
