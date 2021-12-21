using System;
using System.Collections.Generic;

namespace MelonModManager.Core {
    /// <summary>
    /// Class that contains mod's settings.
    /// </summary>
    public abstract class ModSetting {
        /// <summary>
        /// Returns instance of given type. 
        /// </summary>
        /// <typeparam name="T">Type of setting to get.</typeparam>
        /*
        public T GetSetting<T>() where T : ModSetting, new() {
            var settings = MelonModManagerSettings.Instance.Settings;
            
            if (settings.TryGetValue(typeof(T), out var value)) return (T) value;
            settings[typeof(T)] = new T();
            return (T) settings[typeof(T)];
        }*/
    }
}