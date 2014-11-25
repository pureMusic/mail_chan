using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	public GameObject player;
	private float cameraPosZ = -10f;
	public float screenNumX = 3;
	private float screenSizeX = 640;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
				//カメラ移動（追従）
				transform.position = new Vector3 (player.transform.position.x, 0, cameraPosZ);

				//見切れ防止
				if (transform.position.x < 0) {
						transform.position = new Vector3 (0, 0, cameraPosZ);
				}
				float pos = (screenNumX - 1) * screenSizeX ;
				if (transform.position.x >= pos) {
						transform.position = new Vector3 (pos, 0, cameraPosZ);
				}
	}
}
