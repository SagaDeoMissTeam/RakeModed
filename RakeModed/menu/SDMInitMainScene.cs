using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MelonLoader;
using MelonLoader.ICSharpCode.SharpZipLib;
using RakeModed.assetLoader__Experemental_;
using RakeModed.utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RakeModed.menu
{
    internal class SDMInitMainScene
    {

        public static void init()
        {
            
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    
            for (var i = 0; i < allObjects.Length; i++)
            {
                if(allObjects[i].GetComponent<Camera>() != null || allObjects[i].GetComponent<EventSystem>() != null) continue;
                MelonLogger.Msg($"Destroy: {allObjects[i].name}");
                UnityEngine.Object.Destroy(allObjects[i]);   
            }

            AssetBundle assetBundle = new AssetBundle();
            
            
            MelonLogger.Msg("End Delete Objects");
            
            GameObject assetLoader = new GameObject();
            assetLoader.name = "AssetLoader";
            AssetLoader d2 = assetLoader.AddComponent<AssetLoader>();
            
            GameObject d1 = new GameObject();
            d1.name = "LevelManager";
            d1.AddComponent<MenuManager>();
        }
    }
}
