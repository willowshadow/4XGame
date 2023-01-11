using System;
using System.Collections;
using System.Collections.Generic;
using Research_Tree.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Units.Modules
{
    public class Weapon : MonoBehaviour
    {
        public WeaponModuleData weaponData;
        [ShowInInspector]protected IDamageable currentEngagedTarget;
        public Transform[] muzzles;
        [ShowInInspector] private HashSet<IDamageable> _trackedTargets;
        private IDamageable _owner;

        private void Awake()
        {
            _owner = GetComponentInParent<IDamageable>();
            GetComponent<SphereCollider>().radius = weaponData.engagementRange;
            StartCoroutine(nameof(StartEngagement));
        }

        private IEnumerator StartEngagement()
        {
            while(true)
            {
                while (currentEngagedTarget==null)
                {
                    yield return null;
                }
                Aim();
                Shoot();
                yield return new WaitForSeconds(60/weaponData.fireRatePerMin);
            }
        }
        
        public virtual void Shoot()
        {
            
        }

        

        public virtual void Aim()
        {
            if(currentEngagedTarget==null) return;
            transform.LookAt(currentEngagedTarget.ReferenceAim());
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable target))
            {
                if(target==_owner) return;
                currentEngagedTarget = target;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IDamageable target))
            {
                if(target==_owner) return;
                currentEngagedTarget = target;
            }
        }
    }
}