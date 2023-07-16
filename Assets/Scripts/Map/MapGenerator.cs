using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class MapGenerator : MonoBehaviour
    {
        public Vector2 mapSize;
        public Transform mapBase;
        public Collider mapCameraCollider;
        
        public int asteroidCount;
        public float scaleMinRange,scaleMaxRange;
        public GameObject[] asteroidPrefab;
        public Transform asteroidParent;

        private void OnValidate()
        {
            //SetMapBase();
        }

        private IEnumerator Start()
        {
            SetMapBase();
            for (int i = 0; i < asteroidCount; i++)
            {
                yield return null;
                SpawnAsteroid();
            }
        }

        private void SetMapBase()
        {
            mapBase.localScale = new Vector3(mapSize.x / 10f, 1, mapSize.y / 10f);
            mapCameraCollider.transform.localScale = new Vector3(mapSize.x , 1000, mapSize.y);
        }

        private void SpawnAsteroid()
        {
            Bounds bounds = mapCameraCollider.bounds;
            float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
            float offsetY = Random.Range(-bounds.extents.y, -100);
            float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);

            var randomAsteroid = Random.Range(0, asteroidPrefab.Length);
            GameObject newHazard = Instantiate(asteroidPrefab[randomAsteroid],asteroidParent);
            newHazard.transform.position = bounds.center + new Vector3(offsetX, offsetY, offsetZ);
            newHazard.transform.localScale *= Random.Range(scaleMinRange, scaleMaxRange);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapSize.x,0,mapSize.y));
        }
    }
}