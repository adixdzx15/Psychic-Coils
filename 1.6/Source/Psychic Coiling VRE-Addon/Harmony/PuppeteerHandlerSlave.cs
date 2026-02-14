using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Psychic_Coiling_VRE_Addon
{
    
    public static class PuppeteerHandlerSlave
    {
        public static void CheckForAndHandlePuppeteerMod(Harmony harmony)
        {
            //var harmony = new HarmonyLib.Harmony("com.Psychic_Coiling_VRE_Addon");

            // var myPrefixInfo = SymbolExtensions.GetMethodInfo(() => HandlePuppeteerCompatibilityPatch.MyPrefix( null, null, ref(false)));
            var myPrefixInfo = typeof(HandlePuppeteerCompatibilityPatch).GetMethod(nameof(HandlePuppeteerCompatibilityPatch.MyPrefix));
            var patchedName = new string[1];
            patchedName[0] = nameof(VPEPuppeteer.VPEPuppeteerMod);
            var method = new HarmonyMethod(myPrefixInfo, before : patchedName);
            var originalMethod = AccessTools.Method(typeof(MentalStateHandler), "TryStartMentalState"); ;
            harmony.Patch(originalMethod, prefix: method);
        }
    }

    public static class HandlePuppeteerCompatibilityPatch
    {
        //static string puppetid = nameof(VPEPuppeteer.VPEPuppeteerMod);
        //[HarmonyBefore([puppetid])]
        public static bool MyPrefix( Pawn ___pawn, MentalStateDef stateDef, ref bool __result)
        {
            Log.Message("Test 2");
            if ((stateDef.defName == "VREA_Reformatting" || stateDef.defName == "VREA_SolarFlared") &&
                VPEPuppeteer.VPEPUtils.IsPuppet(___pawn))
            {
                Log.Message("Test 3");
                __result = true;
                return false;
            }

            return true;
        }
    }
}
