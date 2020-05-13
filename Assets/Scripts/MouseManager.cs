using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    public float scrollSpeed = 10f;
    public float panSpeed = 0.1f;
    public Vector3 panLimit;

    public float minY = 0f;
    public float maxY = 100f;
    public Camera cam;
    private Vector3 prev_pos;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // right botton
        if (Input.GetMouseButtonDown(1))
            prev_pos = cam.ScreenToViewportPoint(Input.mousePosition);
        if(Input.GetMouseButton(1)){ 
            // move the mouse pans the camera on X-Z plane
            Vector3 direction = prev_pos - cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 pos = cam.transform.position;
            pos.x -= direction.x * panSpeed;
            pos.z -= direction.y * panSpeed;
            // boundaries
            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

            cam.transform.position = pos;//Vector3.Lerp(prev_pos, pos, Time.deltaTime);
        }
            
        // left botton
        if(Input.GetMouseButtonDown(0)){
            // print the name of the hitted object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                print(hit.collider.gameObject.name);
                
        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0f){
            // zoom in and out
            cam.fieldOfView -= scrollSpeed * Input.GetAxis("Mouse ScrollWheel");
            // limits
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minY, maxY);
        }
    }
}
