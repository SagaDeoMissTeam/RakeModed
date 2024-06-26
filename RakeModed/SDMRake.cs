﻿using System.Reflection;
using MelonLoader;
using RakeModed.menu;
using UnityEngine;

namespace RakeModed
{
    public static class BuildInfo
    {
        public const string Name = "SDMRake"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Mod for Rake"; // Description for the Mod.  (Set as null if none)
        public const string Author = "Sixik"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class SDMRake : MelonMod
    {
        public AssetBundle AssetBundle;
        public static Assembly assembly => Assembly.GetExecutingAssembly();
        
        
        public override void OnInitializeMelon()
        {
            HarmonyInstance.PatchAll(typeof(SDMRake));
            
            ModConstants.createFolders();
        }
        
        public override void OnLateInitializeMelon() // Runs after OnApplicationStart.
        {
            SDMInitMainScene.init();
            MelonLogger.Msg("OnApplicationLateStart");
        }
        
        
        public override void OnSceneWasInitialized(int buildindex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
            MelonLogger.Msg("OnSceneWasInitialized: " + buildindex.ToString() + " | " + sceneName);
        }
    }
}
