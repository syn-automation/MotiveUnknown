using UnityEngine;
using System.Collections.Generic;

namespace MurdererStats
{
    public class StatController
    {
        //Base Physical Stats
        public int baseStrength = 1;
        public int baseSpeed = 1;
        public int baseStamina = 1;
        public int baseDexterity = 1;
        
        //Base Psychological Stats
        public int baseStealth = 1;
        public int baseTemperament = 1;
        public int baseEmpathy = 1;
        public int baseAwareness = 1;
        
        
        public List<BaseStat> BaseStats;
        public AttributeModifier RandomizedAttributes;

        public StatController()
        {
            BaseStats = new List<BaseStat>
            {
                new BaseStat("Strength", baseStealth),
                new BaseStat("Speed", baseSpeed),
                new BaseStat("Stamina", baseStamina),
                new BaseStat("Dexterity", baseDexterity),
                new BaseStat("Stealth", baseStealth),
                new BaseStat("Temperament", baseTemperament),
                new BaseStat("Empathy", baseEmpathy),
                new BaseStat("Awareness", baseAwareness)
            };

            RandomizeAttributes();
            ApplyAttributeModifiers();
        }

        private void RandomizeAttributes()
        {
            RandomizedAttributes = new AttributeModifier(
                (BuildType)Random.Range(0, System.Enum.GetValues(typeof(BuildType)).Length),
                (FitnessType)Random.Range(0, System.Enum.GetValues(typeof(FitnessType)).Length),
                (Nature)Random.Range(0, System.Enum.GetValues(typeof(Nature)).Length),
                (Occupation)Random.Range(0, System.Enum.GetValues(typeof(Occupation)).Length)
            );
        }
        private void ApplyAttributeModifiers()
        {
            foreach (var stat in BaseStats)
            {
                if (RandomizedAttributes.StatModifiers.ContainsKey(stat.Name))
                {
                    stat.ApplyModifier(RandomizedAttributes.StatModifiers[stat.Name]);
                }
            }
        }
        
    }
}


