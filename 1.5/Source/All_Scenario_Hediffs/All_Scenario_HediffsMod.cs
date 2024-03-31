using Verse;
using UnityEngine;

namespace All_Scenario_Hediffs;

[StaticConstructorOnStartup]
public static class ApplyOverrides
{
    static ApplyOverrides()
    {
        All_Scenario_HediffsMod.settings.ApplyOverrides();
    }
}

public class All_Scenario_HediffsMod : Mod
{
    public static Settings settings;

    public All_Scenario_HediffsMod(ModContentPack content) : base(content)
    {
        // initialize settings
        settings = GetSettings<Settings>();
        settings.ApplyOverrides();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Feldoh_AllScenarioHediffs_SettingsName".Translate();
    }
}
