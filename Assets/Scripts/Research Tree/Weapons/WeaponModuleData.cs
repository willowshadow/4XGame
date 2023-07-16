using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Research_Tree.Weapons
{
    [CreateAssetMenu(fileName = "WeaponModule", menuName = "Data/Create Weapon")]
    public class WeaponModuleData : ShipModuleData
    {
        public WeaponClass weaponClass;
        public WeaponSize weaponSize;
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
        [BoxGroup("Damage Stats")]public int dps; // 300
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int shieldDamage; // 50% -> 150
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int armorDamage;// 90% -> 280
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int hullDamage;// 100 -> 300
        
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int shieldPenetration; // 20% -> 60 -> Armor 
        [BoxGroup("Damage Stats")][PropertyRange(0,300)] public int armorPenetration; // 30% -> 90 -> Hull
    }

    public enum WeaponClass
    {
        Projectile,
        Guided,
        Laser
    }

    public enum WeaponSize
    {
        S,
        M,
        L
    }
}