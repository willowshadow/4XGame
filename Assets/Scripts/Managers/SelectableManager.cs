using System;
using Sirenix.OdinInspector;
using Units;
using UnityEngine;

namespace Managers
{
    public class SelectableManager : MonoBehaviour
    {
        [ShowInInspector]private ISelectable _currentSelection;
        private Camera _cam;



        public DisplayUnitInformation displayInformation;
        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                const float maxDistance = 1000f;
                var ray = _cam.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                {
                    if (_currentSelection != null)
                    {
                        _currentSelection.OnDeselect();
                        displayInformation.RemoveSelection();
                        _currentSelection = null;
                    }

                    return;
                }

                ISelectable selectable = hit.transform.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    if (_currentSelection != null && _currentSelection != selectable)
                    {
                        _currentSelection.OnDeselect();
                    }

                    _currentSelection = selectable;
                    displayInformation.UpdateSelectedUnit(_currentSelection.OnSelect());
                }
                else if (_currentSelection != null)
                {
                    _currentSelection.OnDeselect();
                    displayInformation.RemoveSelection();
                    _currentSelection = null;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (_currentSelection != null)
                {
                    _currentSelection.OnAction();
                }
            }
        }
    }
}
