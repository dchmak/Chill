/*
* Created by Daniel Mak
*/

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

    public GameObject player;
    [Range(0f, 1f)] public float smoothness;
    public Vector3 offset;
    [Range(0f, 100f)] public float minSize;
    [Range(0f, 100f)] public float maxSize;
    [Range(0f, 100f)] public float radius;
    
    private GameObject[] targets;
    private Vector3 velocity;
    private Camera cam;
    

    private void Start() {
        targets = GameObject.FindGameObjectsWithTag("Enemy");

        cam = GetComponent<Camera>();
    }

    private void Update() {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void LateUpdate() {

        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, smoothness);

        Bounds bounds = new Bounds(player.transform.position, Vector3.zero);
        bounds.Encapsulate(player.transform.position);

        foreach (GameObject target in targets) {
            if ((target.transform.position - player.transform.position).magnitude < radius) {
                //print("Included!");
                bounds.Encapsulate(target.transform.position);
            }
        }

        Vector3 boundSize = bounds.size;
        //print(boundSize);
        float orthographicSize;
        if (bounds.size.y >= bounds.size.x) {
            orthographicSize = Mathf.Clamp(boundSize.y, minSize, maxSize);
        } else {
            orthographicSize = Mathf.Clamp(boundSize.x, minSize, maxSize);
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, orthographicSize, smoothness);

        //cam.fieldOfView = Mathf.Max(bounds.size.x, bounds.size.y);
        //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Mathf.Lerp(maxZoom, minZoom, Mathf.Sqrt(Mathf.Pow(bounds.size.x, 2) + Mathf.Pow(bounds.size.y, 2)) / zoomLimit), Time.deltaTime);
    }

    /*
    private void LateUpdate() {
        float desireX = Mathf.Lerp(transform.position.x, target.position.x, smoothness);
        float desireY = Mathf.Lerp(transform.position.y, target.position.y, smoothness);
        float desireZ = transform.position.z;
        transform.position = new Vector3(desireX, desireY, desireZ);
    }
    */
}