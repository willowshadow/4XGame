
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacementManager : MonoBehaviour
{
    public Material validPlacementMat;
    public Material invalidPlacementMat;

    public BuildingPlacement placementDummy;
    public BuildingPlacement placementDummyObject;
    
    public GameObject building;

    public LayerMask layersToHit;
    private new Camera camera;
    
    [SerializeField] private bool isPlacement;
    [SerializeField] private bool isPlaceState;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 placementPoint;
    [SerializeField] private Vector3 currentPos;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private bool isRotation;
    
    

    private void Awake()
    {
        camera = Camera.main;
    }

    public void SetPlacementState()
    {
        //Destroy(placementDummyObject.gameObject);
        isPlaceState = true;
        if(placementDummyObject!=null)
            Destroy(placementDummyObject.gameObject);
        placementDummyObject = Instantiate(placementDummy, transform);
    }
    public void SetNewBuildingDummy(BuildingPlacement newDummy)
    {
       
        placementDummy = newDummy;
    }

    public void SetNewBuilding(GameObject newBuilding)
    {
        building = newBuilding;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPlaceState = false;
            Destroy(placementDummyObject.gameObject);
        }
        if(!isPlaceState) return;
        
        var mousePosition = Input.mousePosition;
        var ray = camera.ScreenPointToRay(mousePosition);
        
        
        if (Physics.Raycast(ray, out RaycastHit hit,1000f,layersToHit))
        {
            
            if(!isRotation) placementDummyObject.SetPosition(hit.point);
            isPlacement = placementDummyObject.HasValidPlacement();
            placementDummyObject.SetMaterialInPlacementCheck(isPlacement ? validPlacementMat : invalidPlacementMat);
            
            
        }
        if(EventSystem.current.IsPointerOverGameObject()) return;

        #region Rotation Logics

        
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            placementPoint = hit.point;
            isRotation = true;
        }
        if (Input.GetMouseButton(0))
        {
            currentPos = Input.mousePosition;
            var delta = startPos - currentPos;
            var currentRot = placementDummyObject.GetRotation();
            
            var forward = hit.point - placementDummyObject.GetPosition();
            var lookRotation = Quaternion.LookRotation(forward, Vector3.up);
            var limitY = new Vector3(0, lookRotation.eulerAngles.y, 0);
            Debug.Log(forward,this);
            placementDummyObject.SetRotation(limitY);
        }
        if (Input.GetMouseButtonUp(0))
        {
            isRotation = false;
            if (isPlacement)
            {
                isPlaceState = false;
                var newBuilding = Instantiate(building, placementPoint, Quaternion.Euler(placementDummyObject.GetRotation()));
                newBuilding.GetComponent<BuildingPlacement>().SetPosition(placementPoint);
                Destroy(placementDummyObject.gameObject);
            }
        }
        #endregion
       
        if (Input.GetMouseButtonDown(0))
        {
           
        }
        
       
    }
}
