﻿using Harmony;
using LmpClient.Extensions;
using LmpClient.Systems.VesselPositionSys;
using LmpCommon.Enums;

// ReSharper disable All

namespace LmpClient.Harmony
{
    /// <summary>
    /// This harmony patch is intended to avoid run the part die in vessels that are immortal
    /// </summary>
    [HarmonyPatch(typeof(Part))]
    [HarmonyPatch("Die")]
    public class Part_Die
    {
        [HarmonyPrefix]
        private static bool PrefixDie(Part __instance)
        {
            if (MainSystem.NetworkState < ClientState.Connected || !__instance.vessel) return true;

            if (__instance.vessel.IsImmortal())
                return false;

            //The vessel have updates queued as it was left there by a player in a future subspace
            if (VesselPositionSystem.Singleton.VesselHavePositionUpdatesQueued(__instance.vessel.id))
                return false;

            return true;
        }
    }
}
