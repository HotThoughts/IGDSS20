﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public float scrollSpeed = 100f;
    private float maxY = 85f;
    private float minY = 25f;
    public float panSpeed;
    public Bounds bounds;
    public Camera cam;
    private Vector3 prev_pos;
    private Vector3 smoothCamPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        smoothCamPos = cam.transform.position; // Initial move
        // Determine scene bounds
        var rnds = FindObjectsOfType<Renderer>();
        if (rnds.Length == 0)
            return; // nothing to see here, go on

        var b = rnds[0].bounds;
        for (int i = 1; i < rnds.Length; i++)
            b.Encapsulate(rnds[i].bounds);
        this.bounds = b;
    }

    // Update is called once per frame
    void Update()
    {
        // right button
        if (Input.GetMouseButtonDown(1))
            prev_pos = cam.ScreenToViewportPoint(Input.mousePosition);
        if(Input.GetMouseButton(1)){
            // move the mouse pans the camera on X-Z plane
            Vector3 direction = prev_pos - cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 pos = cam.transform.position;
            pos.x += direction.y * panSpeed;
            pos.z -= direction.x * panSpeed;
            // boundaries
            pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
            pos.z = Mathf.Clamp(pos.z, bounds.min.z, bounds.max.z);

            cam.transform.position = pos;//Vector3.Lerp(prev_pos, pos, Time.deltaTime);
            smoothCamPos = pos;
        }
            
        // left button
        if(Input.GetMouseButtonDown(0)){
            // print the name of the hitted object
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                print(hit.collider.gameObject.name);
        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0f){
            // zoom in and out
            Vector3 pos = cam.transform.position;
            var scrollAxis = Input.GetAxis("Mouse ScrollWheel");
            // Condition for fixing change of X axis while Y is max / min
            if (pos.y <= maxY - 5f && pos.y >= minY + 5f)
            {
                pos.x -= scrollSpeed * scrollAxis;
                pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
            }
            pos.y -= scrollSpeed * scrollAxis;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            this.smoothCamPos = pos;
        }
    }

    private void LateUpdate()
    {
        Vector3 desiredPos = this.smoothCamPos;
        Vector3 smoothedPos = Vector3.Lerp(cam.transform.position, desiredPos, 0.125f);
        cam.transform.position = smoothedPos;
    }
}
