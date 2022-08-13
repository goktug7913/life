using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject PlayerRoot;
    public GameObject CameraRoot;
    public GameObject _Camera;

    public float moveSpeed = 1f;
    public float zoomSpeed = 5000f;
    public float orbitSpeed = 1f;
    private float orbitMultiplier = 1000f;
    private float moveMultiplier = 10f;
    public bool invertY = true;

    public bool canMove = true;
    public bool canZoom = true;
    public bool canOrbit = true;

    List<LifeBaseV2> selectedLife;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectLife();
    }

    void FixedUpdate()
    {
        // I moved this to FixedUpdate because it is supposed to be more stable for the camera and movement.
        CameraMove();
        CameraOrbit();
        ScrollZoom();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);

        if (selectedLife != null)
        {
            foreach (LifeBaseV2 life in selectedLife)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(life.transform.position, 0.1f);
            }
        }
    }

    void SelectLife()
    {
        // Basic selection system.
        // Selects a life if you click on it.
        // Deselects all when you click on nothing.
        // Deselects all when you click on another life.
        // Adds selection if CTRL is held.
        // Removes selection if SHIFT is held.

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Life")
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        // Add to selection.
                        if (!selectedLife.Contains(hit.transform.gameObject.GetComponent<LifeBaseV2>()))
                        {
                            selectedLife.Add(hit.transform.gameObject.GetComponent<LifeBaseV2>());
                            Debug.Log("Added to selection");
                        }
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        // Remove from selection.
                        if (selectedLife.Contains(hit.transform.gameObject.GetComponent<LifeBaseV2>()))
                        {
                            selectedLife.Remove(hit.transform.gameObject.GetComponent<LifeBaseV2>());
                            Debug.Log("Removed from selection");
                        }
                    }
                    else
                    {
                        // Deselect all and select single.
                        selectedLife.Clear();
                        selectedLife.Add(hit.transform.gameObject.GetComponent<LifeBaseV2>());
                        Debug.Log("Selected single");
                    }
                }
                else
                {
                    // Deselect all if you click on nothing, iterate through all selected life and deselect them.
                    foreach (LifeBaseV2 life in selectedLife)
                    {
                        life.infoCard.SetVisibility(false);
                    }
                    selectedLife.Clear();
                }

                // Iterate through the selected life and show the info card.
                foreach (LifeBaseV2 life in selectedLife)
                    {
                        life.infoCard.SetVisibility(true);
                    }
            }
        }
    }

    void CameraMove()
    {
        if (!canMove){return;} // If can't move, return.
        
        // WASD movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal == 0 && moveVertical == 0){return;} // If no input, return.

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // rotate movement with camera.
        movement = Quaternion.Euler(0, CameraRoot.transform.rotation.eulerAngles.y, 0) * movement;

        // add modifiers and move.
        movement = movement * moveSpeed * moveMultiplier *  Time.deltaTime;
        PlayerRoot.transform.Translate(movement);
    }

    void CameraOrbit()
    {
        if (!canOrbit){return;} // If can't orbit, return.

        if (Input.GetMouseButton(2))
        {
            // get cursor position
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // apply inversion if enabled
            if (invertY)
            {
                mouseY *= -1;
            }

            // mouseX is the horizontal distance, mouseY is the vertical distance.
            Vector3 mouseMovement = new Vector3(mouseY, 0f, mouseX) * Time.deltaTime * orbitSpeed * orbitMultiplier;
            CameraRoot.transform.Rotate(mouseY,mouseX,0);

            // we need to zero out Z axis... (fix later)
            CameraRoot.transform.rotation = Quaternion.Euler(CameraRoot.transform.rotation.eulerAngles.x, CameraRoot.transform.rotation.eulerAngles.y, 0);
        }
    }

    void ScrollZoom()
    {
        if (!canZoom){return;} // If can't zoom, return.

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0){return;} // if no scroll, return.

        // change camera distance based on scroll.
        float zoom = scroll * zoomSpeed;

        // move camera
        _Camera.transform.Translate(0, 0, zoom);
    }
}


