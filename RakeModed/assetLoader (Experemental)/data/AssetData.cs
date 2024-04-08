using System.Collections.Generic;
using UnityEngine;

namespace RakeModed.utils
{
    public class AssetData
    {
        public static Dictionary<string, Texture> TEXTURES = new Dictionary<string, Texture>();
        public static Dictionary<string, GameObject> MESHES = new Dictionary<string, GameObject>();


        public static void reload()
        {
            TEXTURES.Clear();
            MESHES.Clear();
        }
    }
}