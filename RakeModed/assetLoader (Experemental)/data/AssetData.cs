using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine;

namespace RakeModed.assetLoader__Experemental_.data
{
    public class AssetData
    {
        public static Dictionary<string, SDMAsset<Texture2D>> TEXTURES = new Dictionary<string, SDMAsset<Texture2D>>();
        public static Dictionary<string, SDMAsset<GameObject>> MESHES = new Dictionary<string, SDMAsset<GameObject>>();
        public static Dictionary<string, SDMAsset<AssetBundle>> BUNDLES = new Dictionary<string, SDMAsset<AssetBundle>>();
        public static Dictionary<string, Font> FONTS = new Dictionary<string, Font>();
        
        public static Texture2D getTextureById(int id)
        {
            SDMAsset<Texture2D>[] texturesArray = TEXTURES.Values.ToArray();
            if (id >= 0 && id < texturesArray.Length)
            {
                return texturesArray[id].obj;
            }
            else
            {
                MelonLogger.Error("Invalid index for getting texture from dictionary.");
                return null;
            }
        }
        
        public static void reload()
        {
            TEXTURES.Clear();
            MESHES.Clear();
            BUNDLES.Clear();
            FONTS.Clear();
        }
    }
}