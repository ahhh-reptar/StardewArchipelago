﻿using HarmonyLib;
using StardewArchipelago.Locations.CodeInjections;
using StardewArchipelago.Locations.GingerIsland.Boat;
using StardewArchipelago.Locations.GingerIsland.Parrots;
using StardewArchipelago.Locations.GingerIsland.VolcanoForge;
using StardewArchipelago.Locations.GingerIsland.WalnutRoom;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using KaitoKid.ArchipelagoUtilities.Net.Interfaces;
using KaitoKid.ArchipelagoUtilities.Net;
using StardewArchipelago.Archipelago;

namespace StardewArchipelago.Locations.GingerIsland
{
    public class GingerIslandPatcher
    {
        private readonly StardewArchipelagoClient _archipelago;
        private readonly Harmony _harmony;
        private readonly IParrotReplacer[] _parrotReplacers;

        public GingerIslandPatcher(ILogger logger, IModHelper modHelper, Harmony harmony, StardewArchipelagoClient archipelago, LocationChecker locationChecker)
        {
            _archipelago = archipelago;
            _harmony = harmony;
            GingerIslandInitializer.Initialize(logger, modHelper, _archipelago, locationChecker);
            _parrotReplacers = new IParrotReplacer[]
            {
                new IslandHutInjections(), new IslandNorthInjections(),
                new IslandSouthInjections(), new IslandWestInjections(),
            };
        }

        public void PatchGingerIslandLocations()
        {
            if (_archipelago.SlotData.ExcludeGingerIsland)
            {
                return;
            }

            UnlockWalnutRoomBasedOnApItem();
            ReplaceBoatRepairWithChecks();
            ReplaceParrotsWithChecks();
            ReplaceFieldOfficeWithChecks();
            ReplaceCalderaWithChecks();
        }

        private void UnlockWalnutRoomBasedOnApItem()
        {
            _harmony.Patch(
                original: AccessTools.Method(typeof(IslandWest), nameof(IslandWest.checkAction)),
                prefix: new HarmonyMethod(typeof(WalnutRoomDoorInjection), nameof(WalnutRoomDoorInjection.CheckAction_WalnutRoomDoorBasedOnAPItem_Prefix))
            );
        }

        private void ReplaceBoatRepairWithChecks()
        {
            _harmony.Patch(
                original: AccessTools.Method(typeof(BoatTunnel), nameof(BoatTunnel.checkAction)),
                prefix: new HarmonyMethod(typeof(BoatTunnelInjections), nameof(BoatTunnelInjections.CheckAction_BoatRepairAndUsage_Prefix))
            );

            _harmony.Patch(
                original: AccessTools.Method(typeof(BoatTunnel), nameof(BoatTunnel.answerDialogue)),
                prefix: new HarmonyMethod(typeof(BoatTunnelInjections), nameof(BoatTunnelInjections.AnswerDialogue_BoatRepairAndUsage_Prefix))
            );

            _harmony.Patch(
                original: AccessTools.Method(typeof(BoatTunnel), nameof(BoatTunnel.draw)),
                postfix: new HarmonyMethod(typeof(BoatTunnelInjections), nameof(BoatTunnelInjections.Draw_DrawBoatSectionsBasedOnTasksCompleted_Postfix))
            );
        }

        private void ReplaceParrotsWithChecks()
        {
            foreach (var parrotReplacer in _parrotReplacers)
            {
                parrotReplacer.ReplaceParrots();
            }

            _harmony.Patch(
                original: AccessTools.Method(typeof(IslandLocation), nameof(IslandLocation.checkAction)),
                prefix: new HarmonyMethod(typeof(IslandLocationInjections), nameof(IslandLocationInjections.CheckAction_InteractWithParrots_Prefix))
            );

            _harmony.Patch(
                original: AccessTools.Method(typeof(VolcanoDungeon), nameof(VolcanoDungeon.GenerateContents)),
                postfix: new HarmonyMethod(typeof(VolcanoDungeonInjections), nameof(VolcanoDungeonInjections.GenerateContents_ReplaceParrots_Postfix))
            );

            _harmony.Patch(
                original: AccessTools.Method(typeof(IslandNorth), nameof(IslandNorth.explosionAt)),
                prefix: new HarmonyMethod(typeof(IslandNorthInjections), nameof(IslandNorthInjections.ExplosionAt_CheckProfessorSnailLocation_Prefix))
            );
        }

        private void ReplaceFieldOfficeWithChecks()
        {
            _harmony.Patch(
                original: AccessTools.Method(typeof(Event.DefaultCommands), nameof(Event.DefaultCommands.AddCraftingRecipe)),
                prefix: new HarmonyMethod(typeof(FieldOfficeInjections), nameof(FieldOfficeInjections.AddCraftingRecipe_OstrichIncubator_Prefix))
            );
        }

        private void ReplaceCalderaWithChecks()
        {
            _harmony.Patch(
                original: AccessTools.Method(typeof(Chest), nameof(Chest.checkForAction)),
                prefix: new HarmonyMethod(typeof(CalderaInjections), nameof(CalderaInjections.CheckForAction_CalderaChest_Prefix))
            );
        }
    }
}
