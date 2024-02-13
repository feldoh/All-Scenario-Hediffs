using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace All_Scenario_Hediffs;

public class Settings : ModSettings
{
    private const float RowHeight = 60f;
    public Dictionary<string, bool> HediffOverrides = [];
    private Vector2 _scrollPosition = Vector2.zero;
    private string _searchQuery = string.Empty;

    public bool MatchesFilter(HediffDef hediff) =>
        _searchQuery.Length < 1 ||
        hediff.LabelCap.Resolve().ToLowerInvariant().Contains(_searchQuery.ToLowerInvariant()) ||
        hediff.defName.ToLowerInvariant().Contains(_searchQuery.ToLowerInvariant());

    public void DoWindowContents(Rect wrect)
    {
        Listing_Standard options = new();
        options.Begin(wrect);

        if (options.ButtonText("Feldoh_AllScenarioHediffs_ClearAll".Translate()))
        {
            HediffOverrides.Clear();
        }

        if (options.ButtonText("Feldoh_AllScenarioHediffs_Apply".Translate()))
        {
            ApplyOverrides();
        }

        options.Gap();

        // Add a TextField for the search query
        string lastSearch = _searchQuery;
        float searchBarY = wrect.y;
        Widgets.Label(new Rect(0, searchBarY, 120, 30f), "Feldoh_AllScenarioHediffs_SearchLabel".Translate());
        _searchQuery = Widgets.TextField(new Rect(130, searchBarY, wrect.width - 130, 30f), _searchQuery);
        if (lastSearch != _searchQuery) _scrollPosition = Vector2.zero;

        options.Gap();

        // Filter the current list
        List<HediffDef> filteredHediffDefs = DefDatabase<HediffDef>.AllDefsListForReading.Where(MatchesFilter).ToList();

        // Create a scrollable list
        Rect viewRect = new(0, 0, wrect.width - 16f, filteredHediffDefs.Count * RowHeight);
        Rect scrollRect = new(0, 80f, wrect.width, wrect.height - 80f);
        Widgets.BeginScrollView(scrollRect, ref _scrollPosition, viewRect);

        float num = 0f;
        foreach (HediffDef hediffDef in filteredHediffDefs)
        {
            Rect rowRect = new Rect(0, num * RowHeight, viewRect.width - 10, RowHeight - 5f);
            bool state = HediffOverrides.GetWithFallback(hediffDef.defName, hediffDef.scenarioCanAdd);
            bool beforeState = state;
            Widgets.CheckboxLabeled(rowRect, hediffDef.LabelCap, ref state, placeCheckboxNearText: true);
            if (beforeState != state)
            {
                HediffOverrides.SetOrAdd(hediffDef.defName, state);
                hediffDef.scenarioCanAdd = state;
            }

            num++;
        }

        Widgets.EndScrollView();

        options.End();
    }

    public void ApplyOverrides()
    {
        foreach ((string hediffDefName, bool scenarioCanAdd) in HediffOverrides)
        {
            if (DefDatabase<HediffDef>.GetNamedSilentFail(hediffDefName) is { } def) def.scenarioCanAdd = scenarioCanAdd;
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref HediffOverrides, "hediffOverrides", LookMode.Value);
        if (Scribe.mode == LoadSaveMode.PostLoadInit) ApplyOverrides();
    }

}
