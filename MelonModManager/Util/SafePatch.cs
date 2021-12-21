using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MelonLoader;
using MelonModManager.Util.Attributes;

namespace MelonModManager.Util {
    /// <summary>
    /// Class for safely patching types that might not exist on other versions.
    /// </summary>
    public class SafePatch {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafePatch"/> class.
        /// </summary>
        /// <param name="patchType">Type of the patching method.</param>
        /// <param name="metadata">Attribute of the patch class.</param>
        /// <param name="harmony">Harmony class to apply patch.</param>
        public SafePatch(Type patchType, SafePatchAttribute metadata, HarmonyLib.Harmony harmony) {
            PatchType = patchType;
            Metadata = metadata;
            Harmony = harmony;
            ClassType = Metadata.Assembly.GetType(Metadata.ClassName);
            PatchTargetMethods = ClassType?.GetMethods(AccessTools.all).Where(m => m.Name.Equals(Metadata.MethodName));
        }

        private HarmonyLib.Harmony Harmony { get; set; }
        private Type ClassType { get; set; }
        private Type PatchType { get; set; }
        private IEnumerable<MethodInfo> PatchTargetMethods { get; set; }
        private readonly string[] HardcodedMethodNames = new[] { "Prefix", "Postfix", "Transpiler", "Finalizer", "ILManipulator" };

        /// <summary>
        /// Attribute of the patch class.
        /// </summary>
        internal SafePatchAttribute Metadata { get; set; }

        /// <summary>
        /// Whether the patch is applied.
        /// </summary>
        internal bool IsEnabled { get; private set; }

        /// <summary>
        /// Checks whether the patch is valid for current game's version.
        /// </summary>
        /// <param name="showDebuggingMessage">
        /// Whether to show debugging message in logs.
        /// </param>
        /// <returns>
        /// Patch's current availability in <see cref="bool"/>.
        /// </returns>
        internal bool IsValidPatch(bool showDebuggingMessage = false) {
            // Check if native types exists first
            bool result =
                ClassType != null &&
                PatchType != null &&
                (PatchTargetMethods?.Count() ?? 0) != 0;

#if DEBUG
            if (showDebuggingMessage) {
                // TODO: Check the game version
                MelonLogger.Msg(
                    $"Patch '{Metadata.Id}' is {(result ? "" : "in")}valid!\n\n" +
                    $"More Details -----------\n" +
                    $"Does patching type exist: {ClassType != null}\n" +
                    $"Does patching target type exist: {PatchType != null}\n" +
                    $"Is there a method to patch: {(PatchTargetMethods?.Count() ?? 0) != 0}");
            }
#endif
            return result;
        }

        /// <summary>
        /// Patch contents in this patch instance.
        /// </summary>
        internal void Patch() {
            if (!IsEnabled) {
                foreach (MethodInfo method in PatchTargetMethods) {
                    List<HarmonyMethod> hardcodedMethods = new List<HarmonyMethod>();

                    foreach (string methodName in HardcodedMethodNames) {
                        MethodInfo patchMethod = AccessTools.Method(PatchType, methodName);
                        hardcodedMethods.Add(patchMethod == null ? null : new HarmonyMethod(patchMethod));
                    }

                    Harmony.Patch(
                        method,
                        hardcodedMethods[0],
                        hardcodedMethods[1],
                        hardcodedMethods[2],
                        hardcodedMethods[3],
                        hardcodedMethods[4]);
                }

                IsEnabled = true;
            }
        }

        /// <summary>
        /// Unpatch contents in this patch instance.
        /// </summary>
        internal void Unpatch() {
            if (IsEnabled) {
                foreach (MethodInfo original in PatchTargetMethods) {
                    foreach (MethodInfo patch in PatchType.GetMethods()) {
                        Harmony.Unpatch(original, patch);
                    }
                }

                IsEnabled = false;
            }
        }
    }
}
