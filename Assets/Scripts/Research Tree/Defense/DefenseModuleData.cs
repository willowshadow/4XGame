using UnityEngine;

namespace Research_Tree.Defense
{
    [CreateAssetMenu(fileName = "DefenseModule", menuName = "Data/Create Defense Module")]
    public class DefenseModuleData : ShipModuleData
    {
        public DefenseModuleType defenseModuleType;
        public int value;
    }

    public enum DefenseModuleType
    {
        Shield,
        Armor
    }
}