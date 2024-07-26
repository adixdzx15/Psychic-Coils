using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using VREAndroids;
using Verse.AI;
using MentalStateHandler_TryStartMentalState_Patch = VPEPuppeteer.MentalStateHandler_TryStartMentalState_Patch;
using System.Reflection;
using VPEPuppeteer;

namespace Psychic_Coiling_VRE_Addon
{
    [StaticConstructorOnStartup]
    public static class HandlePuppeteerCompatibility
    {
        static HandlePuppeteerCompatibility()
        {
            var harmony = new Harmony("com.Psychic_Coiling_VRE_Addon");

            if (ModsConfig.IsActive("VanillaExpanded.VPE.Puppeteer"))
            {
                Log.Message("Test 1");
                var myPrefixInfo = SymbolExtensions.GetMethodInfo(() => MyPrefix(null, null));
                var originalMethod = AccessTools.Method(typeof(MentalStateHandler_TryStartMentalState_Patch), "Prefix");

                Log.Message(originalMethod.ToString());

                harmony.Patch(originalMethod, prefix: myPrefixInfo);
            }
        }

        public static bool MyPrefix(MentalStateDef stateDef, Pawn __1)
        {
            Log.Message("Test 2");
            if ((stateDef.defName == "VREA_Reformatting" || stateDef.defName == "VREA_SolarFlared") && __1.IsPuppet())
            {
                Log.Message("Test 3");
                return false;
            }
            return true;
        }
    }
}
