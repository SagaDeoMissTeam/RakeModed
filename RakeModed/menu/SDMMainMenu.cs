using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MelonLoader;
using RakeModed.utils;
using UnityEngine;

namespace RakeModed.menu
{
    internal class SDMMainMenu
    {

        public static void init()
        {
            
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    
            // for (var i = 0; i < allObjects.Length; i++)
            // {
            //     if(allObjects[i].GetComponent<Camera>() != null) continue;
            //     MelonLogger.Msg($"Destroy: {allObjects[i].name}");
            //     UnityEngine.Object.Destroy(allObjects[i]);   
            // }

            AssetBundle assetBundle = new AssetBundle();
            
            
            MelonLogger.Msg("End Delete Objects");
            
            GameObject d1 = new GameObject();
            d1.name = "LevelManager";
            d1.AddComponent<MenuManager>();

            GameObject assetLoader = new GameObject();
            assetLoader.name = "AssetLoader";
            assetLoader.AddComponent<AssetLoader>();


        }

        
    }
}
