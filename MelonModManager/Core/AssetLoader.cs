using System.Reflection;
using MelonLoader;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MelonModManager.Core
{
    public static class AssetLoader
    {
        private static AssetBundle _assetBundle;
        public static GameObject ModSettingCanvas;
        public static GameObject ModTab;
        public static AudioClip Sque;

        /// <summary>
        /// 
        /// </summary>
        public static void Initialization()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("MelonModManager.Core.View.mmm.assets");
            
            _assetBundle = AssetBundle.LoadFromStream(stream);
            ModSettingCanvas = _assetBundle.LoadAsset<GameObject>("ModSetting Canvas");
            ModTab = _assetBundle.LoadAsset<GameObject>("ModTab");
            Sque = _assetBundle.LoadAsset<AudioClip>("sndMenuSquelch");
            
        }
    }
}