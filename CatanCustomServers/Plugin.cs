﻿using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using BepInEx.Logging;

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
        internal static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            logger.LogInfo("CatanCustomServers loaded");
            harmony.PatchAll(typeof(Patches.Patches));
        }
    }
}
