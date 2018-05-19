/*
* Created by Daniel Mak
*/

using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    [Range(0f, 1f)] public float smoothness;
	
	private void LateUpdate () {
        float desireX = Mathf.Lerp(transform.position.x, target.position.x, smoothness);
        float desireY = Mathf.Lerp(transform.position.y, target.position.y, smoothness);
        float desireZ = transform.position.z;
        transform.position = new Vector3(desireX, desireY, desireZ);
	}
}