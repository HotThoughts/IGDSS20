using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public float scrollSpeed = 100f;
    private readonly float maxY = 85f;
    private readonly float minY = 25f;
    public float panSpeed = 7f;
    public Bounds bounds;
    public Camera cam;
    private Vector3 prev_pos;
    private Vector3 smoothCamPos;
    public GameManager gameManager;
    
    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        smoothCamPos = cam.transform.position; // Initial move
        InitializeBounds();
    }

    // Update is called once per frame
    void Update()
    {
        PanCam();
        SelectTile();
        ZoomCam();
    }

    private void LateUpdate()
    {
        Vector3 desiredPos = this.smoothCamPos;
        Vector3 smoothedPos = Vector3.Lerp(cam.transform.position, desiredPos, 0.125f);
        cam.transform.position = smoothedPos;
    }
    #endregion

    #region Methods
    // 
    void PanCam()
    {
        // right button
        if (Input.GetMouseButtonDown(1))
            prev_pos = cam.ScreenToViewportPoint(Input.mousePosition);
        if(Input.GetMouseButton(1))
        {
            // move the mouse pans the camera on X-Z plane
            Vector3 direction = prev_pos - cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 pos = cam.transform.position;
            pos.x += direction.y * panSpeed;
            pos.z -= direction.x * panSpeed;
            // boundaries
            pos.x = Mathf.Clamp(pos.x, bounds.min.x, bounds.max.x);
            pos.z = Mathf.Clamp(pos.z, bounds.min.z, bounds.max.z);

            cam.transform.position = pos;
            this.smoothCamPos = pos;
        }
    }
    // print the selected tile type 
    void SelectTile()
    {   // left button
        if(Input.GetMouseButtonDown(0))
        {
            // print the name of the hitted object
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = LayerMask.GetMask("Tiles");
            if (Physics.Raycast(ray, out hit, 1000f, layerMask))
            {
                Tile t = hit.collider.gameObject.GetComponent<Tile>() as Tile;
                Debug.Log("Tile type: " + t._type);
                gameManager.TileClicked(t._coordinateHeight, t._coordinateWidth);
            }
        }
    }
    // Zoom camera in and out by mouse scrolling
    void ZoomCam()
    {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        Vector3 camPos = cam.transform.position;

        if (scrollAxis != 0f)
        {
            Vector3 desiredPos = cam.transform.position + (cam.transform.forward * scrollSpeed) * scrollAxis;
            if ((camPos.y >= maxY-5f && scrollAxis < 0) || (camPos.y <= minY+5f && scrollAxis > 0))
            {
                desiredPos = camPos;
            }
            desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);
            desiredPos.x = Mathf.Clamp(desiredPos.x, bounds.min.x, bounds.max.x);
            desiredPos.z = Mathf.Clamp(desiredPos.z, bounds.min.z, bounds.max.z);
            this.smoothCamPos = desiredPos;
        }
    }

    void InitializeBounds()
    {
        // Determine scene bounds
        var rnds = FindObjectsOfType<Renderer>();
        if (rnds.Length == 0)
            return; // nothing to see here, go on

        var b = rnds[0].bounds;
        for (int i = 1; i < rnds.Length; i++)
            b.Encapsulate(rnds[i].bounds);
        this.bounds = b;
    }
    #endregion
}