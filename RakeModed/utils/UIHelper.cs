


using System;
using MelonLoader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RakeModed.utils
{
    public class UIHelper
    {

        public static void addObjectToParent(GameObject obj, GameObject parent)
        {
            obj.transform.SetParent(parent.transform, false);
        }
        
        /*
        ------------------------------TEXT------------------------------
         */
        
        public static GameObject createText(string name, Vector2 pos,Vector2 size, string message, int fontSize, int lineSpacing, bool richText, TextAnchor alignment, Color color)
        {
            
            GameObject obj = createCanvasRenderer();
            obj.name = name;
            RectTransform transform = obj.AddComponent<RectTransform>();
            transform.rect.Set(pos.x,pos.y,size.x,size.y);
            transform.localPosition = pos;
            transform.sizeDelta = size;
            
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

        /*
        ------------------------------IMAGE------------------------------
         */
        public static GameObject createImage(string name, Vector2 pos, Vector2 size, Sprite sourceImage)
        {
            return createImage(name, pos,size, sourceImage, Color.white, null, false);
        }       
        
        public static GameObject createImage(string name, Vector2 pos, Vector2 size, Color color)
        {
            return createImage(name, pos,size, null, color, null, false);
        }
        
        public static GameObject createImage(string name, Vector2 pos, Vector2 size, Sprite sourceImage, Color color, Material material, bool preserveAspect)
        {
            GameObject obj = createCanvasRenderer();
            obj.name = name;
            RectTransform transform = obj.AddComponent<RectTransform>();
            transform.rect.Set(pos.x,pos.y,size.x,size.y);
            transform.sizeDelta = size;
            transform.localPosition = pos;

            Image image = obj.AddComponent<Image>();
            image.sprite = sourceImage;
            image.color = color;
            image.material = material;
            image.preserveAspect = preserveAspect;
            

            return obj;
        }
        
        /*
        ------------------------------RAW IMAGE------------------------------
         */
        public static GameObject createRawImage(string name, Vector2 pos, Vector2 size, Texture texture)
        {
            return createRawImage(name, pos,size, texture, Color.white, null, new Rect(0,0,1,1));
        }

        public static GameObject createRawImage(string name, Vector2 pos, Vector2 size, Texture texture, Color color, Material material, Rect UV)
        {
            GameObject obj = createCanvasRenderer();
            obj.name = name;
            RectTransform transform = obj.AddComponent<RectTransform>();
            transform.rect.Set(pos.x,pos.y,size.x,size.y);
            transform.sizeDelta = size;
            transform.localPosition = pos;

            RawImage image = obj.AddComponent<RawImage>();
            image.texture = texture;
            image.color = color;
            image.material = material;
            image.uvRect = UV;
            return obj;
        }
        
        /*
        ------------------------------BUTTON------------------------------
        */        
        public static GameObject createButton(string name, Vector2 pos, Vector2 size, Sprite sourceImage, Color color, Material material, bool preserveAspect, 
            bool interactable, Selectable.Transition transition, Navigation navigation, UnityAction d1)
        {
            GameObject obj = createImage(name, pos, size, sourceImage, color, material, preserveAspect);
            Button button = obj.AddComponent<Button>();

            button.interactable = interactable;
            button.transition = transition;
            button.navigation = navigation;
            button.onClick.AddListener(d1);
            
            return obj;
        }
        
        /*
        ------------------------------BUTTON------------------------------
        */  
        public static GameObject createSlider(string name, Vector2 pos, Vector2 size, Sprite sourceImage, Color color, Material material, bool preserveAspect, 
            bool interactable, Selectable.Transition transition, Navigation navigation, Button.ButtonClickedEvent func)
        {
            GameObject obj = createImage(name, pos, size, sourceImage, color, material, preserveAspect);
            Button button = obj.AddComponent<Button>();

            button.interactable = interactable;
            button.transition = transition;
            button.navigation = navigation;
            button.onClick = func;
            
            return obj;
        }
            
        public static GameObject createCanvasRenderer()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<CanvasRenderer>();
            return obj;
        }

        public static GameObject createCanvas()
        {
            GameObject obj = new GameObject();
            obj.name = "Canvas";
            
            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.rect.Set(574, 272, 1148, 544);
            
            Canvas canvasComponent = obj.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasComponent.pixelPerfect = false;
            canvasComponent.sortingOrder = 0;
            
            CanvasScaler canvasScaler = obj.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            canvasScaler.scaleFactor = 1;
            canvasScaler.referencePixelsPerUnit = 100;

            GraphicRaycaster graphicRaycaster = obj.AddComponent<GraphicRaycaster>();
            graphicRaycaster.ignoreReversedGraphics = true;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            return obj;
        }
        
    }
}