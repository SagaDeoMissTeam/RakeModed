using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using UnityEngine;

namespace RakeModed.utils
{
    public class AssetLoader : MonoBehaviour
    {
        
        protected IEnumerator Start()
        {
            AssetData.reload();
            
            string[] files = Directory.GetFiles(ModConstants.texturesFolder);
            foreach (string file in files)
            {
                if (IsImageFile(file))
                {
                    yield return StartCoroutine(loadTextures(file));
                }
            }
            
            files = Directory.GetFiles(ModConstants.meshFolder);
            foreach (string file in files)
            {
                if (IsModelFile(file))
                {
                    yield return StartCoroutine(loadModels(file));
                }
            }
        }

        private IEnumerator loadTextures(string filePath)
        {
            string path = "file://" + filePath;
            
            WWW www = new WWW(path);
            
            yield return www;
            
            if (www.error != null)
            {
                MelonLogger.Error("Ошибка загрузки модели: " + www.error);
            }
            else
            {
                
                AssetData.MESHES.Add(www.assetBundle.mainAsset.name, www.assetBundle.mainAsset as GameObject);
                MelonLogger.Msg($"Загруженна модели: {www.assetBundle.mainAsset.name}");
            }
        }
        
        private IEnumerator loadModels(string filePath)
        {
            string path = "file://" + filePath;
            
            WWW www = new WWW(path);
            
            yield return www;
            
            if (www.error != null)
            {
                MelonLogger.Error("Ошибка загрузки текстуры: " + www.error);
            }
            else
            {
                AssetData.TEXTURES.Add(www.texture.name, www.texture);
                MelonLogger.Msg($"Загруженна текстура: {www.texture.name}");
            }
        }
        
        private bool IsImageFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".bmp";
        }
        
        private bool IsModelFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension == ".obj" || extension == ".fbx"; // Добавьте другие расширения по необходимости
        }
    }
}