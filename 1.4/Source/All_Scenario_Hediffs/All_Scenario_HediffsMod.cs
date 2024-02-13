using Verse;
using UnityEngine;

namespace All_Scenario_Hediffs;

public class All_Scenario_HediffsMod : Verse.Mod
{
    public static Settings settings;

    public All_Scenario_HediffsMod(ModContentPack content) : base(content)
    {

        // initialize settings
        settings = GetSettings<Settings>();


    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "All Scenario Hediffs";
    }
}
