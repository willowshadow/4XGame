using System;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities.Vector;

namespace CameraModule
{
    public class BaseCameraController : MonoBehaviour
    {
        public Transform target;
        public CinemachineVirtualCamera cameraCm;
        public Cinemachine3rdPersonFollow cameraFollow;
        public BoxCollider boundaryBox;
        
        [Header("Navigation")]
        public Vector3 navDir;
        public Vector3 navRot;
        public float navSpeed;
        public float rotSpeed;
        public float navMultiplier;

        public float lookSpeed;
        public bool lookEnabled;

        [SerializeField] private float zoomSpeed;
        [SerializeField] private float zoomRange;
        [SerializeField] private float zoom;

        private void Awake()
        {
            cameraFollow = cameraCm.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            _totalZoom = 0;
            // Translate the change in zoom from local space to world space
            
            _desiredPosition = target.position;
            // Get the bounds
            var bounds = boundaryBox.bounds;
            _desiredPosition = new Vector3(
                Mathf.Clamp(_desiredPosition.x, bounds.min.x, bounds.max.x),
                Mathf.Clamp(_desiredPosition.y, bounds.min.y, bounds.max.y), // Assuming we are not constraining height here
                Mathf.Clamp(_desiredPosition.z, bounds.min.z, bounds.max.z)
            );

            target.DOLocalMove(_desiredPosition + _zoomMovement, 0.2f);
        }

        private void Update()
        {
            // Move the target
            ClampedZoom();
            ClampedMovement();
            
        }


        public void Move(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            Vector3 forward = target.forward;
            forward.y = 0;
            forward.Normalize();
    
            Vector3 right = target.right;
            right.y = 0;
            right.Normalize();
   
            navDir = value.y * forward + value.x * right;
           
        }
        private void ClampedZoom()
        {
            // Update the target position at a smooth rate
            cameraFollow.CameraDistance = Mathf.MoveTowards(cameraFollow.CameraDistance, _totalZoom, Time.deltaTime*zoomSpeed);
            
        }

        private void ClampedMovement()
        {
            if(navDir==Vector3.zero) return;

            target.Translate(navDir * (navSpeed * Time.deltaTime * navMultiplier), Space.World);

            // Get the bounds
            var bounds = boundaryBox.bounds;

            // Clamp the position of the target to the bounds
            
            _desiredPosition = target.position;
            _desiredPosition = new Vector3(
                Mathf.Clamp(_desiredPosition.x, bounds.min.x, bounds.max.x),
                Mathf.Clamp(_desiredPosition.y, bounds.min.y, bounds.max.y), // Assuming we are not constraining height here
                Mathf.Clamp(_desiredPosition.z, bounds.min.z, bounds.max.z)
            );
            
            target.position = _desiredPosition;
            
        }

        public void Rotate(InputAction.CallbackContext ctx)
        {
            return;
            var value = ctx.ReadValue<float>();
            navRot = new Vector3(0, value, 0);
        }

        public void FastTraverse(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValueAsButton();
            navMultiplier = value ? 10 : 1;
        }

        public void LookEnabled(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValueAsButton();
            lookEnabled = value;
        }

        private float _verticalLookRotation;

        public void Look(InputAction.CallbackContext ctx)
        {
            if(!lookEnabled) return;
            var value = ctx.ReadValue<Vector2>();

            // Handling horizontal rotation (y-axis).
            target.Rotate(Vector3.up * value.x * Time.deltaTime * lookSpeed);

            // Handling vertical rotation (x-axis).
            // Add the vertical movement to the verticalLookRotation variable with a negative sign because positive mouse input should result in looking down.
            //_verticalLookRotation += -value.y * Time.deltaTime * lookSpeed;
            // Clamping the vertical look rotation to avoid unnatural flipping.
            //_verticalLookRotation = Mathf.Clamp(_verticalLookRotation, 10f, 80f);
            // Apply the vertical look rotation to the local x-axis rotation of the target.
            target.localEulerAngles = new Vector3(44, target.localEulerAngles.y, 0);
        }
        
        private float _totalZoom = 0;
        private float _actualDeltaZoom;
        private float _desiredDeltaZoom;
        private Vector3 _zoomMovement;
        private Vector3 _desiredPosition;

        public void Zoom(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            zoom = ctx.ReadValue<float>() * zoomSpeed * Time.deltaTime;

            // Calculate the desired change in zoom
            float deltaZoom = Mathf.Sign(zoom)*5f;

            // Calculate the desired total zoom and clamp it
            float desiredTotalZoom = _totalZoom - deltaZoom;

            desiredTotalZoom = Mathf.Clamp(desiredTotalZoom, 0, zoomRange);

            // Get the actual zoom change after clamping 
            _desiredDeltaZoom = desiredTotalZoom - _totalZoom;

            // Update total zoom    
            _totalZoom = desiredTotalZoom;
          
        }
    }
}