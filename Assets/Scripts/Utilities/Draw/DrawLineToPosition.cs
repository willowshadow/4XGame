using UnityEngine;

namespace Utilities.Draw
{
    public class DrawLineToPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 targetPosition; // target position as Vector3
        [SerializeField] private LineRenderer lineRenderer;  // LineRenderer component

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

        public void SetPositions(Vector3 newTargetPosition)
        {
            targetPosition = newTargetPosition;
            EnableLineDrawing();
        }

        public void EnableLineDrawing()
        {
            lineRenderer.enabled = true;
        }

        public void DisableLineDrawing()
        {
            lineRenderer.enabled = false;
        }
    }
}
