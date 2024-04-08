using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MelonLoader;
using RakeModed.assetLoader__Experemental_;
using RakeModed.assetLoader__Experemental_.data;
using RakeModed.events;
using RakeModed.utils;
using UnityEngine;
using UnityEngine.UI;

namespace RakeModed.menu
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        
        private void OnEnable()
        {
            GlobalEvents.OnAssetsLoaded += onAssetsLoaded;
        }

        private void OnDestroy()
        {
            GlobalEvents.OnAssetsLoaded -= onAssetsLoaded;
        }

        private void Start()
        {
            name = "SDMMenuManager";
            
            createMainMenuV1();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

                for (var i = 0; i < allObjects.Length; i++)
                {
                    MelonLogger.Msg($"{allObjects[i].name}: {allObjects[i].transform.position}");
                    // UnityEngine.Object.Destroy(allObjects[i]);   
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                
                // createMainMenu();
            }
        }
        
        public void onAssetsLoaded()
        {
            
            mainMenu = createMainMenuV1();
            MelonLogger.Msg("Main Menu Created !");
        }
        
        public GameObject createMainMenuV1()
        {
            
            GameObject obj = UIHelper.createCanvas();
            obj.name = "SDMMenu";
            obj.layer = UnityEngine.LayerMask.NameToLayer("UI");
            
            
            GameObject MainBackGround = UIHelper.createImage("MainScreenBackGround", new Vector2(0,0), new Vector2(0, 0), Color.black);
            RectTransform rectTransform = MainBackGround.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1,1,1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            UIHelper.addObjectToParent(MainBackGround, obj);
            
            
            GameObject backGround = UIHelper.createImage("BackGround", new Vector2(0,0), new Vector2(297f, 463f), Color.black); 
            rectTransform = backGround.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1,1,1);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            UIHelper.addObjectToParent(backGround, obj);
            
            GameObject button = UIHelper.createButton("Exit_Button", new Vector2(0, -15f), new Vector2(160, 30),
                Resources.Load<Sprite>("unity_builtin_extra/UISprite"), Color.white, null, false, true,
                Selectable.Transition.ColorTint, Navigation.defaultNavigation, Application.Quit);
            UIHelper.addObjectToParent(button, backGround);
            
            GameObject childObject = UIHelper.createText("Text", new Vector2(0,0), new Vector2(0,0),
                "Exit",
                20, 1, true, TextAnchor.MiddleCenter, Color.black);
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            UIHelper.addObjectToParent(childObject, button);        
            
            button = UIHelper.createButton("Setting_Button", new Vector2(0, 16f), new Vector2(160, 30),
                Resources.Load<Sprite>("unity_builtin_extra/UISprite"), Color.white, null, false, true,
                Selectable.Transition.ColorTint, Navigation.defaultNavigation, (() => {}));
            UIHelper.addObjectToParent(button, backGround);
            
            childObject = UIHelper.createText("Text", new Vector2(0,0), new Vector2(0,0),
                "Settings",
                20, 1, true, TextAnchor.MiddleCenter, Color.black);
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            UIHelper.addObjectToParent(childObject, button);               
            
            button = UIHelper.createButton("Connect_Button", new Vector2(0, 47f), new Vector2(160, 30),
                Resources.Load<Sprite>("unity_builtin_extra/UISprite"), Color.white, null, false, true,
                Selectable.Transition.ColorTint, Navigation.defaultNavigation, (() => {}));
            UIHelper.addObjectToParent(button, backGround);
            
            childObject = UIHelper.createText("Text", new Vector2(0,0), new Vector2(0,0),
                "Connect",
                20, 1, true, TextAnchor.MiddleCenter, Color.black);
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            UIHelper.addObjectToParent(childObject, button);       
            
            button = UIHelper.createButton("HostServer_Button", new Vector2(0, 78f), new Vector2(160, 30),
                Resources.Load<Sprite>("unity_builtin_extra/UISprite"), Color.white, null, false, true,
                Selectable.Transition.ColorTint, Navigation.defaultNavigation, (() => {}));
            UIHelper.addObjectToParent(button, backGround);
            
            childObject = UIHelper.createText("Text", new Vector2(0,0), new Vector2(0,0),
                "Host Server",
                20, 1, true, TextAnchor.MiddleCenter, Color.black);
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            UIHelper.addObjectToParent(childObject, button);     
            
            
            childObject = UIHelper.createImage("RakeLogo", new Vector2(6, -7.5f), new Vector2(432, 163),
                AssetConverter.convertFromTexture(AssetData.TEXTURES["screen_selector.png"].obj));
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            UIHelper.addObjectToParent(childObject, obj);
            
            
            
            

            childObject = UIHelper.createRawImage("SDMLogo", new Vector2(226.6f, -7.5f), new Vector2(82, 82),
                AssetData.TEXTURES["sdm_logo.png"].obj);
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            UIHelper.addObjectToParent(childObject, obj);

            childObject = UIHelper.createText("GameInfo", new Vector2(6.5f, 15f), new Vector2(363, 29),
                "Rake Multiplayer Moded by Sixik",
                20, 1, true, TextAnchor.MiddleLeft, Color.white);
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0, 0.5f);
            UIHelper.addObjectToParent(childObject, obj);
            
            childObject = UIHelper.createText("GameVersion", new Vector2(6.5f, 43.5f), new Vector2(233.5f, 26f),
                "Version " + ModConstants.GAME_VERSION,
                20, 1, true, TextAnchor.MiddleLeft, Color.white);
            rectTransform = childObject.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(0, 0.5f);
            UIHelper.addObjectToParent(childObject, obj);     
            
            return obj;
        }

        public void send()
        {
            MelonLogger.Msg("TEST ?");
        }
    }
}