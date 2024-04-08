using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using MelonLoader;
using RakeModed.assetLoader__Experemental_.data;
using RakeModed.events;
using UnityEngine;

namespace RakeModed.assetLoader__Experemental_
{
    public class AssetLoader : MonoBehaviour
    {
        public bool isAllLoaded = false;
        
        protected IEnumerator Start()
        {
            MelonLogger.Msg("Start load Assets");
            AssetData.reload();
            
            MelonLogger.Msg("Load BUNDLES");
            string[] files = Directory.GetFiles(ModConstants.bundlesFolder);
            
            foreach (var file in files)
            {
                if (IsAssetBundleFile(file))
                {
                    yield return loadAssetBundles(file);
                }
            }
            
            MelonLogger.Msg("Load TEXTURES");
            files = Directory.GetFiles(ModConstants.texturesFolder);
            foreach (string file in files)
            {
                if (IsImageFile(file))
                {
                    yield return StartCoroutine(loadTextures(file));
                }
            }
            
            MelonLogger.Msg("Load MODELS");
            files = Directory.GetFiles(ModConstants.meshFolder);
            foreach (string file in files)
            {
                
                if (IsModelFile(file))
                {
                    yield return StartCoroutine(loadModels(file));
                }
            }
            
            MelonLogger.Msg("Load FONTS");
            files = Directory.GetFiles(ModConstants.fontFolder);
            foreach (string file in files)
            {
                
                if (IsFontFile(file))
                {
                    yield return StartCoroutine(loadFont(file));
                }
            }
            
            MelonLogger.Msg("End load Assets");
            sendStatistic();
        }

        private IEnumerator loadFont(string filePath)
        {
            string path = "file://" + filePath;
            
            WWW www = new WWW(path);
            
            yield return www;
            
            if (www.error != null)
            {
                MelonLogger.Error("Error load: " + www.error);
            }
            else
            {
                byte[] fontData = File.ReadAllBytes(filePath);
                
                
                Font font = new Font();
                font.material = new Material(Shader.Find("UI/Default"));
                font.name = Path.GetFileName(filePath).Replace(".otf", "");
                font.RequestCharactersInTexture("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");
                
                MelonLogger.Msg($"Loaded: {Path.GetFileName(filePath)}");
            }
        }
        
        private IEnumerator loadAssetBundles(string filePath)
        {
            string path = "file://" + filePath;


            WWW www = new WWW(path);

            yield return www;

            try
            {
                if (www.error != null)
                {
                    MelonLogger.Error("Error load: " + www.error);
                }
                else
                {
                    SDMAsset<AssetBundle> asset =
                        new SDMAsset<AssetBundle>(Path.GetFileName(filePath), www.assetBundle);
                    AssetData.BUNDLES.Add(asset.name, asset);
                    MelonLogger.Msg($"Loaded: {asset.name}");
                }

            }
            catch (Exception e)
            {
                MelonLogger.Error(e);
            }
        }
        
        private IEnumerator loadTextures(string filePath)
        {
            string path = "file://" + filePath;
            
            WWW www = new WWW(path);
            
            yield return www;
            
            if (www.error != null)
            {
                MelonLogger.Error("Error load: " + www.error);
            }
            else
            {
                SDMAsset<Texture2D> asset = new SDMAsset<Texture2D>(Path.GetFileName(filePath), www.texture);
                AssetData.TEXTURES.Add(asset.name, asset);
                MelonLogger.Msg($"Loaded: {asset.name}");
            }
        }
        
        private IEnumerator loadModels(string filePath)
        {
            string path = "file://" + filePath;
            
            WWW www = new WWW(path);
            
            yield return www;
            
            if (www.error != null)
            {
                MelonLogger.Error("Error load: " + www.error);
            }
            else
            {
                
                SDMAsset<GameObject> asset = new SDMAsset<GameObject>(Path.GetFileName(filePath), www.assetBundle.mainAsset as GameObject);
                AssetData.MESHES.Add(asset.name, asset);
                MelonLogger.Msg($"Loaded: {asset.name}");
            }
        }
        

        protected void sendStatistic()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine($"BUNDLES LOADED: {AssetData.BUNDLES.Count}");
            stringBuilder.AppendLine($"TEXTURES LOADED: {AssetData.TEXTURES.Count}");
            stringBuilder.AppendLine($"MODELS LOADED: {AssetData.MESHES.Count}");
            
            MelonLogger.Msg(stringBuilder);
            
            GlobalEvents.SendOnAssetsLoaded();
        }
        
        private bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp";
        }
        
        private bool IsModelFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".obj" || extension == ".fbx";
        }
        
        
        private bool IsAssetBundleFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".assetbundle" || extension == ".unity3d";
        }        
        private bool IsFontFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".otf";
        }
    }
}