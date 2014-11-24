using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour {
		//private int lifePoint = 28;
		private GameObject player;

	// Use this for initialization
	void Start () {
				player = GameObject.Find ("player");
	}
	
	// Update is called once per frame
	void Update () {
				//if (Input.GetKeyDown ("return")) {
						ThianaCtrl tmp = player.GetComponent<ThianaCtrl>();
						tmp.getLifePoint ();
						LifeCheck (tmp.getLifePoint ());
				//}
	}

		public void LifeCheck(int life){
				Vector2 vec = transform.localScale;
				vec.y = life;
				transform.localScale = vec;
		}
}
