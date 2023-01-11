using UnityEngine;

namespace Research_Tree.Ship_Hulls
{
    [CreateAssetMenu(menuName = "Create Ship Hull")]
    public class ShipHullModuleData : ShipModuleData
    {
        public ShipClass shipClass;
        public string className;
        public float hullHealth;
        
        public int weaponTurretSlots;
        public int defenseSlots;
        public int engineSlots;
        public int powerSourceSlots;
    }

    public enum ShipClass
    {
        Frigate,
        Destroyer,
        Cruiser,
        BattleShip
    }
}