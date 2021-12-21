using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;
using MelonModManager.Core;
using MelonModManager.Core.View;
using MelonModManager.Util;
using UnityEngine.SceneManagement;

namespace MelonModManager {
    /// <summary>
    /// The main runner of the <see cref="MelonModManager"/> plugin.
    /// </summary>
    public class MelonModManager : MelonPlugin {
        /// <summary>
        /// The Settings instance of <see cref="MelonModManagerSettings"/>.
        /// </summary>
        //public MelonModManagerSettings Settings => MelonModManagerSettings.Instance;
        public KeyCombo OpenGUIKeyCombo = new KeyCombo(UnityEngine.KeyCode.F10, true);

        /// <summary>
        /// Instance of <see cref="ModWindow"/>.
        /// </summary>
        public ModWindow Window => ModWindow.Instance;

        /// <summary>
        /// <see cref="ModWindow"/>'s GameObject.
        /// </summary>
        public GameObject WindowObject { get; private set; }

        private bool _windowsEnabled = false;

        /// <summary>
        /// The enabled status of windows.
        /// </summary>
        public bool WindowsEnabled {
            get => _windowsEnabled;
            set {
                scrController.instance.paused = value;

                _windowsEnabled = value;
                
                
                ModWindow.Instance.CanvasObject.SetActive(value);
                var c = scrController.instance.camy.camobj;
                c.GetOrAddComponent<BluredScreenShot>();
                BluredScreenShot.isTakeScreenShot = value;
                if (ModWindow.Instance.SelectMod != null && !value)
                {
                    ModWindow.Instance.SelectMod.SetFocus(false);
                    ModWindow.Instance.SelectMod = null;
                    ModWindow.Instance.ModInfo.gameObject.SetActive(false);
                }
            }
        }
        
        /// <summary>
        /// The modes of inheriting MelonLoaderMod.
        /// </summary>
        public static List<MelonMod> SupportMods = new List<MelonMod>();
        
        public static Dictionary<string, ModStatus> ModToggle = new Dictionary<string, ModStatus>();

        private bool _isSceneFirstLoaded = true;

        /// <summary>
        /// Prepare to load mod informations and setup GUI.
        /// </summary>
        public override void OnApplicationStart()
        {
            AssetLoader.Initialization();
            // Load pre-saved preferences
            //MelonModManagerSettings.Load();
        }

        /// <summary>
        /// Toggle all installed mods.
        /// </summary>
        ///
        public override void OnApplicationLateStart() {
            
            foreach (var mod in MelonHandler.Mods)
            {
                if(!(mod is MelonLoaderMod)) continue;
                SupportMods.Add(mod);
                ModToggle[mod.Info.Name] = ModStatus.Enabled;
                try
                {
                    var mlmod = mod as MelonLoaderMod;
                    mlmod.OnToggle(true);
                }
                catch(Exception e)
                {
                    ModToggle[mod.Info.Name] = ModStatus.Error;
                    MelonLogger.Msg($"[{mod.Info.Name}] {e.Message}\n{e.StackTrace}");
                }

            }


            // Disable UI if user prefers to
            /*
            if (!Settings.OpenGUIOnStartup) {
                WindowObject.SetActive(false);
            }*/
        }


        /// <summary>
        /// Save all prefs and quit the application.
        /// </summary>
        public override void OnApplicationQuit() {
            //MelonModManagerSettings.Save();
        }

   

        /// <summary>
        /// Toggle <see cref="ModWindow"/> compoenent with specific keybind.
        /// </summary>
        /// 
        public override void OnLateUpdate() {

            // When the first scene was loaded
            if (_isSceneFirstLoaded)
            {
                var scene = SceneManager.GetSceneByName("scnNewIntro");
                
                if (scene.isLoaded)
                {
                    _isSceneFirstLoaded = false;
                    Task.Run(() =>
                    {
                        WindowObject = new GameObject("ModWindow_Creater");
                        WindowObject.AddComponent<ModWindow>();
                        WindowsEnabled = false;
                        
                        foreach (var mod in SupportMods)
                        {
                            var mlmod = mod as MelonLoaderMod;
                            if(ModToggle[mod.Info.Name]==ModStatus.Enabled) mlmod?.OnApplicationInitalized();
                        }
                    });
                    
                    // Create GameObject and append Component
                }
            }

            // Close window by specific keycombo
            if (OpenGUIKeyCombo.CheckKeyDown()) {
                WindowsEnabled = !WindowsEnabled;
            }

            // Close window on hitting escape while not writing
            if (WindowsEnabled && Input.GetKeyDown(KeyCode.Escape) && !Window.IsWriting) {
                scrController.instance.paused = true;
                WindowsEnabled = false;
            }
        }
    }
}