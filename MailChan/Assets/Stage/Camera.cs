using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
		public GameObject player;
		private float cameraPosZ = -10f;
		public float screenNumX = 3;
		public float screenNumY = 3;
		private float screenSizeX = 640f;
		private float screenSizeY = 480f;
		public bool scrollXFlag = true;
		public bool scrollYFlag = false;

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
				int xFlag = (scrollXFlag ? 1 : 0);
				int yFlag = (scrollYFlag ? 1 : 0);

				//カメラ移動（追従）
				transform.position = new Vector3 (xFlag * player.transform.position.x, yFlag * player.transform.position.y, cameraPosZ);

				//見切れ防止
				float posX = (screenNumX - 1) * screenSizeX;
				float posY = (screenNumY - 1) * screenSizeY;

				if (transform.position.x < 0) {
						posX = 0;
				} else if (transform.position.x < posX) {
						posX = player.transform.position.x;
				}

				if (transform.position.y < 0) {
						posY = 0;
				} else if (transform.position.y < posY) {
						posY = player.transform.position.y;
				}
				transform.position = new Vector3 (xFlag * posX, yFlag * posY, cameraPosZ);
		}
}
