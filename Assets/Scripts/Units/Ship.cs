using System;
using System.Collections;
using DG.Tweening;
using Generic_Interfaces;
using Research_Tree.Engine;
using Research_Tree.Ship_Hulls;
using Research_Tree.Weapons;
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
        public ShipDefensiveStats shipDefensiveStats;
        public ShipHullModuleData hullModuleData;
        public EngineModuleData engineModuleData;

        public UnityEvent<Vector3> onShipMovement;
        public UnityEvent onShipDestinationReached;
        public UnityEvent onShipDeselect;
        public UnityEvent onShipSelect;
        
        private void Awake()
        {
            //todo temporary
            ConstructShip();
        }

        private void ConstructShip()
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

        #region Ship Movement

        private void MoveTowardsPosition(Vector3 position)
        {
            StartCoroutine(MoveTowardsPositionCo(position));
        }

        private IEnumerator MoveTowardsPositionCo(Vector3 position)
        {
            float rotationSpeed = engineModuleData.traverseMaxSpeed; // Speed of rotation
            float movementSpeed = engineModuleData.engineMaxSpeed; // Speed of movement
            Transform t = transform;
            Vector3 direction;
            Quaternion targetRotation;
            
            float tiltAmount = 30f; // Tilt amount in degrees. Adjust as needed.
            float tiltSpeed = 2f; // Speed at which the ship tilts. Adjust as needed.
            
            while (true)
            {
                
                // Calculate the direction vector from the object to the target position.
                direction = position - t.position;
        
                // Check if the object has reached the position. You can adjust the value to your needs.
                if (direction.sqrMagnitude < 7f)
                {
                    onShipDestinationReached.Invoke();
                    yield break;
                }
                
                // Calculate the rotation needed to face the target position.
                targetRotation = LookRotation(direction);
                
                // Calculate the tilt based on the difference in y-axis rotation
                var rotation = t.rotation;
                float currentYaw = rotation.eulerAngles.y;
                float targetYaw = targetRotation.eulerAngles.y;

                float tiltZ = Mathf.DeltaAngle(currentYaw, targetYaw) * tiltAmount;

                // Create a quaternion for the tilt around the Z axis
                Quaternion tiltRotation = Euler(0, 0, tiltZ);

                // Smoothly rotate the object to face the target over time according to rotationSpeed.
                rotation = RotateTowards(rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
                // Smoothly apply the tilt
                rotation = Slerp(rotation, tiltRotation, Time.deltaTime * tiltSpeed);
                t.rotation = rotation;

                // Move the object towards the target position over time according to movementSpeed.
                t.position += t.forward * (Time.deltaTime * movementSpeed);
                
                yield return null;
            }
            
        }
        
        #endregion
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
        public void OnAction()
        {
            var mousePosition = Input.mousePosition;
            var worldPosition = Camera.main.ScreenPointToRay(mousePosition);
            var layerMask = LayerMask.NameToLayer("Map");
            if (Physics.Raycast(worldPosition, out RaycastHit hit, 5000,1<<layerMask))
            {
                StopAllCoroutines();
                MoveTowardsPosition(hit.point);
                onShipMovement.Invoke(hit.point);
            }
        }



        public string unitName
        {
            get => $"{hullModuleData.className } {hullModuleData.shipClass}";

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

        public Sprite sprite
        {
            get => hullModuleData.moduleIcon;
            set=>throw new NotImplementedException();
        }
    }
}