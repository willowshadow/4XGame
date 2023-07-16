using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Generic_Interfaces;
using Research_Tree;
using Research_Tree.Defense;
using Research_Tree.Engine;
using Research_Tree.Ship_Hulls;
using Research_Tree.Weapons;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static UnityEngine.Quaternion;

namespace Units
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ship : MonoBehaviour, IDamageable , ISelectable,IBaseUnit
    {
        //temp
        public List<ShipModuleData> allModules;
        
        
        public ShipDefensiveStats shipDefensiveStats;
        
        
        [BoxGroup("Modules")]public ShipHullModuleData hullModule;
        [BoxGroup("Modules")]public List<EngineModuleData> engineModules;
        [BoxGroup("Modules")]public List<DefenseModuleData> defenseModules;

        
       [BoxGroup("Events")] public UnityEvent<Vector3> onShipMovement;
       [BoxGroup("Events")] public UnityEvent onShipDestinationReached;
       [BoxGroup("Events")] public UnityEvent onShipDeselect;
       [BoxGroup("Events")] public UnityEvent onShipSelect;


        //todo add more factors
        // if we want 10m/s speed we need to multiply by total weight to get required Engine Power
        private float shipSpeed
        {
            get
            {
                float totalWeight = 0;
                foreach (var module in allModules)    
                {
                    totalWeight+=module.weightCost;
                }

                float enginePower = 0;
                foreach (var module in engineModules)
                {
                    enginePower += module.engineHorsePower;
                }
                return enginePower/totalWeight;
            }
        }

        private float shipTraverseSpeed => shipSpeed * 2f;


        private void Awake()
        {
            //todo temporary
            ConstructShip();
        }

        private void ConstructShip()
        {
            shipDefensiveStats.SetupHull(hullModule);
            shipDefensiveStats.SetupSecondary(defenseModules);
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

        #region Ship Movement

        private bool ShouldMove(Vector3 targetPosition)
        {
            var distance = (targetPosition - transform.position).sqrMagnitude;
            return distance > 500;
        }
        private void MoveTowardsPosition(Vector3 position)
        {
            StartCoroutine(MoveTowardsPositionCo(position));
        }

        private IEnumerator MoveTowardsPositionCo(Vector3 position)
        {
            float rotationSpeed = shipTraverseSpeed; // Speed of rotation
            float movementSpeed = shipSpeed; // Speed of movement
            Transform t = transform;
            Vector3 direction;
            Quaternion rotation;
            Quaternion targetRotation;
            
            while (true)
            {
                
                // Calculate the direction vector from the object to the target position.
                direction = position - t.position;
                rotation = t.rotation;
        
                // Check if the object has reached the position. You can adjust the value to your needs.
                if (direction.sqrMagnitude < 100f)
                {
                    onShipDestinationReached.Invoke();
                    yield break;
                }
                
                // Calculate the rotation needed to face the target position.
                targetRotation = LookRotation(direction);
        
                // Calculate the desired angle between object's forward vector and target's direction.
                
                float desiredAngle = Angle(rotation, targetRotation);
        
                // Modify the speed based on the desired angle. The larger the angle, the slower the speed.
                float adjustedSpeed = movementSpeed * (1f - desiredAngle/180f);

                // Smoothly rotate the object to face the target over time according to rotationSpeed.
                rotation = RotateTowards(rotation, targetRotation, 
                    Time.deltaTime * rotationSpeed);
                t.rotation = rotation;

                // Move the object towards the target position over time according to the adjusted movementSpeed.
                t.position += t.forward * (Time.deltaTime * adjustedSpeed);
                
                yield return null;
            }
            
        }
        
        #endregion

        #region Ship Attack

        public void AttackTarget(IDamageable damageable)
        {
            var t = damageable.ReferenceAim();
        }

        #endregion

        #region ISelection

        public IBaseUnit OnSelect()
        {
            onShipSelect.Invoke();
            return this;
        }

        public void OnDeselect()
        {
            onShipDeselect.Invoke();
        }

        // On Right Click
        public void OnActionSelf()
        {
            var mousePosition = Input.mousePosition;
            var worldPosition = Camera.main.ScreenPointToRay(mousePosition);
            var layerMaskMove = LayerMask.NameToLayer("Map");
            var layerMaskAttack = LayerMask.NameToLayer("Unit");
            
            if (Physics.Raycast(worldPosition, out RaycastHit hit, 5000,1<<layerMaskMove))
            {
                if(!ShouldMove(hit.point)) return;
                StopAllCoroutines();
                MoveTowardsPosition(hit.point);
                onShipMovement.Invoke(hit.point);
                return;
            }
            if (Physics.Raycast(worldPosition, out RaycastHit point, 5000,1<<layerMaskAttack))
            {
                if(!ShouldMove(hit.point)) return;
                StopAllCoroutines();
                MoveTowardsPosition(point.point);
                onShipMovement.Invoke(point.point);
                return;
            }
        }

        public void OnActionAtPosition(Vector3 position)
        {
            if(!ShouldMove(position)) return;
            StopAllCoroutines();
            MoveTowardsPosition(position);
            onShipMovement.Invoke(position);
            return;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        #endregion
        
        #region IBaseUnit

        public string unitName
        {
            get => $"{hullModule.className } {hullModule.shipClass}";

            set => throw new NotImplementedException();
        }
        public int hp
        {
            get => (int)shipDefensiveStats.hullHealth;
            set => throw new NotImplementedException();
        }

        public int maxHp {
            
            get => (int)shipDefensiveStats.maxHullHealth;
            set => throw new NotImplementedException();
        }

        public int armor
        {
            get => (int)shipDefensiveStats.armorHealth;
            set => throw new NotImplementedException();

        }

        public int maxArmor
        {
            get => (int)shipDefensiveStats.maxArmorHealth;
            set => throw new NotImplementedException();

        }

        public int shield
        {
            get => (int)shipDefensiveStats.shieldHealth;
            set => throw new NotImplementedException();
        }

        public int maxShield
        {
            get => (int)shipDefensiveStats.maxShieldHealth;
            set => throw new NotImplementedException();
        }

        public Sprite sprite
        {
            get => hullModule.moduleIcon;
            set=>throw new NotImplementedException();
        }

        #endregion
    }
}