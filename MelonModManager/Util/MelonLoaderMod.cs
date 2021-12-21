using System;
using MelonLoader;
using MelonModManager.Util.Attributes;

namespace MelonModManager.Util
{
    public class MelonLoaderMod : MelonMod
    {
        public string Description
        {
            get
            {
                var attribute =
                    Assembly.GetCustomAttributes(typeof(MelonDescriptionAttribute), false)[0] as
                        MelonDescriptionAttribute;
                return attribute?.Description;
            }
        }

        
        public virtual void OnApplicationInitalized() { }
        public virtual void OnToggle(bool result) { }
        public virtual void OnSettingGUI() { }
    }
    
}