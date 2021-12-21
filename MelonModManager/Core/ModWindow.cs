using MelonLoader;
using MelonModManager.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonModManager.Core.View;
using UnityEngine;
using UnityEngine.UI;

namespace MelonModManager.Core
{
    public class ModWindow : MonoBehaviour {
        private static ModWindow instance;

        /// <summary>
        /// Instance of <see cref="ModWindow"/>.
        /// </summary>
        public static ModWindow Instance {
            get => instance;
        }

        /// <summary>
        /// Whether the player is writing in internal UI.
        /// </summary>
        public bool IsWriting { get; private set; }
        
        /// <summary>
        /// Currently selected mod.
        /// </summary>
        //private MelonMod SelectedMod;

        public ModTab SelectMod;
        public GameObject CanvasObject;
        public Transform Content;
        public ModInfo ModInfo;
        public List<ModTab> ModTabs = new List<ModTab>();
        public AudioSource Audio;


        private void OnDestroy()
        {
            Destroy(CanvasObject);
        }

        private void Awake() {
            if (instance) {
                Destroy(gameObject);
                return;
            }

            instance = this;

            CanvasObject = Instantiate(AssetLoader.ModSettingCanvas);
            Audio = gameObject.AddComponent<AudioSource>();
            Audio.clip = AssetLoader.Sque;

            var settingMenu = CanvasObject.transform.Find("ModSettingView").transform.Find("SettingsMenu");
            ModInfo = settingMenu.transform.Find("ModInfo").gameObject.AddComponent<ModInfo>();
            ModInfo.gameObject.SetActive(false);

            Content = settingMenu.transform.Find("settingsContainer").transform.Find("Viewport").transform.Find("Content");
            
            foreach (var mod in MelonHandler.Mods)
            {
                var modTabObject = Instantiate(AssetLoader.ModTab, Content, true);
                modTabObject.name = mod.Info.Name;
                
                var modTab = modTabObject.AddComponent<ModTab>();
                modTab.Name.text = mod.Info.Name;
                modTabObject.transform.localScale = new Vector3(1, 1, 1);
                ModTabs.Add(modTab);
                if (MelonModManager.SupportMods.Contains(mod))
                {
                    var status = MelonModManager.ModToggle[mod.Info.Name];
                    modTab.ValueName.text = status == ModStatus.Enabled ? "On" : (status==ModStatus.Disabled ? "Off" : "<color=#ff0000>!! </color>Off");
                }
                else
                {
                    modTab.NotMelonLoaderMod();
                }
            }

            DontDestroyOnLoad(CanvasObject);
            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            // TODO: Write code to display the mod window
        }
    }
}
