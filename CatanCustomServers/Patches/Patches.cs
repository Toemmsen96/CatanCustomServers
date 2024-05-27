using BepInEx.Logging;
using Common.GameLogic.Communication;
using Common.GameLogic.Model;
using Common.Net;
using Common.Screens;
using Common.Screens.Chat;
using Common.Screens.Overlay;
using Common.Shop;
using Common.Unity;
using GameHub;
using GameHub.Screens;
using GameHub.States;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using HarmonyLib;
using System.Collections.Generic;
using Catan.Unity.Components;
using Nakama.Core;
using System;
namespace CatanCustomServers.Patches
{
    internal class Patches
    {
        internal static NakamaConfig CustomNakamaConfig = NakamaSettings.NakamaConfigDEV;


        [HarmonyPatch(typeof(GameSparksConnectionTransport), "SendPlayAction")]
        [HarmonyPostfix]
        private static void LogPlayAction(ref CommonGameState gameState, ref CommonGameActionState action)
        {
            CatanCustomServers.logger.LogInfo($"Sending GSCT play action: {action}\nand GameState: {gameState}");
        }

        [HarmonyPatch(typeof(LocalPlayerConnectionTransport), "SendPlayAction")]
        [HarmonyPrefix]
        private static void LogPlayAction2(ref CommonGameState gameState, ref CommonGameActionState action)
        {
            CatanCustomServers.logger.LogInfo($"Sending LPCT play action: {action}\nand GameState: {gameState}");
           

        }

        [HarmonyPatch(typeof(AIGameSparksConnectionTransport), "SendPlayAction")]
        [HarmonyPostfix]
        private static void LogPlayAction3(ref CommonGameState gameState, ref CommonGameActionState action)
        {
            CatanCustomServers.logger.LogInfo($"Sending AIGSCT play action: {action}\n\nand GameState: {gameState}");
        }
        [HarmonyPatch(typeof(NakamaConnection))]
        [HarmonyPatch(".ctor", new System.Type[] { typeof(NakamaInstance), typeof(INakamaPlatform), typeof(NakamaConfig) })][HarmonyPatch(typeof(NakamaConnection), MethodType.Constructor)]
        [HarmonyPrefix]
        private static void LogIntoDEV(ref NakamaConfig config)
        {
            config = CustomNakamaConfig;
            CatanCustomServers.logger.LogInfo($"NakamaConnection initialized: Host: {config.Host} Port: {config.Port} Key: {config.ServerKey}");
        }

        [HarmonyPatch(typeof(NetworkResponseBase), "HasError")]
        [HarmonyPostfix]
        private static void LogError(ref bool __result, ref NetworkResponseBase __instance)
        {
            if (__result)
            {
                CatanCustomServers.logger.LogError($"NetworkResponseBase error: {__instance.Error.Description}");
            }
        }

        [HarmonyPatch(typeof(NakamaConnector), "LoginToNakamaWithCredentials")]
        [HarmonyPrefix]
        private static void LogLogin(ref string email, ref string password, ref Action<NetworkResponseBase> authenticationCallBack)
        {
            CatanCustomServers.logger.LogInfo($"Logging into Nakama with username: {email} and password: {password} Callback {authenticationCallBack.ToString()}");
        }

        [HarmonyPatch(typeof(ChatCommandController), "HandleMessage")]
        [HarmonyPrefix]
        private static bool SendCommand(ref string message, ref bool __result)
        {
            string[] args = message.Split(' ');
            if (args[0] == "/send")
            {
                CatanCustomServers.logger.LogInfo("Sending message to server: " + args[1]);
                CatanCustomServers.customClient.SendMessageToServer(args[1]);
                __result = true;
                return true;
            }
            if (args[0] == "/connect")
            {
                CatanCustomServers.logger.LogInfo("Connecting to server...");
                CatanCustomServers.customClient.ConnectToServer();
                __result = true;
                return true;
            }
            if (args[0] == "/disconnect")
            {
                CatanCustomServers.logger.LogInfo("Disconnecting from server...");
                CatanCustomServers.customClient.CloseConnection();
                __result = true;
                return true;
            }

            if (args[0] == "/setenv")
            {
                string env = args[1];
                switch (env){
                    case "DEV":
                        CustomNakamaConfig = NakamaSettings.NakamaConfigDEV;
                        break;
                    case "PROD":
                        CustomNakamaConfig = NakamaSettings.NakamaConfigPROD;
                        break;
                    case "LOCAL":
                        CustomNakamaConfig = NakamaSettings.NakamaConfigLOCAL;
                        break;
                    case "QA":
                        CustomNakamaConfig = NakamaSettings.NakamaConfigQA;
                        break;
                    default:
                        CatanCustomServers.logger.LogWarning("Invalid environment. Use DEV, PROD, LOCAL or QA.");
                        __result = false;
                        return true;
                    }
                    CatanCustomServers.logger.LogInfo($"Changed environment to {env}");
                __result = true;
                return true;
            }
            __result = false;
            return true;
        }
    }
}
