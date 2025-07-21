using System.Collections.Generic;

namespace StardewArchipelago.Constants.Modded
{
    public static class ModNameAliases
    {
        public static List<List<string>> ItemNameAliasGroups = new()
        { // I left one from the original file for the sake of reference
         //   new List<string> { "L. Goat Milk", "Large Goat Milk", "Goat Milk (Large)" },
        };

        public static Dictionary<string, string> CookingRecipeNameAliases = new()
        { // If this section becomes required, just look how it's done in the crafting section for reference
        };

        public static Dictionary<string, string> CraftingRecipeNameAliases = new()
        {
            { "FlashShifter.StardewValleyExpandedCP_Marsh_Tonic", "Marsh Tonic" },
        };

        public static Dictionary<string, string> NPCNameAliases = new()
        {  // Need to move all strings referencing the NPCNameAliases to point here and remove it from the original file.
            { "MisterGinger", "Mr. Ginger" },
            { "MarlonFay", "Marlon" },
            { "GuntherSilvian", "Gunther" },
            { "MorrisTod", "Morris" },
            { "Aimon111.WitchSwampOverhaulCP_GoblinZic", "Zic" },
            { "ichortower.HatMouseLacey_Lacey", "Lacey" },
            { "SPB_Alec", "Alec" },
        };
    }
}
