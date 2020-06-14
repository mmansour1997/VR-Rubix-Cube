using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class InstCube : MonoBehaviour {
    public GameObject Cubelet;
    Transform CubeTran;
    List<GameObject> AllCubes = new List<GameObject> ();
    GameObject CubeCenter;
    public bool canRotate = true;
    public bool canShuffle = true;
    public bool started = true;

    float rotateSpeed = 5;
    public bool isCompleted = false;
    public bool done = false;
    public Text win;



    List<GameObject> UpPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.y) == 0);
        }
    }
    List<GameObject> DownPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.y) == -2);
        }
    }
    List<GameObject> RightPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.z) == 2);
        }
    }
    List<GameObject> LeftPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.z) == 0);
        }
    }
    List<GameObject> FrontPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.x) == 0);
        }
    }
    List<GameObject> BackPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.x) == -2);
        }
    }
    List<GameObject> UpHorizontalPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.x) == -1);
        }
    }
    List<GameObject> UpVerticalPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.z) == 1);
        }
    }
    List<GameObject> FrontHorizontalPieces {
        get {
            return AllCubes.FindAll (x => Mathf.Round (x.transform.localPosition.y) == -1);
        }
    }

    // Start is called before the first frame update

    void Start () {
        CreateCube ();
        StartCoroutine (shuffleCube ());
    }

    // Update is called once per frame
    void Update () {
        InputButton ();
    }
    void InputButton () {
        if (canRotate) {
            if (Input.GetKeyDown (KeyCode.W)) {
                StartCoroutine (Rotate (UpPieces, new Vector3 (0, 1, 0), rotateSpeed));
            } else if (Input.GetKeyDown (KeyCode.S)) {
                StartCoroutine (Rotate (DownPieces, new Vector3 (0, -1, 0), rotateSpeed));
            } else if (Input.GetKeyDown (KeyCode.E)) {
                StartCoroutine (Rotate (FrontPieces, new Vector3 (1, 0, 0), rotateSpeed));
            } else if (Input.GetKeyDown (KeyCode.Q)) {
                StartCoroutine (Rotate (BackPieces, new Vector3 (-1, 0, 0), rotateSpeed));
            } else if (Input.GetKeyDown (KeyCode.D)) {
                StartCoroutine (Rotate (RightPieces, new Vector3 (0, 0, 1), rotateSpeed));
            } else if (Input.GetKeyDown (KeyCode.A)) {
                StartCoroutine (Rotate (LeftPieces, new Vector3 (0, 0, -1), rotateSpeed));
            } else if (Input.GetKeyDown (KeyCode.K) && canShuffle || (OVRInput.Get (OVRInput.Button.Two) && canShuffle)) {
                StartCoroutine (shuffleCube ());
            } else if (OVRInput.Get (OVRInput.Button.One) || Input.GetKeyDown (KeyCode.R)) {
                CreateCube ();
            }
        }

    }

    void CreateCube () {

        foreach (GameObject cube in AllCubes) {
            DestroyImmediate (cube);
        }

        AllCubes.Clear ();
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    GameObject cube = Instantiate (Cubelet, CubeTran, false);
                    cube.transform.localPosition = new Vector3 (-x, -y, z);
                    cube.GetComponent<PrefabScript> ().SetColor (-x, -y, z);
                    AllCubes.Add (cube);
                }
            }
        }
        CubeCenter = AllCubes[13];
    }

    Vector3[] RotateVectors = {
        new Vector3 (0, 1, 0),
        new Vector3 (0, -1, 0),
        new Vector3 (0, 0, -1),
        new Vector3 (0, 0, 1),
        new Vector3 (1, 0, 0),
        new Vector3 (-1, 0, 0)
    };
    IEnumerator shuffleCube () {
        canShuffle = false;

        for (int MoveNum = Random.Range (15, 20); MoveNum >= 0; MoveNum--) {
            List<GameObject> EdgeCublets = new List<GameObject> ();
            int randomEdge = Random.Range (0, 6); // choose random sides of the cube

            switch (randomEdge) {
                case 0:
                    EdgeCublets = UpPieces;
                    break;
                case 1:
                    EdgeCublets = DownPieces;
                    break;
                case 2:
                    EdgeCublets = RightPieces;
                    break;
                case 3:
                    EdgeCublets = LeftPieces;
                    break;
                case 4:
                    EdgeCublets = FrontPieces;
                    break;
                case 5:
                    EdgeCublets = BackPieces;
                    break;
            }

            StartCoroutine (Rotate (EdgeCublets, RotateVectors[randomEdge], 25));
            yield return new WaitForSeconds (0.75f);
        }
        canShuffle = true;
    }

    IEnumerator Rotate (List<GameObject> pieces, Vector3 rotationvec, float speed = 5) {

        canRotate = false;
        int angle = 0;
        while (angle < 90) {
            foreach (GameObject cube in pieces) {
                cube.transform.RotateAround (CubeCenter.transform.position, rotationvec, speed);

            }
            angle += 5;
            yield return null;
        }
        CubeIsComplete ();
        canRotate = true;

    }

    public void DetectRotate (List<GameObject> cublets, List<GameObject> planes) {
        if (!canRotate || !canShuffle)
            return;

        if (UpVerticalPieces.Exists (x => x == cublets[0]) &&
            UpVerticalPieces.Exists (x => x == cublets[1]))
            StartCoroutine (Rotate (UpVerticalPieces, new Vector3 (0, 0, 1 * DetectLeftMiddleRightSign (cublets))));

        else if (UpHorizontalPieces.Exists (x => x == cublets[0]) &&
            UpHorizontalPieces.Exists (x => x == cublets[1]))
            StartCoroutine (Rotate (UpHorizontalPieces, new Vector3 (1 * DetectFrontMiddleBackSign (cublets), 0, 0)));

        else if (FrontHorizontalPieces.Exists (x => x == cublets[0]) &&
            FrontHorizontalPieces.Exists (x => x == cublets[1]))
            StartCoroutine (Rotate (FrontHorizontalPieces, new Vector3 (0, 1 * DetectUpMiddleDownSign (cublets), 0)));

        else if (DetectSide (planes, new Vector3 (1, 0, 0), new Vector3 (0, 0, 1), UpPieces))
            StartCoroutine (Rotate (UpPieces, new Vector3 (0, 1 * DetectUpMiddleDownSign (cublets), 0)));

        else if (DetectSide (planes, new Vector3 (1, 0, 0), new Vector3 (0, 0, 1), DownPieces))
            StartCoroutine (Rotate (DownPieces, new Vector3 (0, 1 * DetectUpMiddleDownSign (cublets), 0)));

        else if (DetectSide (planes, new Vector3 (0, 0, 1), new Vector3 (0, 1, 0), FrontPieces))
            StartCoroutine (Rotate (FrontPieces, new Vector3 (1 * DetectFrontMiddleBackSign (cublets), 0, 0)));

        else if (DetectSide (planes, new Vector3 (0, 0, 1), new Vector3 (0, 1, 0), BackPieces))
            StartCoroutine (Rotate (BackPieces, new Vector3 (1 * DetectFrontMiddleBackSign (cublets), 0, 0)));

        else if (DetectSide (planes, new Vector3 (1, 0, 0), new Vector3 (0, 1, 0), LeftPieces))
            StartCoroutine (Rotate (LeftPieces, new Vector3 (0, 0, 1 * DetectLeftMiddleRightSign (cublets))));

        else if (DetectSide (planes, new Vector3 (1, 0, 0), new Vector3 (0, 1, 0), RightPieces))
            StartCoroutine (Rotate (RightPieces, new Vector3 (0, 0, 1 * DetectLeftMiddleRightSign (cublets))));

    }

    bool DetectSide (List<GameObject> planes, Vector3 fDirection, Vector3 sDirection, List<GameObject> side) {
        GameObject centerPiece = side.Find (x => x.GetComponent<PrefabScript> ().Planes.FindAll (y => y.activeInHierarchy).Count == 1);

        List<RaycastHit> hit1 = new List<RaycastHit> (Physics.RaycastAll (planes[1].transform.position, fDirection)),
            hit2 = new List<RaycastHit> (Physics.RaycastAll (planes[0].transform.position, fDirection)),
            hit1_m = new List<RaycastHit> (Physics.RaycastAll (planes[1].transform.position, -fDirection)),
            hit2_m = new List<RaycastHit> (Physics.RaycastAll (planes[0].transform.position, -fDirection)),

            hit3 = new List<RaycastHit> (Physics.RaycastAll (planes[1].transform.position, sDirection)),
            hit4 = new List<RaycastHit> (Physics.RaycastAll (planes[0].transform.position, sDirection)),
            hit3_m = new List<RaycastHit> (Physics.RaycastAll (planes[1].transform.position, -sDirection)),
            hit4_m = new List<RaycastHit> (Physics.RaycastAll (planes[0].transform.position, -sDirection));

        return hit1.Exists (x => x.collider.gameObject == centerPiece) ||
            hit2.Exists (x => x.collider.gameObject == centerPiece) ||
            hit1_m.Exists (x => x.collider.gameObject == centerPiece) ||
            hit2_m.Exists (x => x.collider.gameObject == centerPiece) ||

            hit3.Exists (x => x.collider.gameObject == centerPiece) ||
            hit4.Exists (x => x.collider.gameObject == centerPiece) ||
            hit3_m.Exists (x => x.collider.gameObject == centerPiece) ||
            hit4_m.Exists (x => x.collider.gameObject == centerPiece);

    }

    float DetectLeftMiddleRightSign (List<GameObject> cublets) {
        float sign = 0;

        if (Mathf.Round (cublets[1].transform.position.y) != Mathf.Round (cublets[0].transform.position.y)) {
            if (Mathf.Round (cublets[0].transform.position.x) == -2)
                sign = Mathf.Round (cublets[0].transform.position.y) - Mathf.Round (cublets[1].transform.position.y);
            else
                sign = Mathf.Round (cublets[1].transform.position.y) - Mathf.Round (cublets[0].transform.position.y);
        } else {
            if (Mathf.Round (cublets[0].transform.position.y) == -2)
                sign = Mathf.Round (cublets[1].transform.position.x) - Mathf.Round (cublets[0].transform.position.x);
            else
                sign = Mathf.Round (cublets[0].transform.position.x) - Mathf.Round (cublets[1].transform.position.x);
        }

        return sign;
    }
    float DetectFrontMiddleBackSign (List<GameObject> cublets) {
        float sign = 0;

        if (Mathf.Round (cublets[1].transform.position.z) != Mathf.Round (cublets[0].transform.position.z)) {
            if (Mathf.Round (cublets[0].transform.position.y) == 0)
                sign = Mathf.Round (cublets[1].transform.position.z) - Mathf.Round (cublets[0].transform.position.z);
            else
                sign = Mathf.Round (cublets[0].transform.position.z) - Mathf.Round (cublets[1].transform.position.z);
        } else {
            if (Mathf.Round (cublets[0].transform.position.z) == 0)
                sign = Mathf.Round (cublets[1].transform.position.y) - Mathf.Round (cublets[0].transform.position.y);
            else
                sign = Mathf.Round (cublets[0].transform.position.y) - Mathf.Round (cublets[1].transform.position.y);
        }

        return sign;
    }
    float DetectUpMiddleDownSign (List<GameObject> cublets) {
        float sign = 0;

        if (Mathf.Round (cublets[1].transform.position.z) != Mathf.Round (cublets[0].transform.position.z)) {
            if (Mathf.Round (cublets[0].transform.position.x) == -2)
                sign = Mathf.Round (cublets[1].transform.position.z) - Mathf.Round (cublets[0].transform.position.z);
            else
                sign = Mathf.Round (cublets[0].transform.position.z) - Mathf.Round (cublets[1].transform.position.z);
        } else {
            if (Mathf.Round (cublets[0].transform.position.z) == 0)
                sign = Mathf.Round (cublets[0].transform.position.x) - Mathf.Round (cublets[1].transform.position.x);
            else
                sign = Mathf.Round (cublets[1].transform.position.y) - Mathf.Round (cublets[0].transform.position.x);
        }

        return sign;
    }

    void CubeIsComplete () {

        if (IsSideComplete(UpPieces) &&
            IsSideComplete(DownPieces) &&
            IsSideComplete(LeftPieces) &&
            IsSideComplete(RightPieces) &&
            IsSideComplete(FrontPieces) &&
            IsSideComplete(BackPieces))
        {
            Debug.Log("Cube complete!");
            isCompleted = true;
            done = true;
            win.GetComponent<Text>().text = "WON!";

        }
        else
        {
            isCompleted = false;
        }
        
    }

    bool IsSideComplete (List<GameObject> pieces) {

        int getPlaneIndex = pieces[4].GetComponent<PrefabScript> ().Planes.FindIndex (x => x.activeInHierarchy);

        for (int i = 0; i < pieces.Count; i++) {
            if (!pieces[i].GetComponent<PrefabScript> ().Planes[getPlaneIndex].activeInHierarchy ||
                pieces[i].GetComponent<PrefabScript> ().Planes[getPlaneIndex].GetComponent<Renderer> ().material.color !=
                pieces[4].GetComponent<PrefabScript> ().Planes[getPlaneIndex].GetComponent<Renderer> ().material.color) {
                return false;
            }
        }
        return true;
    }

}