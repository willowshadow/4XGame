using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    public MeshRenderer[] meshes;
    [ShowInInspector]private int _nCollisions;
    [SerializeField]private BoxCollider _collider;
    [SerializeField]private LayerMask layer;
    [SerializeField] private float offsetGround;
    

    public void SetMaterialInPlacementCheck(Material mat)
    {
        foreach (var meshRend in meshes)
        {
            meshRend.sharedMaterial = mat;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            _nCollisions++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            _nCollisions--;
        }
    }

    public void SetPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition+Vector3.up*offsetGround;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetRotation(Vector3 worldRotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(worldRotation),Time.deltaTime );
    }

    public Vector3 GetRotation() => transform.rotation.eulerAngles;
    public bool HasValidPlacement()
    {
        if (_nCollisions > 0) return false;

        // get 4 bottom corner positions
        Vector3 p = transform.position;
        Vector3 c = _collider.center;
        Vector3 e = _collider.size / 2f;
        float bottomHeight = c.y - e.y + 0.5f;
        Vector3[] bottomCorners = {
            new(c.x - e.x, bottomHeight, c.z - e.z),
            new(c.x - e.x, bottomHeight, c.z + e.z),
            new(c.x + e.x, bottomHeight, c.z - e.z),
            new(c.x + e.x, bottomHeight, c.z + e.z)
        };
        // cast a small ray beneath the corner to check for a close ground
        // (if at least two are not valid, then placement is invalid)
        int invalidCornersCount = 0;
        foreach (Vector3 corner in bottomCorners)
        {
            if (!Physics.Raycast(
                    p + corner,
                    Vector3.up * -1f,
                    1f,
                    layer
                ))
                invalidCornersCount++;
        }
        Debug.Log(invalidCornersCount);
        return invalidCornersCount < 1;
    }
}
