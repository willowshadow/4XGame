using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GalaxyPlacement : MonoBehaviour
{
    [SerializeField] private Vector3 galaxySize;
    [SerializeField] private float starGap;
    
    public List<Vector3> starPositions;
    public PoissonDiskSampler3D dataSampler;

    public List<Transform> starPrefabs;
    [SerializeField]private List<Transform> _instantiated;

    [Button]
    public void Generate()
    {
        starPositions = new List<Vector3>();
        dataSampler = new PoissonDiskSampler3D(galaxySize.x, galaxySize.y, galaxySize.z, starGap);
        foreach (var star in _instantiated)
        {
            DestroyImmediate(star.gameObject);
        }

        _instantiated = new List<Transform>();
        
        foreach (var pos in dataSampler.Samples())
        {
            starPositions.Add(pos);
        }

        foreach (var starPosition in starPositions)
        {
            var random = Random.Range(0, starPrefabs.Count);
            var star=Instantiate(starPrefabs[random], starPosition, quaternion.identity, transform);
            _instantiated.Add(star);
        }
    }

    private void OnDrawGizmos()
    {
        //Handles.DrawWireDisc(Vector3.zero,Vector3.up,galaxySize);
        foreach (var position in starPositions)
        {
            Gizmos.DrawSphere(position,.1f);
        }
    }
}
