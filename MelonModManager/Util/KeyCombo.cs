using UnityEngine;

namespace MelonModManager.Util {
    /// <summary>
    /// Class for storing keybind informations.
    /// </summary>
    public class KeyCombo {
        /// <summary>
        /// Whether Control key is pressed.
        /// </summary>
        public static bool CtrlPressed => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        /// <summary>
        /// Whether Shift key is pressed.
        /// </summary>
        public static bool ShiftPressed => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        /// <summary>
        /// Whether Alt key is pressed.
        /// </summary>
        public static bool AltPressed => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

        public KeyCombo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyCombo"/> class.
        /// </summary>
        /// <param name="keyCode">Keycode to check.</param>
        /// <param name="ctrl">Whether pressing ctrl key is required.</param>
        /// <param name="shift">Whether pressing alt key is required.</param>
        /// <param name="alt">Whether pressing shift key is required.</param>
        public KeyCombo(KeyCode keyCode, bool ctrl = false, bool shift = false, bool alt = false) {
            KeyCode = keyCode;
            Ctrl = ctrl;
            Shift = shift;
            Alt = alt;
        }

        /// <summary>
        /// Keycode to check from.
        /// </summary>
        public readonly KeyCode KeyCode;

        /// <summary>
        /// Whether pressing ctrl key is required.
        /// </summary>
        public readonly bool Ctrl;

        /// <summary>
        /// Whether pressing shift key is required.
        /// </summary>
        public readonly bool Shift;

        /// <summary>
        /// Whether pressing alt key is required.
        /// </summary>
        public readonly bool Alt;

        /// <summary>
        /// Check if all keys are pressed.
        /// </summary>
        /// <returns></returns>
        public bool CheckKey() {
            return CheckAdditionalKeys() && Input.GetKey(KeyCode);
        }

        /// <summary>
        /// Check if all keys are down.
        /// </summary>
        /// <returns></returns>
        public bool CheckKeyDown() {
            return CheckAdditionalKeys() && Input.GetKeyDown(KeyCode);
        }

        private bool CheckAdditionalKeys() {
            if (Ctrl ^ CtrlPressed) return false;
            if (Shift ^ ShiftPressed) return false;
            if (Alt ^ AltPressed) return false;
            return true;
        }
    }
}
