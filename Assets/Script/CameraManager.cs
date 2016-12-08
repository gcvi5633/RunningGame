using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
    [SerializeField]
    Transform target;
    float offsetX;
    Transform myTransform;
	void Start () {
        //target = GameObject.Find("Player").transform;
        myTransform = transform;
        offsetX = myTransform.position.x - target.position.x;
    }

	void Update() {
        myTransform.position = new Vector3( target.position.x + offsetX ,myTransform.position.y,myTransform.position.z);
	}
}
