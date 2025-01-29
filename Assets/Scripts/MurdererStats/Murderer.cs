using UnityEngine;

namespace MurdererStats
{
    public class Murderer : MonoBehaviour
    {
        private StatController Stats;
        private void Awake()
        {
            Stats = new StatController();
            
            Debug.Log($"Randomized Attributes: Build - {Stats.RandomizedAttributes.Build}, " +
                      $"Fitness - {Stats.RandomizedAttributes.Fitness}, Nature - {Stats.RandomizedAttributes.Nature}, " +
                      $"Occupation - {Stats.RandomizedAttributes.Occupation}, " + $"OccupationSubType - {Stats.RandomizedAttributes.OccupationSubType}");

            foreach (var stat in Stats.BaseStats)
            {
                Debug.Log($"{stat.Name}: {stat.ModifiedValue}");
            }
            
            // test comment
            
        }
    }

}

