using RakeModed.assetLoader__Experemental_;
using RakeModed.assetLoader__Experemental_.data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RakeModed.utils
{
    public class UIHelperEXP
    {
        public static AssetBundle bundle;
        public static GameObject createText(string name)
        {
           GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("Text"));
           obj.name = name;
           return obj;
        }

        public static GameObject createImage(string name, Texture2D texture2D)
        {
            GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("Image"));
            obj.GetComponent<Image>().sprite = AssetConverter.convertFromTexture(texture2D);
            obj.name = name;
            return obj;
        }
        public static GameObject createRawImage(string name, Texture2D texture2D)
        {
            GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("RawImage"));
            obj.GetComponent<RawImage>().texture = texture2D;
            obj.name = name;
            return obj;
        }        
        
        public static GameObject createToggle(string name, UnityAction<bool> toggleEvent)
        {
            GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("Toggle"));
            obj.GetComponent<Toggle>().onValueChanged.AddListener(toggleEvent);
            obj.name = name;
            return obj;
        }        
        
        public static GameObject createButton(string name, UnityAction clickEvent)
        {
            GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("Button"));
            obj.GetComponent<Button>().onClick.AddListener(clickEvent);
            obj.name = name;
            return obj;
        }        
        
        public static GameObject createInputField(string name, UnityAction<string> valueChangeEvent)
        {
            GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("InputField"));
            obj.GetComponent<InputField>().onValueChange.AddListener(valueChangeEvent);
            obj.name = name;
            return obj;
        }        
        public static GameObject createSlider(string name, UnityAction<float> valueChangeEvent)
        {
            GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("Slider"));
            obj.GetComponent<Slider>().onValueChanged.AddListener(valueChangeEvent);
            obj.name = name;
            return obj;
        }        
        
        public static GameObject createPanel(string name)
        {
            GameObject obj = Object.Instantiate(bundle.LoadAsset<GameObject>("Panel"));
            obj.name = name;
            return obj;
        }
    }
}