using System;

namespace MelonModManager.Util.Attributes {
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class MelonDescriptionAttribute : Attribute
    {
        public string Description;

        public MelonDescriptionAttribute(string description = null) {
            Description = description;
        }

    }
}