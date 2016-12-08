using UnityEngine;
using System.Collections;

public class GetSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var sizeX = GetComponent<BoxCollider2D>().size.x * transform.localScale.x;
        Debug.Log(sizeX);
	}

	// Update is called once per frame
	void Update () {

	}
}
