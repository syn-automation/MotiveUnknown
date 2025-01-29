using System;
using System.Collections.Generic;

namespace MurdererStats
{
    public class OccupationSubTypeMapping
    {
        public static Dictionary<Occupation, List<OccupationSubType>> SubTypeMap = new Dictionary<Occupation, List<OccupationSubType>>
        {
            { Occupation.Corporate, new List<OccupationSubType> { OccupationSubType.CEO, OccupationSubType.CFO, OccupationSubType.COO } },
            { Occupation.Professional, new List<OccupationSubType> { OccupationSubType.Doctor, OccupationSubType.Lawyer, OccupationSubType.Engineer } },
            { Occupation.Technician, new List<OccupationSubType> { OccupationSubType.Mechanic, OccupationSubType.Electrician, OccupationSubType.Welder } },
            { Occupation.Laborer, new List<OccupationSubType> { OccupationSubType.FactoryWorker, OccupationSubType.ConstructionWorker, OccupationSubType.Farmer } }
        };

        public static OccupationSubType GetRandomSubType(Occupation occupation)
        {
            if (SubTypeMap.ContainsKey(occupation))
            {
                var subtypes = SubTypeMap[occupation];
                return subtypes[UnityEngine.Random.Range(0, subtypes.Count)];
            }

            throw new Exception("Invalid occupation: no subtypes available.");
        }
    }
}


