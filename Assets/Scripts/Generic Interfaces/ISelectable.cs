using Generic_Interfaces;
using UnityEngine;

namespace Units
{
    public interface ISelectable
    {
        public IBaseUnit OnSelect();
        public void OnDeselect();
        public void OnActionSelf();
        public void OnActionAtPosition(Vector3 position);
        public Transform GetTransform();
    }
}