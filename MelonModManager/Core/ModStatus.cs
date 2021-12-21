using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelonModManager.Core {
    /// <summary>
    /// Status of installed mods.
    /// </summary>
    public enum ModStatus {
        Enabled, 
        Disabled, 
        Warning, 
        Error
    }
}
