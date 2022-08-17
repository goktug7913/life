using System.Collections.Generic;
using Creature.V2;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("PlayerRoot")] public GameObject playerRoot;
    [FormerlySerializedAs("CameraRoot")] public GameObject cameraRoot;
    public Camera _Camera;

    public float moveSpeed = 3f;
    public float zoomSpeed = 10f;
    public float orbitSpeed = 1f;
    float _orbitMultiplier = 1000f;
    float _moveMultiplier = 10f;
    public bool invertY = true;

    public bool canMove = true;
    public bool canZoom = true;
    public bool canOrbit = true;

    List<LifeBaseV2> _selectedLife;

    SimulationManager _simulationManager;
    
    // Start is called before the first frame update
    private void Start()
    {
        _selectedLife = new List<LifeBaseV2>(); // initialize the list.
        // Funny note: I forgot to initialize this list, and it was causing a null reference exception.
        // It took me a few hours to find out. I was almost deleting the whole selection script.

        // Find the manager in the level
        _simulationManager = FindObjectOfType<SimulationManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        SelectLife();
        // Spawn a pair of animals on right click.
        if (Input.GetMouseButtonDown(1))
        {
            //SpawnNewSpeciesPair(LifeBaseV2.Genus.Animalia);
            SpawnLifeV3();
        }
    }

    private void FixedUpdate()
    {
        // I moved this to FixedUpdate because it is supposed to be more stable for the camera and movement.
        CameraMove();
        CameraOrbit();
        ScrollZoom();
    }

    private void SelectLife()
    {
        // Basic selection system.
        
        if (!Input.GetMouseButtonDown(0)) return;
        
        Ray ray = _Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;
        
        Debug.Log("Hit " + hit.transform.name);
        
        var lifebase = hit.transform.GetComponent<LifeBaseV2>();
        if (lifebase)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                // Todo        
            }
            else
            {
                _selectedLife.Clear();
                _selectedLife.Add(lifebase);
            }
        }

        // Iterate through the selected life and show the info card.
        foreach (LifeBaseV2 life in _selectedLife)
        {
            life.infoCard.SetVisibility(true);
        }
    }

    private void CameraMove()
    {
        if (!canMove){return;} // If can't move, return.
        
        // WASD movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal == 0 && moveVertical == 0){return;} // If no input, return.

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // rotate movement with camera.
        movement = Quaternion.Euler(0, cameraRoot.transform.rotation.eulerAngles.y, 0) * movement;

        // add modifiers and move.
        movement = movement * (moveSpeed * _moveMultiplier * Time.deltaTime);
        playerRoot.transform.Translate(movement);
    }

    private void CameraOrbit()
    {
        if (!canOrbit) return; // If can't orbit, return.

        if (!Input.GetMouseButton(2)) return;
        // get cursor position
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // apply inversion if enabled
        if (invertY)
        {
            mouseY *= -1;
        }

        // mouseX is the horizontal distance, mouseY is the vertical distance.
        Vector3 mouseMovement = new Vector3(mouseY, 0f, mouseX) * (Time.deltaTime * orbitSpeed * _orbitMultiplier);
        cameraRoot.transform.Rotate(mouseY,mouseX,0);

        // we need to zero out Z axis... (fix later)
        var rotation = cameraRoot.transform.rotation;
        rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        cameraRoot.transform.rotation = rotation;
    }

    private void ScrollZoom()
    {
        if (!canZoom){return;} // If can't zoom, return.

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0){return;} // if no scroll, return.

        // change camera distance based on scroll.
        float zoom = scroll * zoomSpeed;

        // move camera
        _Camera.transform.Translate(0, 0, zoom);
    }
    
    void SpawnLifeV3()
    {
        // Get the mouse position in world space.
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 worldPos = _Camera.ScreenToWorldPoint(mousePos);
        // Move the Y position to the ground.
        worldPos.y = 3;
        
        EventManager.current.RequestLifeSpawn(worldPos);
    }
}


