using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public enum BuildType {Muscular, Lean, Stocky, Average}
public enum FitnessType {Endurance, Agility, Dexterity, Athletic}
public enum Nature {Quiet, Timid, Outgoing, Aggressive, Apathetic}
public enum Occupation {Corporate, Professional, Technician, Laborer}

public enum OccupationSubType
{
    // Corporate Subtypes
    CEO,
    CFO,
    COO,
    // Professional Subtypes
    Doctor,
    Lawyer,
    Engineer,
    // Technician Subtypes
    Mechanic,
    Electrician,
    Welder,
    // Laborer Subtypes
    FactoryWorker,
    ConstructionWorker,
    Farmer
}

namespace MurdererStats
{ 
    [Serializable]
    public class AttributeModifier
    {
    public BuildType Build;
    public FitnessType Fitness;
    public Nature Nature;
    public Occupation Occupation;
    public OccupationSubType OccupationSubType;
    
    public Dictionary<string, int> StatModifiers = new Dictionary<string, int>();

    public AttributeModifier(BuildType build, FitnessType fitness, Nature nature, Occupation occupation)
    {
        Build = build;
        Fitness = fitness;
        Nature = nature;
        Occupation = occupation;
        OccupationSubType = OccupationSubTypeMapping.GetRandomSubType(occupation);

        ApplyBuildModifiers();
        ApplyFitnessModifiers();
        ApplyNatureModifiers();
        ApplyOccupationModifiers();
        

    }

    private void ApplyBuildModifiers()
    {
        switch (Build)
        {
            case BuildType.Muscular:
                AddModifier("Strength", 3);
                AddModifier("Stamina", 1);
                AddModifier("Dexterity", -1);
                break;
            case BuildType.Lean:
                AddModifier("Speed", 2);
                AddModifier("Dexterity", 1);
                AddModifier("Strength", -1);
                break;
            case BuildType.Stocky:
                AddModifier("Strength", 2);
                AddModifier("Stamina", 2);
                AddModifier("Speed", -1);
                break;
            case BuildType.Average:
                AddModifier("Strength", 1);
                AddModifier("Stamina", 1);
                AddModifier("Dexterity", 1);
                AddModifier("Speed", 1);
                break;
        }
    }

    private void ApplyFitnessModifiers()
    {
        switch (Fitness)
        {
            case FitnessType.Endurance:
                AddModifier("Stamina", 3);
                break;
            case FitnessType.Agility:
                AddModifier("Speed", 2);
                AddModifier("Dexterity", 1);
                break;
            case FitnessType.Dexterity:
                AddModifier("Dexterity", 3);
                break;
            case FitnessType.Athletic:
                AddModifier("Strength", 1);
                AddModifier("Speed", 1);
                AddModifier("Stamina", 1);
                break;
        }
    }

    private void ApplyNatureModifiers()
    {
        switch (Nature)
        {
            case Nature.Quiet:
                AddModifier("Stealth", 2);
                break;
            case Nature.Timid:
                AddModifier("Awareness", 1);
                AddModifier("Stealth", 1);
                break;
            case Nature.Outgoing:
                AddModifier("Empathy", 2);
                break;
            case Nature.Aggressive:
                AddModifier("Temperament", -1);
                AddModifier("Strength", 1);
                break;
            case Nature.Apathetic:
                AddModifier("Temperament", -2);
                break;
        }
    }

    private void ApplyOccupationModifiers()
    {
        switch (Occupation)
        {
            case Occupation.Corporate:
                AddModifier("Awareness", 1);
                AddModifier("Empathy", 1);
                break;
            case Occupation.Professional:
                AddModifier("Dexterity", 2);
                break;
            case Occupation.Technician:
                AddModifier("Dexterity", 1);
                AddModifier("Awareness", 1);
                break;
            case Occupation.Laborer:
                AddModifier("Strength", 2);
                AddModifier("Stamina", 1);
                break;
        }
    }
    

    private void AddModifier(String statName, int value)
    {
        if (StatModifiers.ContainsKey(statName))
        {
            StatModifiers[statName] += value;
        }
        else
        {
            StatModifiers[statName] = value;
        }
    }
}
}

