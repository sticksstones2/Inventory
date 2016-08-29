using UnityEngine;
using System.Collections;

public class MouseInputController : MonoBehaviour
{
    public GameObject pointer;

    void Update()
    {


        pointer.transform.position = MouseWorldPos();

        if (Input.GetButtonDown("Fire1"))
        {
            Mouse1Down();
        }
    }

    Vector3 MouseWorldPos()
    {
        Camera camera = GetComponent<Camera>();
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(mousePos);
    }

    void Mouse1Down()
    {
        print("fire");
    }
}

