using System;
using DG.Tweening;
using UnityEngine;

namespace Utilities.Draw
{
    public class DrawLineToPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 targetPosition; // target position as Vector3
        [SerializeField] private LineRenderer lineRenderer;  // LineRenderer component
        [SerializeField] private Transform destinationPoint;
        
        private void Start()    
        {
            // Initially line renderer should be disabled
            lineRenderer.enabled = false;
            
            //Set Position To Transform Position
            var position = targetPosition = transform.position;
            lineRenderer.SetPosition(0,position);
            lineRenderer.SetPosition(1,position);
        }

        private void Update()
        {
            if (lineRenderer.enabled)
            {
                // Draw line to targetPosition
                lineRenderer.SetPosition(0, transform.position); // Line starts from this object's position
                lineRenderer.SetPosition(1, targetPosition); // Line ends on the target position
            }
        }

        private void LateUpdate()
        {
            if (lineRenderer.enabled) destinationPoint.position = targetPosition;
        }

        public void SetPositions(Vector3 newTargetPosition)
        {
            destinationPoint.position = targetPosition = newTargetPosition;
            EnableLineDrawing();
        }

        public void EnableLineDrawing()
        {
            destinationPoint.gameObject.SetActive(true);
            destinationPoint.DOPunchScale(Vector3.one*3, 0.4f);
            lineRenderer.enabled = true;
        }

        public void DisableLineDrawing()
        {
            lineRenderer.enabled = false;
            destinationPoint.gameObject.SetActive(false);
        }
    }
}
