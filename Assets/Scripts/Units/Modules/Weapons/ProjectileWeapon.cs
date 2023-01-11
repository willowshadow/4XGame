using Sirenix.OdinInspector;
using Units.Modules.Ammo;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units.Modules
{
    [RequireComponent(typeof(SphereCollider))]
    public class ProjectileWeapon : Weapon
    {
        [FormerlySerializedAs("projectilePrefab")] public Projectile projectilePrefab;
        public override void Shoot()
        {
            base.Shoot();
            if(currentEngagedTarget==null) return;
            if(currentEngagedTarget.IsDestroyed())return;

            foreach (var muzzle in muzzles)
            {
                var projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
                projectilePrefab.SetOwner(this);
                projectile.Fire();
            }
        }
        
        [Button]
        public void TestShoot()
        {
            foreach (var muzzle in muzzles)
            {
                var projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
                projectilePrefab.SetOwner(this);
                projectile.Fire();
            }
        }
    }
}