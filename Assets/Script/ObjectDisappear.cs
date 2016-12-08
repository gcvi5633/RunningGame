using UnityEngine;
using System.Collections;

public class ObjectDisappear : MonoBehaviour {

    public GameObject trigger;
    Transform target;

    public float disappearDistance = 50f;

    public void OnEnable() {
        if(trigger) {
            trigger.SetActive(true);
        }
    }

    void Start() {
        target = GameObject.Find("Player").transform;
    }

    void Update() {
        if(Mathf.Abs(transform.position.x - target.position.x) > disappearDistance) {
            gameObject.SetActive(false);
            //Destroy(gameObject,2.5f);
        }
    }
}
