using System;
using System.Collections.Generic;
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
        
        public float maxHullHealth;
        public float maxArmorHealth;
        public float maxShieldHealth;

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
            maxHullHealth = hullHealth;
        }

        public void SetupSecondary(List<DefenseModuleData> data)
        {
            foreach (var defenseModuleData in data)
            {
                switch (defenseModuleData.defenseModuleType)
                {
                    case DefenseModuleType.Shield:
                        shieldHealth += defenseModuleData.value;
                        maxShieldHealth = shieldHealth;
                        break;
                    case DefenseModuleType.Armor:
                        armorHealth += defenseModuleData.value;
                        maxArmorHealth = armorHealth;
                        break;
                }
            }
           
        }

        public bool IsDead()
        {
            if (hullHealth < 0) return true;
            else return false;
        }
    }
}