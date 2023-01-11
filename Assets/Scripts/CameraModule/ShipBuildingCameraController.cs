using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipBuildingCameraController : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public bool lookEnabled;
    public float lookSpeed;
    public float lookDistance;
    private void Awake()
    {
        cam.transform.LookAt(target);
        cam.transform.position-=(cam.transform.forward*lookDistance);
    }

    public void Update()
    {
        cam.transform.LookAt(target);
        if (!lookEnabled)
        {
            //target.rotation = Quaternion.Lerp(target.rotation,Quaternion.identity, Time.deltaTime*5f);
        }
        
    }
    public void ReadMouseInput(InputAction.CallbackContext context)
    {
        if(!lookEnabled) return;
        Vector2 mousePosition = context.ReadValue<Vector2>();
        //Vector2 objectPosition = (Vector2) Camera.main.WorldToScreenPoint(transform.position);
        //Vector2 direction = (mousePosition - objectPosition).normalized;
        
        Vector3 mouseMove = new Vector3(mousePosition.x, mousePosition.y, 0f);
        Vector3 mouseWorldDirection = cam.transform.TransformDirection(
            mouseMove);
        Vector3 rotAxis = Vector3.Cross(mouseWorldDirection, 
            cam.transform.forward);

        target.RotateAround(target.position, Vector3.up, Time.deltaTime* mousePosition.x* lookSpeed);
        
    }

    public void ReadLookMode(InputAction.CallbackContext context)
    {
        lookEnabled = context.ReadValueAsButton();
    }
}
