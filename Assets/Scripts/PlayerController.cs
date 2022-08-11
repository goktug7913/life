using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject PlayerRoot;
    public GameObject CameraRoot;
    public GameObject _Camera;

    public float moveSpeed = 3f;
    public float zoomSpeed = 3f;
    public float orbitSpeed = 1f;
    private float orbitMultiplier = 1000f;
    public bool invertY = true;

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
        PlayerRoot.transform.position += movement * Time.deltaTime * moveSpeed;

        
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

        // scroll zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // lerp the camera's field of view to a new value.
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Camera.main.fieldOfView - scroll * zoomSpeed, Time.deltaTime);
        }
    }
}
