using System;
using System.Collections;
using MelonLoader;
using RakeModed.utils;
using UnityEngine;
using UnityEngine.UI;

namespace RakeModed.menu
{
    public class MenuManager : MonoBehaviour
    {
        private void Start()
        {
            name = "SDM";

            MelonLogger.Msg("SDM CREATED");
            
            MelonLogger.Msg("LOAD ?");

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
    
                for (var i = 0; i < allObjects.Length; i++)
                {
                    MelonLogger.Msg($"{allObjects[i].name}: {allObjects[i].transform.position}");
                    // UnityEngine.Object.Destroy(allObjects[i]);   
                }
            }
        }
        
        protected GameObject createMainMenu()
        {
            GameObject obj = new GameObject();
            
            obj.name = "SDMMenu";
            obj.layer = UnityEngine.LayerMask.NameToLayer("UI");
            
            RectTransform rectTransform = obj.AddComponent<RectTransform>();
            rectTransform.position = new Vector3(574, 272, 0);
            rectTransform.sizeDelta = new Vector2(1148, 544);
            
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

            GameObject text = UIHelper.createText("text", new Vector2(10, 10),"ITS ME SIXIK", 14, 1, true, TextAnchor.LowerCenter,
                Color.black);
            
            UIHelper.addObjectToParent(text,obj);
            
            return obj;
        }
    }
}