using System;
using Research_Tree.Ship_Hulls;
using Research_Tree.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ship : MonoBehaviour, IDamageable
    {
        public ShipDefensiveStats shipDefensiveStats;
        public ShipHullModuleData hullModuleData;

        private void Awake()
        {
            //todo temporary
            ConstructShip();
        }

        public void ConstructShip()
        {
            shipDefensiveStats.SetupHull(hullModuleData);
            //shipDefensiveStats.SetupSecondary();
        }

        #region Interactions By Others

        public void TakeDamage(OffensiveData damageStats)
        {
            shipDefensiveStats.CalculateDamage(damageStats);
        }

        public bool IsDestroyed()
        {
            return shipDefensiveStats.IsDead();
        }

        public Transform ReferenceAim()
        {
            return transform;
        }

        #endregion

       
    }
}