using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float zoomSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // WASD movement.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.position += movement * Time.deltaTime * moveSpeed;

        // Hold middle mouse button to orbit
        if (Input.GetMouseButton(2))
        {
            // Get the mouse position in world space.
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            // Rotate the camera to look at the mouse position.
            transform.LookAt(worldPos);
        }

        // scroll zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // lerp the camera's field of view to a new value.
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Camera.main.fieldOfView - scroll * zoomSpeed, Time.deltaTime);
        }
    }
}
