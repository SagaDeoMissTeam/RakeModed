


using UnityEngine;
using UnityEngine.UI;

namespace RakeModed.utils
{
    public class UIHelper
    {

        public static void addObjectToParent(GameObject obj, GameObject parent)
        {
            obj.transform.SetParent(parent.transform, false);
        }
        
        public static GameObject createText(string name, Vector2 pos, string message, int fontSize, int lineSpacing, bool richText, TextAnchor alignment, Color color)
        {
            
            GameObject obj = createCanvasRenderer();
            obj.name = name;
            RectTransform transform = obj.AddComponent<RectTransform>();
            transform.rect.Set(0,0,200,20);
            
            Text text = obj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = message;
            text.fontSize = fontSize;
            text.lineSpacing = lineSpacing;
            text.supportRichText = richText;
            text.alignment = alignment;
            text.color = color;
            
            return obj;
        }



        public static GameObject createCanvasRenderer()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<CanvasRenderer>();
            return obj;
        }
    }
}