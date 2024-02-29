using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalRoles.Managers;
using LethalRoles.Patches;
using LethalRoles.Patches.PlayerController;
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

            LoadComponents();

            Harmony harmony = new(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Logger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_NAME);
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} has been sucessfully loaded!");
        }

        private static void LoadComponents()
        {
            new GameObject("RolesManager").AddComponent<RoleManager>();
            new GameObject("RolePowerManager").AddComponent<PlayerPowerManager>();
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
