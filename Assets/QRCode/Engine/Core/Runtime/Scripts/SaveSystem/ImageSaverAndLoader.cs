﻿namespace QRCode.Engine.Core.SaveSystem
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Toolbox;
    using Engine.Debugging;
    using UnityEngine;
    using Constants = Toolbox.Constants;

    public class ImageSaver
    {
        public async Task SaveImage(Texture2D texture, string fileName, string extension)
        {
            var itemBGBytes = texture.EncodeToPNG();
            var saveSettings = SaveServiceSettings.Instance;

            var fullPath = saveSettings.FullPath + "/" + fileName + extension;
            var task = File.WriteAllBytesAsync(fullPath, itemBGBytes);
            
            try
            {
                await task;
            }
            catch (Exception e)
            {
                QRDebug.DebugError(Constants.DebuggingChannels.SaveManager, e);
                throw;
            }
        }
    }

    public class ImageLoader
    {
        public async Task<Texture2D> LoadImage(string fullPath)
        {
            byte[] bytes = null;

            try
            {
                var task = File.ReadAllBytesAsync(fullPath);
                bytes = await task;
            }
            catch (Exception e)
            {
                QRDebug.DebugError(Constants.DebuggingChannels.SaveManager, e);

                var saveSetting = SaveServiceSettings.Instance;
                return saveSetting.NotLoadedTexture;
            }

            var image = new Texture2D(0,0);
            image.LoadImage(bytes);
            return image;
        }
    }
}