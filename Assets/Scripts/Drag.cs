using UnityEngine;
using DG.Tweening;
public class Drag : MonoBehaviour
{

    private Vector3 parentDragOffset;

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
        parentDragOffset = transform.parent.position - GetMousePos();
    }


    void OnMouseDrag()
    {
        if (dragEnabled)
        {
            if(transform.parent.CompareTag("Palabra"))
            {
                if (transform.parent)
                {
                    transform.parent.DOMove(GetMousePos() + parentDragOffset, 0.1f);
                }
            }
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