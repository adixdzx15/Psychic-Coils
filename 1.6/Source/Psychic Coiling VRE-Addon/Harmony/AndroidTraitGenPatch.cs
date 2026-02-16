using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using VREAndroids;

namespace Psychic_Coiling_VRE_Addon
{
    [HarmonyPatch(typeof(PawnGenerator_GenerateSkills_Patch))]
    [HarmonyPatch(nameof(PawnGenerator_GenerateSkills_Patch.Prefix))]
    public static class InjectRoyalAndroidPatch
    {
        [HarmonyPrefix]
        public static void Prefix(Pawn pawn, PawnGenerationRequest request)
        {
            if (pawn.genes?.GetGene(VREA_DefOf.VREA_SyntheticBody) is Gene_SyntheticBody gene1)
            {
                if (pawn.IsAwakened() && Rand.Chance(Settings.storedSettings.ImperialAndroid))
                    Utils.TryAssignBackstory(pawn, "RoyalAndroid");
            }
        }
    }
    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("GenerateSkills")]
    public static class AndroidTraitGenPatch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, PawnGenerationRequest request)
        {
            
            
            if (pawn.story.Childhood != null && pawn.story.Childhood.defName == "RoyalGuardX24")
            {
                //Log.Error("It happened");
                // request.ProhibitedTraits = request.ProhibitedTraits?.Where((def => def.defName != "PsychicSensitivity"));
                // Log.Error(pawn.story.Childhood.forcedTraits.Count.ToString());
                foreach (BackstoryTrait trait in pawn.story.Childhood.forcedTraits.Where(def =>
                             def.def.defName == "PsychicSensitivity"))
                {
                    pawn.story.traits.GainTrait(new Trait(trait.def, trait.degree, true));
                }
                
            }
        }
    }
}