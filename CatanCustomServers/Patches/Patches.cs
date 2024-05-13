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
namespace CatanCustomServers.Patches
{
    internal class Patches
    {
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

    }
}
