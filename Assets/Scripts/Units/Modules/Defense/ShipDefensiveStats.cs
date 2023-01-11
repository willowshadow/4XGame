using System;
using Research_Tree.Defense;
using Research_Tree.Ship_Hulls;
using Research_Tree.Weapons;

namespace Units
{
    [Serializable]
    public class ShipDefensiveStats
    {
        public float hullHealth;
        public float armorHealth;
        public float shieldHealth;

        public void CalculateDamage(OffensiveData damageStats)
        {
            if (shieldHealth > 0)
            {
                
            }

            if (armorHealth > 0)
            {
                
            }

            if (hullHealth > 0)
            {
                
            }
        }

        public void SetupHull(ShipHullModuleData data)
        {
            hullHealth = data.hullHealth;
        }

        public void SetupSecondary(DefenseModuleData data)
        {
            switch (data.defenseModuleType)
            {
                case DefenseModuleType.Shield:
                    shieldHealth = data.value;
                    break;
                case DefenseModuleType.Armor:
                    armorHealth = data.value;
                    break;
            }
        }

        public bool IsDead()
        {
            if (hullHealth < 0) return true;
            else return false;
        }
    }
}