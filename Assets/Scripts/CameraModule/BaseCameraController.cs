using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CameraModule
{
    public class BaseCameraController : MonoBehaviour
    {
        public new Transform camera;
        
        [Header("Navigation")]
        public Vector3 navDir;
        public float navSpeed;
        public float navMultiplier;
        private void Awake()
        {
            
        }

        private void Update()
        {
            camera.position = Vector3.Lerp(camera.position,camera.position+navDir,  (Time.deltaTime * navSpeed));
        }

        public void Move(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            navDir = new Vector3(value.x, 0, value.y)*navMultiplier;
        }
    }
}