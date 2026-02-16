using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Unity;
using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace Psychic_Coiling_VRE_Addon
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new HarmonyLib.Harmony(nameof(Psychic_Coiling_VRE_Addon));
            harmony.PatchAll();
            PuppeteerCompatibility.HandlePuppeteerCompatibility(harmony);
        }
    }

    public class Settings : Mod
    {
        public static StoredSettings storedSettings;
        public Settings(ModContentPack content) : base(content)
        {
            storedSettings = GetSettings<StoredSettings>();
        }

        public override string SettingsCategory()
        {
            return "Psychic Coiling";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var list = new Listing_Standard();
            list.Begin(inRect);
            Text.Font = GameFont.Small;
            //TODO: Does not do anything yet
            if (ModsConfig.IsActive("VanillaExpanded.VPE.Puppeteer"))
            {
                list.Label("Compatibility with Vanilla Psycasts Expanded - Puppeteer:");
                // if (storedSettings.puppeteerAndroid)
                // {
                //     storedSettings.AndroidToPuppet = true;
                // }
                list.CheckboxLabeled("Androids can swap with android puppets", ref storedSettings.AndroidToAndroid);
                if (storedSettings.AndroidToAndroid)
                {
                    list.CheckboxLabeled("\t Androids puppets and puppeteers can swap (and be swapped) with any pawn", ref storedSettings.AndroidToAnything);
                }
                else
                {
                    Settings.storedSettings.AndroidToAnything = false;
                }
                //if (!storedSettings.AndroidToPuppet)
                //{
                //    storedSettings.puppeteerAndroid = false;
                //}
                list.CheckboxLabeled("Non-psydeaf androids can be affected by puppeteer psycasts", ref storedSettings.puppeteerAndroid);
                
                

            }
            list.GapLine();
            list.Label("Android Generation");
            storedSettings.ImperialAndroid = list.SliderLabeled(
                "Additional chance of a pawn generating with an imperial backstory (allows certain psycasts in Vanilla Psycasts Expanded)",
                storedSettings.ImperialAndroid, 0, 1, tooltip: (100 * storedSettings.ImperialAndroid).ToString() + "%");
            list.End();
        }
    }

    public class StoredSettings : ModSettings
    {
        public bool puppeteerAndroid;
        public bool AndroidToAnything;
        public bool AndroidToAndroid = true;

        public float ImperialAndroid;
        //public bool AwakenedPsychicCoils;
        //public bool AwakenedSubroutines;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref puppeteerAndroid, "puppeteerAndroid", true);
            Scribe_Values.Look(ref AndroidToAndroid, "androidToPuppet", true);
            Scribe_Values.Look(ref AndroidToAnything, "androidToAnything", false);
            Scribe_Values.Look<float>(ref ImperialAndroid, "imperialAndroid",0f);
        }
    }
}