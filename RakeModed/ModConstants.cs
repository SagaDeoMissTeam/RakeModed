using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace RakeModed
{
    public class ModConstants
    {
        public static string GAME_VERSION = "0.0.2";
        
        protected static List<string> FOLDERS = new List<string>();
        public static string gameFolder = UnityEngine.Application.dataPath.Replace("Rake(multiplayer)_Data", "");
        public static string assetsFolder = registerFolder(gameFolder + "Assets/");

        public static string texturesFolder = registerFolder(assetsFolder + "Textures/");
        public static string meshFolder = registerFolder(assetsFolder + "Mesh/");
        public static string bundlesFolder = registerFolder(assetsFolder + "Bundles/");
        public static string fontFolder = registerFolder(assetsFolder + "Fonts/");
        public static string D1 = registerFolder(Application.streamingAssetsPath);

        public static string registerFolder(string path)
        {
            FOLDERS.Add(path);
            return path;
        }

        
        public static void createFolders()
        {
            foreach (string dir in FOLDERS)
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);   
            }
        }
    }
}