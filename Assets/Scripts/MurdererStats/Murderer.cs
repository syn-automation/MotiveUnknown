using UnityEngine;
using System.Collections.Generic;

namespace MurdererStats
{
    public class Murderer : MonoBehaviour
    {
        private StatController Stats;
        public bool debugStats = false; 
        public int dStrength = 1;
        public int dDexterity = 1;
        public int dSpeed = 1;
        public int dStamina = 1;
        public int dStealth = 1;
        public int dTemperament = 1;
        public int dEmpathy = 1;
        public int dAwareness = 1;
        private void Awake()
        {
            if (debugStats)
            {
                List<BaseStat> stats = new List<BaseStat>
                {
                    new BaseStat("Strength", dStealth),
                    new BaseStat("Speed", dSpeed),
                    new BaseStat("Stamina", dStamina),
                    new BaseStat("Dexterity", dDexterity),
                    new BaseStat("Stealth", dStealth),
                    new BaseStat("Temperament", dTemperament),
                    new BaseStat("Empathy", dEmpathy),
                    new BaseStat("Awareness", dAwareness)
                };
                
            }
            else
            {
                Stats = new StatController();
                
                Debug.Log($"Randomized Attributes: Build - {Stats.RandomizedAttributes.Build}, " +
                          $"Fitness - {Stats.RandomizedAttributes.Fitness}, Nature - {Stats.RandomizedAttributes.Nature}, " +
                          $"Occupation - {Stats.RandomizedAttributes.Occupation}, " + $"OccupationSubType - {Stats.RandomizedAttributes.OccupationSubType}");
                
                foreach (var stat in Stats.BaseStats)
                {
                    Debug.Log($"{stat.Name}: {stat.ModifiedValue}");
                }
            }
            
            
        }
    }

}

