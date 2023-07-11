using UnityEngine;

namespace Units.Modules.Ammo
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public Weapon owner;
        public Rigidbody rB;

        public void SetOwner(Weapon weapon)
        {
            owner = weapon;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(owner.weaponData.offensiveData);
            Destroy(gameObject);
        }

        public void Fire()
        {
            rB.AddForce(owner.transform.forward*owner.weaponData.fireVelocity,ForceMode.VelocityChange);
            Destroy(gameObject,10f);
            
        }
    }
}