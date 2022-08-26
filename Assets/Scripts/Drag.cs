using UnityEngine;

public class Drag : MonoBehaviour
{

    private Vector3 dragOffset;
    public Camera cam;

    public bool dragEnabled;


    void Awake()
    {
        dragEnabled = true;

        if (!cam)
        {
            cam = Camera.main;
        }
    }

    void OnMouseDown()
    {
        dragOffset = transform.position - GetMousePos();
    }


    void OnMouseDrag()
    {
        if (dragEnabled)
        {
            transform.position = GetMousePos() + dragOffset;
        }
    }


    Vector3 GetMousePos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = - cam.transform.position.z + transform.position.z;
        return cam.ScreenToWorldPoint(mousePos);
    }

    public void enableDrag()
    {
        dragEnabled = true;
    }
    public void disableDrag()
    {
        dragEnabled = false;
    }

}