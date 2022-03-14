using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    private GameInput _input;
    private Camera _mainCamera;
    [HideInInspector] public PathArm _currentlyHeldArm = null;
    public LayerMask pathLayerMask;
    public LayerMask towerLayerMask;
    public LayerMask planetLayerMask;
    private WaveSpawner _waveSpawner;
    private ShopManagerScript _shopManager;

    private GraphicRaycaster _gRaycaster;
    private EventSystem _eventSystem;
    private InfoPanel _infoPanel;

    private GameObject objectToPlace = null;    // make this a class and group together its price
    private int objectCost = -1;

    private Vector3 offScreenPoint = Vector3.up * 1000;

    public Planet lastPlanet = null;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _input = new GameInput();
        _waveSpawner = FindObjectOfType<WaveSpawner>();
        _shopManager = FindObjectOfType<ShopManagerScript>();
        _infoPanel = FindObjectOfType<InfoPanel>();
        _gRaycaster = FindObjectOfType<GraphicRaycaster>();
        _eventSystem = FindObjectOfType<EventSystem>();

        _infoPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _input.Mouse.Enable();
    }

    private void OnDisable()
    {
        _input.Mouse.Disable();
    }

    private void Start()
    {
        _input.Mouse.ClickPath.started += _ => StartedClick();
        _input.Mouse.ClickPath.performed += _ => EndedClick();

        _input.Mouse.Click.started += _ => StartedTurretClick();
    }


    private void PlaceObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_input.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDistance: 1000.0f, layerMask: pathLayerMask);
        if (hit.collider != null)
        {
            // check placing an item first:
            if (hit.collider.CompareTag("MovablePlatform")) // can only place on platforms
            {
                var armEnd = hit.collider.gameObject.GetComponent<ArmEnd>();
                // separate branch logic
                var branch = objectToPlace.GetComponent<PathArm>();
                if(branch != null && _shopManager.Purchase(objectCost, Planet.PlanetResourceType.COIN))
                {
                    objectToPlace.transform.position = armEnd.armEndPoint.position;
                    objectToPlace.transform.rotation = Quaternion.identity;
                    objectToPlace.transform.SetParent(armEnd.armEndPoint);

                    objectToPlace = null;
                }

                // reduce coins
                else if (armEnd.isAvailable() && _shopManager.Purchase(objectCost, Planet.PlanetResourceType.COIN))  // short circuit purchase
                {
                    armEnd.PlaceStructure();
                    // spawn the object as a child of the end of the platform
                    objectToPlace.transform.position = hit.point;
                    objectToPlace.transform.rotation = Quaternion.identity;
                    objectToPlace.transform.SetParent(hit.collider.transform);
                    var placable = objectToPlace.GetComponent<PlacableObject>();
                    if(placable != null)
                    {
                        placable.OnPlace(armEnd);
                    }
                    objectToPlace = null;
                }
            }
        }
        if(objectToPlace != null)
        {
            Destroy(objectToPlace);
            objectToPlace = null; // disable placing mode
        }
    }

    public void PreviewObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_input.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDistance: 1000.0f, layerMask: pathLayerMask);
        if (hit.collider != null)
        {
            // check placing an item first:
            var branch = objectToPlace.GetComponent<PathArm>();
            if(branch != null)
            {
                // we have a branch node on our hands:
                objectToPlace.transform.position = hit.collider.gameObject.transform.position;
            }
            else if (objectToPlace != null && hit.collider.CompareTag("MovablePlatform") &&
                hit.collider.GetComponent<ArmEnd>().isAvailable()) // can only place on platforms
            {
                // move the test obj to da spot
                objectToPlace.transform.position = hit.point;
            }
        }
        else
        {
            objectToPlace.transform.position = offScreenPoint;
        }
    }

    public void PreviewPlanet()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_input.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDistance: 1000.0f, layerMask: planetLayerMask);
        if (hit.collider != null)
        {
            var planet = hit.collider.GetComponent<Planet>();
            lastPlanet = planet;
            planet.EnableUI();
        }
        else
        {
            if(lastPlanet != null)
            {
                lastPlanet.DisableUI();
                lastPlanet = null;
            }
        }
    }

    public void ClickStructure()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_input.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDistance: 1000.0f, layerMask: towerLayerMask);
        if (hit.collider != null)
        {
            BaseTurret turret = hit.collider.GetComponent<BaseTurret>();
            if (turret != null)
            {
                _infoPanel.gameObject.SetActive(true);
                _infoPanel.SetTurret(turret);
            }
            else
            {
                var mine = hit.collider.GetComponent<MiningBase>();
                if(mine != null)
                {
                    _infoPanel.gameObject.SetActive(true);
                    _infoPanel.SetMine(mine);
                }
            }

        }
        else
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Didn't hit the UI?");
                _infoPanel.DeselectAll();
                _infoPanel.gameObject.SetActive(false);
            }
        }
    }

    public void MoveArm()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_input.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDistance: 1000.0f, layerMask: pathLayerMask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("MovablePlatform") && _waveSpawner._enemiesInPlay <= 0)
            {
                _currentlyHeldArm = hit.collider.GetComponent<ArmEnd>().parentArmOrigin;
            }
        }
    }

    private void StartedClick()
    {
        MoveArm();
    }

    private void StartedTurretClick()
    {
        if(objectToPlace != null)
        {
            PlaceObject();
        }
        else
        {
            ClickStructure();
        }
    }
    
    private void EndedClick()
    {
        _currentlyHeldArm = null;
    }

    private void MoveCurrentPathArm()
    {
        Vector3 currentArmPosition = _currentlyHeldArm.transform.position;
        // get where the forward of the arm should be based on mouse position
        float camToFloorDistance = Mathf.Abs(_mainCamera.transform.position.y - currentArmPosition.y);
        // get where the mouse is
        Vector2 screenSpacePos = _input.Mouse.Position.ReadValue<Vector2>();
        Vector3 worldMousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenSpacePos.x,
            screenSpacePos.y, camToFloorDistance));
        Vector3 difference = worldMousePosition - currentArmPosition;
        // set the rotation of the arm to look at the point where the mouse is
        _currentlyHeldArm.transform.rotation = Quaternion.LookRotation(difference) * 
                                               Quaternion.Euler(new Vector3(0, -90, 0));
    }

    private void Update()
    {
        if(objectToPlace != null)
        {
            // preview Object
            PreviewObject();
        }
        // handle player holding down mouse button after clicking on an arm
        if (_input.Mouse.ClickPath.IsPressed() && _currentlyHeldArm != null &&
            !_waveSpawner.waveActive)
        {
            MoveCurrentPathArm();
        }
        PreviewPlanet();
    }
    
    // buy item call
    public void BuyItem(GameObject item, int cost)
    {
        // set up cursor for placing an object
        objectCost = cost;
        objectToPlace = Instantiate(item, offScreenPoint, Quaternion.identity);
    }
}
