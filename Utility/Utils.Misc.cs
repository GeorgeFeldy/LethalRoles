using BepInEx.Logging;
using GameNetcodeStuff;
using LethalRoles.Core;
using System;
using System.Reflection;

namespace LethalRoles.Utility
{
    public static partial class Utils
    {
        public static Role GetRole(this PlayerControllerB player)
        {
            return RoleManager.Instance.GetRole(player);
        }

        public static PlayerControllerB FindPlayerById(ulong clientId)
        {
            foreach (var player in StartOfRound.Instance.allPlayerScripts)
            {
                if (player.actualClientId == clientId)
                    return player;
            }
            return null;
        }

        /// <summary> Invokes a private method on a given object. </summary>
		/// <param name="instance"> The object on which to invoke the method. If the method is static, pass the type. </param>
		/// <param name="methodName"> Name of the method to invoke. </param>
		/// <param name="parameters"> Parameters to pass to the method. </param>
		/// <returns> Result of the method invocation. </returns>
		public static object InvokeMethod(object instance, string methodName, params object[] parameters)
        {
            if (instance == null && !(parameters?.Length > 0 && parameters[0] is Type))
                throw new ArgumentNullException(nameof(instance), "The instance should not be null, unless you're invoking a static method and the first parameter in 'parameters' is of type 'Type'.");

            Type type = instance as Type ?? instance.GetType();
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            return methodInfo == null
                ? throw new ArgumentException($"Method '{methodName}' not found on type {type.FullName}.", nameof(methodName))
                : methodInfo.Invoke(instance is Type ? null : instance, parameters);
        }
    }
}
