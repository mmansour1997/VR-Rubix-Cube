using System.Collections;
using System.Collections.Generic;
using OVRTouchSample;
using UnityEngine;

public class Pointer2 : MonoBehaviour {
    public float default_length = 15.0f;
    public GameObject Dot;
    public Ray ray;
    RaycastHit hit;
    private LineRenderer lineRenderer = null;

    public InstCube BigCube;
    List<GameObject> cublets = new List<GameObject> (),
        planes = new List<GameObject> ();

    private void Awake () {
        lineRenderer = GetComponent<LineRenderer> ();
        lineRenderer.material = new Material (Shader.Find ("Sprites/Default"));
        lineRenderer.SetColors (Color.red, Color.red);
    }

    // Update is called once per frame
    void Update () {
        UpdateLine ();
    }

    private void UpdateLine () {
        float targetLength = default_length;

        hit = CreateRaycast (targetLength);
        Vector3 endPosition = transform.position + (transform.forward * targetLength * 2);

        if (hit.collider != null)
            endPosition = hit.point;

        Dot.transform.position = endPosition;

        lineRenderer.SetPosition (0, transform.position);
        lineRenderer.SetPosition (1, endPosition);

    }

    public RaycastHit CreateRaycast (float length) {
        float checkInputTrigger = OVRInput.Get (OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
        if ((checkInputTrigger > 0.1f)) {
            RaycastHit hit;
            ray = new Ray (transform.position, transform.forward);
            Physics.Raycast (ray, out hit, default_length);

            if (Physics.Raycast (ray, out hit, default_length)) {

                if (cublets.Count < 2 &&
                    !cublets.Exists (x => x == hit.collider.transform.parent.gameObject) &&
                    hit.transform.parent.gameObject != BigCube.gameObject) {
                    cublets.Add (hit.collider.transform.parent.gameObject);
                    planes.Add (hit.collider.gameObject);
                } else if (cublets.Count == 2) {
                    BigCube.DetectRotate (cublets, planes);

                }
            }
        } else if ((checkInputTrigger < 0.1f)) {
            cublets.Clear ();
            planes.Clear ();

        }

        return hit;
    }
}