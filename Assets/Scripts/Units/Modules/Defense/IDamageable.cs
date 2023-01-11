using Research_Tree.Weapons;
using UnityEngine;

namespace Units
{
    public interface IDamageable
    {
        public void TakeDamage(OffensiveData damageStats);
        public bool IsDestroyed();

        public Transform ReferenceAim();
    }
}