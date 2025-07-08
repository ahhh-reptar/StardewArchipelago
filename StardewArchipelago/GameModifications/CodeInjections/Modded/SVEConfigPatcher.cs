using HarmonyLib;
using KaitoKid.ArchipelagoUtilities.Net.Interfaces;
using StardewArchipelago.Integrations.GenericModConfigMenu;
using StardewModdingAPI;
using System.Security.Cryptography.X509Certificates;

namespace StardewArchipelago.GameModifications.CodeInjections.Modded
{
    public class SVEConfigPatcher
    //Namespace SVE StardewValleyExpandedCP
    {
        private const string SVE_NAMESPACE = "StardewValleyExpandedCP";
        private const string CP_NAMESPACE = "ContentPatcher";
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
            var contentPatcherEntryType = AccessTools.TypeByName($"{CP_NAMESPACE}.modentry");
            var sveModEntryType = AccessTools.TypeByName($"{CP_NAMESPACE}.modentry");
            var sveConfigType = AccessTools.TypeByName($"{CP_NAMESPACE}.Framework.ConfigField.{SVE_NAMESPACE}.ContentConfig");

            // internal static Config Config;
            var configField = AccessTools.Field(sveModEntryType, "Config");
            var config = configField.GetValue(null);

            ApplyBool(config, "GaldoranGemMuseumMoveable", GALDORAN_GEM_MUSEUMSANITY);
        }

        private void ApplyBool(object config, string settingName, bool boolValue)
        {
            //bool truthField = _modHelper.Reflection.GetField<bool>(config, settingName);
                
            IReflectedField<bool> truthField = _modHelper.Reflection.GetField<bool>((config, settingName), "truthField");
            truthField.SetValue(boolValue);
            //bool truthField = _modHelper.Reflection.GetField<bool>(config, settingName);
            //object truthField = _modHelper.Reflection.GetField<bool>(config, settingName);
            //if (truthField = boolValue)
            //truthField.Equals(boolValue);
            //if ((bool)(truthField == false) & (bool)(boolValue = false)) ;
            //{
            //    truthField = !truthField;
            //}
            //truthField.SetValue(boolValue);
        }
    }
}