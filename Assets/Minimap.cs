using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {
    Vector3 position;
    public Camera cam;
    // Use this for initialization
    void Start ()
    {
        float aspect = (float) Screen.width / Screen.height;
        float ratio = (float) cam.pixelHeight / cam.pixelWidth;
        float camHeight = ((float)(Screen.height /5) / Screen.height )* aspect;
        float camWidth = ((float)(Screen.height / 5) / Screen.height) * ratio;

        cam.rect = new Rect(1-camWidth, 0, camWidth, camHeight);

    }
}
