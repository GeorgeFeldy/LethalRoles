using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace LethalRoles
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            NetcodePatcherSetup();
            PatchHooks();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} has been loaded!");
        }

        private void PatchHooks()
        {
            Harmony.CreateAndPatchAll(typeof(TerminalHooks));
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
