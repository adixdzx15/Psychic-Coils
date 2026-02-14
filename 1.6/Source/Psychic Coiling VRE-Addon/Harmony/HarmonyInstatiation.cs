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
            var harmony = new HarmonyLib.Harmony("com.Psychic_Coiling_VRE_Addon");
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
                if (storedSettings.puppeteerAndroid)
                {
                    storedSettings.AndroidToPuppet = true;
                }
                list.CheckboxLabeled("Androids can swap with Puppets", ref storedSettings.AndroidToPuppet);
                if (!storedSettings.AndroidToPuppet)
                {
                    storedSettings.puppeteerAndroid = false;
                }
                list.CheckboxLabeled("\tAndroids can directly become Puppets", ref storedSettings.puppeteerAndroid);

            }
            list.End();
        }
    }

    public class StoredSettings : ModSettings
    {
        public bool puppeteerAndroid;
        public bool AndroidToPuppet = true;
        public bool AwakenedPsychicCoils;
        public bool AwakenedSubroutines;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref puppeteerAndroid, "puppeteerAndroid");
            Scribe_Values.Look(ref AndroidToPuppet, "androidToPuppet", true);
            Scribe_Values.Look(ref AwakenedPsychicCoils, "awakenedPsychic", false);
        }
    }
}