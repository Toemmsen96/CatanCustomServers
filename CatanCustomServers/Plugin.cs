using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BepInEx.Logging;
using Nakama.Core;

namespace CatanCustomServers
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class CatanCustomServers : BaseUnityPlugin
    {
        private const string modGUID = "Toemmsen96.CatanCustomServers";
        private const string modName = "CatanCustomServers";
        private const string modVersion = "0.0.1";
        private readonly Harmony harmony = new Harmony(modGUID);
        private static CatanCustomServers instance;
        internal static CustomClient customClient;
        internal static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            harmony.PatchAll(typeof(Patches.Patches));
            customClient = new CustomClient();
            logger.LogInfo("CatanCustomServers loaded");
            logger.LogWarning($"Dev config:\nHost:{NakamaSettings.NakamaConfigDEV.Host}\nPort{NakamaSettings.NakamaConfigDEV.Port}\nScheme:{NakamaSettings.NakamaConfigDEV.Scheme}\nServerKey{NakamaSettings.NakamaConfigDEV.ServerKey}");

        }
        private void OnDestroy()
        {
            customClient.CloseConnection();
            harmony.UnpatchSelf();

        }
    }
}
