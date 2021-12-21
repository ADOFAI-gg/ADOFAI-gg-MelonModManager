using System;
using System.Collections.Generic;
using MelonLoader;
using MelonModManager.Util;
using UnityEngine.Events;

namespace MelonModManager.Core {
    /// <summary>
    /// Class for storing <see cref="MelonModManager"/>'s Settings.
    /// </summary>
    public class MelonModManagerSettings {
        /// <summary>
        /// Saved status of mods being enabled.
        /// </summary>
        public Dictionary<string, bool> EnabledMods = new Dictionary<string, bool>();

        /// <summary>
        /// Key Combo for opening <see cref="ModWindow"/>.
        /// </summary>
        public KeyCombo OpenGUIKeyCombo = new KeyCombo(UnityEngine.KeyCode.F10, true);

        /// <summary>
        /// Whether to open the UI automatically after the game is loaded.
        /// </summary>
        public bool OpenGUIOnStartup = true;

        private static MelonModManagerSettings _instance;

        /// <summary>
        /// Instance of <see cref="MelonModManagerSettings"/>.
        /// </summary>
        public static MelonModManagerSettings Instance {
            get {
                if (_instance != null) return _instance;
                Load();
                return _instance;
            }
        }

        /// <summary>
        /// An Event that is invoked after Preferences is loaded.
        /// </summary>
        public static UnityEvent OnAfterLoad = new UnityEvent();
        
        /// <summary>
        /// An Event that is invoked before Preferences is saved.
        /// </summary>
        public static UnityEvent OnBeforeSave = new UnityEvent();

        private static readonly MelonPreferences_Category PrefCategory = MelonPreferences.CreateCategory("MelonModManagerSettings");
        private static readonly MelonPreferences_Entry<MelonModManagerSettings> PrefEntry = PrefCategory.CreateEntry("MelonModManagerSettings", new MelonModManagerSettings());

        /// <summary>
        /// Load MLModManager settings from Preferences.
        /// </summary>
        public static void Load() {
            PrefCategory.LoadFromFile();
            _instance = PrefEntry.Value;
            OnAfterLoad.Invoke();
        }

        /// <summary>
        /// Save MLModManager settings to Preferences.
        /// </summary>
        public static void Save() {
            OnBeforeSave.Invoke();
            PrefEntry.Value = _instance;
            PrefCategory.SaveToFile();
        }
    }
}
