using System.IO;

namespace RakeModed
{
    public class ModConstants
    {

        public static string gameFolder = UnityEngine.Application.dataPath.Replace("Rake(multiplayer)_Data", "/");
        public static string assetsFolder = gameFolder + "Assets/";

        public static string texturesFolder = assetsFolder + "Textures/";
        public static string meshFolder = assetsFolder + "Mesh/";
    }
}