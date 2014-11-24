using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour {
		public float bulletSpeed = 128f;

		// Use this for initialization
		void Start () {
				rigidbody2D.velocity = new Vector2 (bulletSpeed, 0);
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		//画面外処理
		void OnBecameInvisible(){
				//弾の削除
				Destroy (this.gameObject);
		}

		//衝突処理
		void OnCollisionEnter2D(Collision2D col){
				Destroy (this.gameObject);
		
		}
}
