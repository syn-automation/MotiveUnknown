using UnityEngine;
using System;

namespace MurdererStats
{
    [Serializable]
    public class BaseStat
    {
    
        public string Name; //Name of Stat
        public int BaseValue;
        public int ModifiedValue;
        public const int MinValue = 0;
        public const int MaxValue = 100;

        public BaseStat(string name, int baseValue)
        {
            Name = name;
            BaseValue = baseValue;
            ModifiedValue = BaseValue;
        
        }

        public void ApplyModifier(int amount)
        {
            ModifiedValue += BaseValue + amount;
        }
    }
}

