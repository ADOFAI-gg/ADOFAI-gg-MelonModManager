using System;
using System.Linq;
using System.Reflection;

namespace MelonModManager.Util.Attributes {
    /// <summary>
    /// Attribute for the type <see cref="SafePatch"/>.
    /// </summary>
    public class SafePatchAttribute : Attribute {
        /// <summary>
        /// Unique ID of the patch.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the class to find specific method from.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Name of the method to patch.
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Assembly to find the type from.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Whether to print debug message when checking the patch's validity.
        /// </summary>
        public bool PrintDebugMessage { get; set; }

        public string[,] GameVersion; 

        /// <summary>
        /// Initializes a new instance of the <see cref="SafePatchAttribute"/> Attribute.
        /// </summary>
        /// <param name="id">Unique ID of the patch.</param>
        /// <param name="className">Name of the class to find specific method from.</param>
        /// <param name="methodName">Name of the method to patch.</param>
        /// <param name="assemblyName">Name of the assembly to find the type from.</param>
        /// <param name="printDebugMessage">Name of the method to patch.</param>
        public SafePatchAttribute(string id, string className, string methodName, string assemblyName = null, bool printDebugMessage = false) {
            Id = id;
            ClassName = className;
            MethodName = methodName;
            Assembly = GetAssemblyByName(assemblyName);
            PrintDebugMessage = printDebugMessage;
        }

        private static Assembly GetAssemblyByName(string name) {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
    }
}