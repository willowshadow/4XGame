using UnityEngine;

namespace Research_Tree.Power_Source
{
    [CreateAssetMenu(fileName = "PSource", menuName = "Data/Create Power Source")]
    public class PowerModuleData : ShipModuleData
    {
        public float maxCapacity;
        public float rateOfCharge;
    }
}