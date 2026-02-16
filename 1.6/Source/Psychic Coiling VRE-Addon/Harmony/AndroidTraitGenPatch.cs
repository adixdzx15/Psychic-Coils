using System.Linq;
using HarmonyLib;
using Verse;
using VREAndroids;

namespace Psychic_Coiling_VRE_Addon
{
    [HarmonyPatch(typeof(PawnGenerator_GenerateTraits_Patch))]
    [HarmonyPatch(nameof(PawnGenerator_GenerateTraits_Patch.Prefix))]
    public static class AndroidTraitGenPatch
    {
        public static void Postfix(Pawn pawn, PawnGenerationRequest request)
        {
            if (pawn.story.Childhood != null && pawn.story.Childhood.defName == "RoyalGuardX24")
            {
                request.ProhibitedTraits = request.ProhibitedTraits.Where((def => def.defName != "PsychicSensitivity"));
            }
        }
    }
}