using UnityEngine;

namespace MelonModManager.Core {
    /// <summary>
    /// Class for storing <see cref="ModWindow"/>'s UI Styles.
    /// </summary>
    public static class ModWindowStyles {
        /// <summary>
        /// .
        /// </summary>
        public static class Colors
        {

            public static Color selectedLabelColor = new Color(1, 1, 1, 1);
            public static Color unselectedLabelColor = new Color(0.7138f, 0.7138f, 0.7138f, 0.884f);
            public static Color unselectedBorderColor = new Color(0.7138f, 0.7138f, 0.7138f, 0.416f);
            public static Color selectedFillColor = new Color(0.2075f, 0.2075f, 0.2075f, 0.36f);
            public static Color unselectedFillColor = new Color(0, 0, 0, 0.43f);
            

            // TODO: Replace hex string to actual color instances and replace all <color> tags
            public static string SelectText = "#bb86fc";
            public static string NotSelectText = "#969696";
            public static string OnlineText = "#43b581";
            public static string OfflineText = "#747f8d";
            public static string WarningText = "#faa61a";
            public static string ErrorText = "#f04747";
        }
    }
}
