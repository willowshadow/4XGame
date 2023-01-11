using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Research_Tree.Weapons
{
    [CreateAssetMenu(fileName = "WeaponModule", menuName = "Data/Create Weapon")]
    public class WeaponModuleData : ShipModuleData
    {
        public OffensiveData offensiveData;
        [PropertyTooltip("How many barrels does the weapon have from which the projectiles fire")]
        [BoxGroup("Weapon Stats")]public int projectileSourceCount;
        [BoxGroup("Weapon Stats")]public int energyConsumptionPerShot;
        [BoxGroup("Weapon Stats")]public int fireRatePerMin;
        [BoxGroup("Weapon Stats")]public int fireVelocity;
        [BoxGroup("Weapon Stats")]public int engagementRange;
        public GameObject weaponModel;
    }
    
    [Serializable]
    public class OffensiveData
    {
        [BoxGroup("Damage Stats")]public int dps;
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int shieldDamage;
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int armorDamage;
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int hullDamage;
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int shieldPenetration;
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int armorPenetration;
        
    }
}