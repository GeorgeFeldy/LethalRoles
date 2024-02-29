using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalRoles.Core;
using LethalRoles.Patches;
using LethalRoles.Patches.PlayerController;
using LethalRoles.Roles;
using System.Reflection;
using UnityEngine;

namespace LethalRoles
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]

    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger;

        private void Awake()
        {
            NetcodePatcherSetup();

            RegisterRoles();

            Harmony harmony = new(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Logger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} has been sucessfully loaded!");
        }

        private static void RegisterRoles()
        {
            new GameObject("RoleManager").AddComponent<RoleManager>();
            RoleManager.Instance.RegisterRole<Scout>();
            RoleManager.Instance.RegisterRole<Hauler>();
            RoleManager.Instance.RegisterRole<Cleaner>();
            RoleManager.Instance.RegisterRole<Techie>();
        }

        private void NetcodePatcherSetup()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }
    }

}
