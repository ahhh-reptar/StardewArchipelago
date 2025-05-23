﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using KaitoKid.ArchipelagoUtilities.Net.Interfaces;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewArchipelago.Constants.Vanilla;

namespace StardewArchipelago.Items.Traps
{
    public class DebrisSpawner
    {
        private const string TWIG_1 = "294";
        private const string TWIG_2 = "295";
        private const string STONE_1 = "343";
        private const string STONE_2 = "450";
        private const string WEEDS = "750";

        private ILogger _logger;

        public DebrisSpawner(ILogger logger)
        {
            _logger = logger;
        }

        public void CreateDebris(int amount)
        {
            var farm = Game1.getFarm();
            var hasGoldClock = farm.isBuildingConstructed("Gold Clock");
            var currentLocation = Game1.player.currentLocation;
            var locations = new List<GameLocation>();
            locations.Add(farm);
            if (currentLocation != farm)
            {
                locations.Add(currentLocation);
            }

            if (hasGoldClock)
            {
                amount /= 2;
            }

            var amountOfDebrisPerLocation = amount / locations.Count;
            foreach (var gameLocation in locations)
            {
                if (hasGoldClock && gameLocation == farm)
                {
                    SpawnDebris(gameLocation, amountOfDebrisPerLocation / 2);
                }
                else
                {
                    SpawnDebris(gameLocation, amountOfDebrisPerLocation);
                }
            }
        }

        public void SpawnDebris(GameLocation location, int amount)
        {
            for (var i = 0; i < amount; ++i)
            {
                var tile = new Vector2(Game1.random.Next(location.map.Layers[0].LayerWidth), Game1.random.Next(location.map.Layers[0].LayerHeight));
                var noSpawn = location.doesTileHaveProperty((int)tile.X, (int)tile.Y, "NoSpawn", "Back") != null;
                var wood = location.doesTileHaveProperty((int)tile.X, (int)tile.Y, "Type", "Back") == "Wood";
                var tileIsClear = location.CanItemBePlacedHere(tile) && !location.objects.ContainsKey(tile) && !location.terrainFeatures.ContainsKey(tile);
                if (noSpawn || wood || !tileIsClear)
                {
                    continue;
                }

                if (Game1.random.NextDouble() < 0.05)
                {
                    SpawnRandomTree(location, tile);
                    continue;
                }

                var itemToSpawn = ChooseRandomDebris(location);
                location.objects.Add(tile, new Object(itemToSpawn, 1));
            }
        }

        private static void SpawnRandomTree(GameLocation location, Vector2 tile)
        {
            location.terrainFeatures.Add(tile, new Tree((Game1.random.Next(3) + 1).ToString(), Game1.random.Next(3)));
        }

        private static string ChooseRandomDebris(GameLocation location)
        {
            var typeRoll = Game1.random.NextDouble();
            if (typeRoll < 0.33)
            {
                return Game1.random.NextDouble() < 0.5 ? TWIG_1 : TWIG_2;
            }
            if (typeRoll < 0.67)
            {
                return Game1.random.NextDouble() < 0.5 ? STONE_1 : STONE_2;
            }

            return QualifiedItemIds.UnqualifyId(GameLocation.getWeedForSeason(Game1.random, location.GetSeason()));
        }
    }
}
