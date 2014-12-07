using UnityEngine;
using System.Collections;

public class ScrollMode : MonoBehaviour {
		public bool scrollXFlag = false;
		public bool scrollYFlag = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		void OnTriggerEnter2D(Collider2D col){
				if (col.gameObject.tag == "Player") {
						GameObject camera = GameObject.Find ("Main Camera");
						Camera cameraCtrl = camera.GetComponent<Camera> ();
						cameraCtrl.scrollXFlag = scrollXFlag;
						cameraCtrl.scrollYFlag = scrollYFlag;
				}
		}
}
