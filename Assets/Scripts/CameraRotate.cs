using System.Collections;
using System.Collections.Generic;
using OVRTouchSample;
using UnityEngine;

public class CameraRotate : MonoBehaviour {
    bool DisableCamera = false,
        rotationDisable = false;
    Vector3 CamRotation = new Vector3 (-90, 0, 0);
    //public Camera cam;
    public InstCube BigCube;
    List<GameObject> cublets = new List<GameObject> (),
        planes = new List<GameObject> ();
    // Update is called once per frame   
    void LateUpdate () {
        rotationDisable = true;
        Vector2 get2d1 = OVRInput.Get (OVRInput.Axis2D.PrimaryThumbstick);
        Vector2 get2d2 = OVRInput.Get (OVRInput.Axis2D.SecondaryThumbstick);

        if (get2d1.x != 0 || get2d1.y != 0) {
            CamRotation.x += get2d1.x;
            CamRotation.y += get2d1.y;
            CamRotation.y = Mathf.Clamp (CamRotation.y, -90, 90);
        } else if (get2d2.x != 0 || get2d2.y != 0) {
            CamRotation.x += get2d2.x;
            CamRotation.y += get2d2.y;
            CamRotation.y = Mathf.Clamp (CamRotation.y, -90, 90);
        }
        Quaternion qt = Quaternion.Euler (CamRotation.y, CamRotation.x, 0);
        transform.parent.rotation = Quaternion.Lerp (transform.parent.rotation, qt, Time.deltaTime * 15);
    }
}