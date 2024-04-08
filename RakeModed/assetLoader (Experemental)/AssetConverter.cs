using UnityEngine;

namespace RakeModed.assetLoader__Experemental_
{
    public class AssetConverter
    {
        public static Sprite convertFromTexture(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}