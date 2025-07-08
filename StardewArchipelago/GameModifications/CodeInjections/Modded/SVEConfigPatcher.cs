using HarmonyLib;
using KaitoKid.ArchipelagoUtilities.Net.Interfaces;
using StardewArchipelago.Integrations.GenericModConfigMenu;
using StardewModdingAPI;

namespace StardewArchipelago.GameModifications.CodeInjections.Modded
{
    public class SVEConfigPatcher
    //Namespace SVE StardewValleyExpandedCP
    {
        private const string SVE_NAMESPACE = "StardewValleyExpandedCP";
        private const bool GALDORAN_GEM_MUSEUMSANITY = true;

        private ILogger _logger;
        private readonly IModHelper _modHelper;

        public SVEConfigPatcher(ILogger logger, IModHelper modHelper)
        {
            _logger = logger;
            _modHelper = modHelper;
        }

        public void PatchSVEConfig()
        {
            //SVE Mod and Config entry
            var sveModEntryType = AccessTools.TypeByName($"{SVE_NAMESPACE}.modentry");
            var sveConfigType = AccessTools.TypeByName($"{SVE_NAMESPACE}.modentry");

            // internal static Config Config;
            var configField = AccessTools.Field(sveModEntryType, "Config");
            var config = configField.GetValue(null);

            ApplyBool(config, "GaldoranGemMuseumMoveable", GALDORAN_GEM_MUSEUMSANITY);
        }
        private void ApplyBool(object config, string settingName, bool BoolValue)
        {
            var truthField = _modHelper.Reflection.GetField<bool>(config, settingName);
            truthField.SetValue(BoolValue);
        }
    }
}