using System;
using HarmonyLib;
using Verse;
using Verse.AI;
using RimWorld;
using VPEPuppeteer;
using VREAndroids;
using MentalStateHandler_TryStartMentalState_Patch = VPEPuppeteer.MentalStateHandler_TryStartMentalState_Patch;

namespace Psychic_Coiling_VRE_Addon
{
    
    public static class PuppeteerHandlerSlave
    {
        public static void CheckForAndHandlePuppeteerMod(Harmony harmony)
        {
            //var harmony = new HarmonyLib.Harmony("com.Psychic_Coiling_VRE_Addon");

            // var myPrefixInfo = SymbolExtensions.GetMethodInfo(() => HandlePuppeteerCompatibilityPatch.MyPrefix( null, null, ref(false)));
            var myPrefixInfo = typeof(HandlePuppeteerCompatibilityPatch).GetMethod(nameof(HandlePuppeteerCompatibilityPatch.MyPrefix));
            var finalizerInfo =
                typeof(HandlePuppeteerCompatibilityPatch).GetMethod(nameof(HandlePuppeteerCompatibilityPatch
                    .ResetFinalizer));
            var VPEInfo = typeof(HandlePuppeteerCompatibilityPatch).GetMethod(nameof(HandlePuppeteerCompatibilityPatch.VPEPrefix));
            var VPEMindInfo = typeof(HandlePuppeteerCompatibilityPatch).GetMethod(nameof(HandlePuppeteerCompatibilityPatch.MindJumpPrefix));
            var patchedName = new string[1];
            patchedName[0] = nameof(VPEPuppeteer.VPEPuppeteerMod);
            var method = new HarmonyMethod(myPrefixInfo, before : patchedName);
            var resetMethod = new HarmonyMethod(finalizerInfo);
            var vpePatcher = new HarmonyMethod(VPEInfo);
            var vpeMethod = AccessTools.Method(typeof(MentalStateHandler_TryStartMentalState_Patch), "Prefix");
            var vpeJumpPatch = new HarmonyMethod(VPEMindInfo);
            var vpeJumpPatched = AccessTools.Method(typeof(Ability_MindJump), nameof(Ability_MindJump.ValidateTarget));
            var originalMethod = AccessTools.Method(typeof(MentalStateHandler), "TryStartMentalState"); ;
            harmony.Patch(originalMethod, prefix: method, finalizer: resetMethod);
            harmony.Patch(vpeMethod, prefix: vpePatcher);
            harmony.Patch(vpeJumpPatched, prefix: vpeJumpPatch);
        }
    }

    [HarmonyPatch(typeof(VREAndroids.VPEPuppeteer_AbilityExtension_TargetValidator_ValidateTarget_Patch))]
    public static class HandlePuppeteerCompatibilityOptions
    {
        [HarmonyPatch(nameof(VREAndroids.VPEPuppeteer_AbilityExtension_TargetValidator_ValidateTarget_Patch.Prefix))]
        [HarmonyPrefix]
        public static bool CheckForCoils(ref bool __result, LocalTargetInfo target)
        {
            // Log.Message("It was called");
            if (!Settings.storedSettings.puppeteerAndroid)
            {
                return true;
            }
            /*if (!(target.Thing is  Pawn a))
            {
                Log.Error("Target is no pawn? what?");
            }
            */

            if (!(target.Thing is Pawn pawn) ||
                !pawn.genes.HasActiveGene(VREAPC_InternalDefs.VREA_Addon_PsychicCoils)) return true;
            __result = true;
            return false;

        }
    }
    public static class HandlePuppeteerCompatibilityPatch
    {
        //static string puppetid = nameof(VPEPuppeteer.VPEPuppeteerMod);
        //[HarmonyBefore([puppetid])]
        private static bool shouldSkipVPE = false;


        public static bool MindJumpPrefix(ref bool __result, Pawn ___pawn, LocalTargetInfo target, bool showMessages)
        
        {
            Log.Message("Logged something");
            if ( Settings.storedSettings.AndroidToAnything || !(target.Thing is Pawn pawn) || (!___pawn.IsAndroid() && ! pawn.IsAndroid()))
            {
                return true;
            }

            if (Settings.storedSettings.AndroidToAndroid)
            {
                
                if (___pawn.IsAndroid() && pawn.IsAndroid())
                {
                    return true;
                }
                if (showMessages)
                {   if (___pawn.IsAndroid())
                        Messages.Message((string) "Can only cast on Android puppet", MessageTypeDefOf.CautionInput);
                    else Messages.Message((string) "Can only cast on Human puppet", MessageTypeDefOf.CautionInput);

                    
                }
                __result = false;
                return false;
            }
            else
            {
                Messages.Message((string) "Androids can not mind jump", MessageTypeDefOf.CautionInput);
                __result = false;
                return false;
            }
        }
        
        public static bool VPEPrefix(ref bool __result)
        {
            if (shouldSkipVPE)
            {
                __result = true;
                shouldSkipVPE = false;
                return false;
            }

            return true;
        }
        //INFO: Necessary since new VPE - Puppeteer method removed stateDef from parameters, which is needed
        // to know whether mental state is valid for android puppets
        public static bool MyPrefix( Pawn ___pawn, MentalStateDef stateDef, ref bool __result)
        {
            // Log.Message("Test 2");
            if ((stateDef.defName == "VREA_Reformatting" || stateDef.defName == "VREA_SolarFlared") &&
                VPEPuppeteer.VPEPUtils.IsPuppet(___pawn))
            {
                // if (___pawn.IsPuppeteer())
                // {
                //     Log.Error("Error: How can pawn be both puppet and puppeteer?");
                // }
                // Log.Message("Test 3");
                shouldSkipVPE = true;
            }

            return true;
        }

        public static void ResetFinalizer()
        {
            shouldSkipVPE = false;
        }

        
    }
}
