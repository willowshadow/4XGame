using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    private float movementSpeed=10f;

    public float rotationSpeed=10f;
    public float zoomSpeed=5f;
    public float zoom=20f;
    public float scrollSpeed=10f;
    public float borderPanArea=10f;

    public float fastSpeed=1f;
    public float normalSpeed=1f;

    private Vector3 pos;
    private Quaternion rot;
    [SerializeField]private Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField]private Camera cam;
    [SerializeField]private Terrain terrain;
    [Header("Limits")]
    [SerializeField]private float maxZoom;

    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private float defaultHeight;
    

    //Bounds
    public BoxCollider confiner;
    public float xMin, xMax, zMin, zMax;
    private void Awake()
    {
        
        rot = transform.localRotation;
        zoom = cinemachineVirtualCamera.m_Lens.FieldOfView;
        defaultHeight = transform.position.y;

        var size = confiner.size;
        xMax = size.x;
        xMin = -size.x;
        zMax = size.z;
        zMin = -size.z;
    }

    private void OnDrawGizmos()
    {
        
    }

    private void Update()
    {

        var mousePosition = new Vector3(Screen.width/2,Screen.height/2);
        var ray = cam.ScreenPointToRay(mousePosition);


        if (Physics.Raycast(ray, out var hit, 1000f, layersToHit))
        {
            var height = terrain.SampleHeight(hit.point);
            var tPos = transform.position;
            tPos = new Vector3(tPos.x, defaultHeight+height, tPos.z);
            
            transform.position = tPos;
        }

       
    }

    private void LateUpdate()
    {
        

        pos = transform.position;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
           
        }
        else
        {
            movementSpeed = normalSpeed;
        }


        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height-borderPanArea)
        {
            pos+= movementSpeed * Time.deltaTime * transform.forward;
        }
        else if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < borderPanArea)
        {
            pos += movementSpeed * Time.deltaTime * -transform.forward;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width - borderPanArea)
        {
            pos += movementSpeed * Time.deltaTime * transform.right;
        }
        else if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < borderPanArea)
        {
            pos += movementSpeed * Time.deltaTime * -transform.right;
        }


        if (Input.GetKey(KeyCode.R))
        {
            if(zoom>10)
                zoom -= Time.deltaTime * zoomSpeed;

            cinemachineVirtualCamera.m_Lens.FieldOfView =
            Mathf.Clamp(zoom, 10f, maxZoom);
        }
        else if (Input.GetKey(KeyCode.F))
        {
            if(zoom<maxZoom)
                zoom += Time.deltaTime * zoomSpeed;

            cinemachineVirtualCamera.m_Lens.FieldOfView = 
            Mathf.Clamp(zoom, 10f, maxZoom);
        }

        if (Input.GetKey(KeyCode.Q))
        {
           rot *= Quaternion.Euler(rotationSpeed * Time.deltaTime * Vector3.up);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rot *= Quaternion.Euler(-rotationSpeed * Time.deltaTime * Vector3.up);
        }

        transform.position = Vector3.Lerp(transform.position,pos,Time.deltaTime*movementSpeed*100f);
        transform.rotation = Quaternion.Lerp(transform.rotation,rot,Time.deltaTime*rotationSpeed);
        
        
    }
}
