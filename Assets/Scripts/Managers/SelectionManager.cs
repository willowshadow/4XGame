using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Units;
using Unity.Mathematics;
using UnityEngine;
using Utilities;
using Utilities.Vector;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SelectionManager : MonoBehaviour
    {
        [ShowInInspector]private HashSet<ISelectable> _currentSelections;
        [ShowInInspector]private ISelectable _currentSelection;
        private ISelectable _selectable=null;
        private Camera _cam;
        
        public DisplayUnitInformation displayInformation;
        
        private bool _isSelecting = false;
        private Vector3 _selectionStartPosition;
        [SerializeField]private Color selectionColor;
        [SerializeField]private Color selectionBorderColor;

        public Texture2D mouseCursor;
        public LayerMask unitLayerMask;
        public LayerMask mapLayerMask;

        private Collider[] results;
        private void Awake()
        {
            results = new Collider[200];
            _cam = Camera.main;
            //_currentSelections = new HashSet<ISelectable>();
            Cursor.SetCursor(mouseCursor, Vector2.zero , CursorMode.Auto);

            _allPlayerUnits = new List<ISelectable>();
            _multiSelectedUnit = new List<ISelectable>();
        }


        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                DeSelectUnits();
            }
            if (Input.GetMouseButtonDown(0))
            {
                const float maxDistance = 1000f;
                var ray = _cam.ScreenPointToRay(Input.mousePosition);
                
                if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance,unitLayerMask))
                {
                    /*if (_currentSelection != null)
                    {
                        _currentSelection.OnDeselect();
                        displayInformation.RemoveSelection();
                    }*/
                    _isSelecting = true;
                    _selectionStartPosition = Input.mousePosition;
                    if(!Input.GetKey(KeyCode.LeftShift))
                    {
                        DeSelectUnits();
                    }
                    
                    
                }
                else
                {
                    // Begin new Selection
                    _selectable = hit.transform.GetComponent<ISelectable>();
                    if (_selectable != null)
                    {
                        if(Input.GetKey(KeyCode.LeftShift))
                        {
                            var unit = _selectable;
                            unit.OnSelect();

                            if(_multiSelectedUnit.Contains(unit))
                            {
                                DeSelectCurrentUnit(unit);
                            }
                            else
                                _multiSelectedUnit.Add(unit);
                            
                        }
                        else
                        {
                            DeSelectUnits();
                            _multiSelectedUnit.Add(_selectable);
                            _selectable.OnSelect();
                            displayInformation.UpdateSelectedUnit(_selectable.OnSelect());
                        }
                        return;
                       
                    }
                }
                
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // End Selection
                _isSelecting = false;
                /*if(_selectable==null) return;

                if (!IsWithinSelectionBounds(transform)) return;
                if (_currentSelections != null && _currentSelections != _selectable)
                {
                    foreach (var selection in _currentSelections)
                    {
                        selection.OnDeselect();
                    }
                }

                _currentSelection = _selectable;
                displayInformation.UpdateSelectedUnit(_currentSelection.OnSelect());*/
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (_multiSelectedUnit.Count == 1)
                {
                    _multiSelectedUnit[0].OnActionSelf();
                }

                if (_multiSelectedUnit.Count > 1)
                {
                    var mousePosition = Input.mousePosition;
                    var worldPosition = _cam.ScreenPointToRay(mousePosition);
                    if (Physics.Raycast(worldPosition, out RaycastHit hit, 5000, mapLayerMask))
                    {
                        {
                            hitPosition = hit.point;
                            //var destinations = PoissonDisc.GeneratePoints3D(pointCount: _multiSelectedUnit.Count,
                            //    mousePos: hit.point,
                            //    radius: 20,
                            //    sampleRegionSize: regionSize= Vector3.one * (_multiSelectedUnit.Count * 40));
                            //Debug.Log(destinations.Count);

                            var destinations = new List<Vector3>()
                            {
                                hit.point,
                                hit.point + Vector3.right * (10 * _multiSelectedUnit.Count),
                                hit.point + Vector3.left * (10 * _multiSelectedUnit.Count)
                            };
                            for (var index = 0; index < _multiSelectedUnit.Count; index++)
                            {
                                var selection = _multiSelectedUnit[index];
                                selection.OnActionAtPosition(destinations[index]);
                            }
                        }
                    }
                }
            }
            if(_isSelecting &&  _selectionStartPosition != Input.mousePosition)
            {
                SelectDragUnit();
            }
        }
        
        private void OnGUI()
        {
            if (_isSelecting)
            {
                var rect = Utils.GetScreenRect(_selectionStartPosition, Input.mousePosition);
                Utils.DrawScreenRect(rect, selectionColor);
                Utils.DrawScreenRectBorder(rect, 1, selectionBorderColor);
            }
        }
        private void SelectDragUnit()
        {
            if (!_isSelecting) return;

            _selectionBounds = Utils.GetViewportBounds(
                _cam,
                _selectionStartPosition,
                Input.mousePosition
            );
            
            List<ISelectable> unitsInBounds = new List<ISelectable>();
        
            // Query the Physics system for overlapping colliders
            var size = Physics.OverlapBoxNonAlloc(_cam.transform.position, Vector3.one * 300, results, _cam.transform.localRotation, unitLayerMask);
    
            foreach (Collider col in results)
            {
                if(col==null) continue;
                // If has ISelectable interface, add it to list
                ISelectable selectable = col.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    unitsInBounds.Add(selectable);
                }
            }
            
            foreach (ISelectable unit in unitsInBounds)
            {
                var inBounds = _selectionBounds.Contains(
                    _cam.WorldToViewportPoint(unit.GetTransform().position)
                );
                if (inBounds)
                {
                    if (!_multiSelectedUnit.Contains(unit))
                    {
                        unit.OnSelect();
                        _multiSelectedUnit.Add(unit);
                    }

                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        DeSelectCurrentUnit(unit);
                    }
                 
                }
            }
        }
        
        [ShowInInspector] private List<ISelectable> _allPlayerUnits;
        [ShowInInspector] private List<ISelectable> _multiSelectedUnit;
        private Bounds _selectionBounds;
        [SerializeField] private Vector3 regionSize;
        [SerializeField] private Vector3 hitPosition;

        private void DeSelectUnits()
        {
           
            displayInformation.RemoveSelection();
            foreach (var unit in _multiSelectedUnit)
            {
                unit.OnDeselect();
            }
            _multiSelectedUnit.Clear();
        }
        private void DeSelectCurrentUnit(ISelectable unit)
        {
            unit.OnDeselect();
            _multiSelectedUnit.Remove(unit);
        }

        public void AddUnitToList(ISelectable unit)
        {
            _allPlayerUnits.Add(unit);
        }
        private static Vector3 ComputePoints(Vector3 start,int size)
        {
            float angle = Random.value * Mathf.PI * 2;
            Vector3 dir = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            Vector3 candidate = start + dir * Random.Range(size, size * 2);

            return candidate;
        }
        private void OnDrawGizmos()
        {
            DebugExtension.DrawBounds(_selectionBounds,Color.red);
           
            Gizmos.DrawWireCube(hitPosition, regionSize);
        }
    }
}
