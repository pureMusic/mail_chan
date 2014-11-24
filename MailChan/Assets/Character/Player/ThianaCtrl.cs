using UnityEngine;
using System.Collections;
using System;

public class ThianaCtrl : MonoBehaviour {
		public float speed = 196f;
		public float jumpForce = 320f;
		bool jumpFlag = false;
		public int bulletMaxNum = 3; 
		private bool facingRight = true;
		bool walkFlag = false;
		bool shotFlag = false;
		public GameObject bullet;	//PLayerBulletのプレハブ
		int counter = 0;

		void Start(){

		}

		void Update(){

				Vector2 v = rigidbody2D.velocity;

				//ジャンプ制御------------------------------------

				//着地
				if (rigidbody2D.velocity.y == 0 && jumpFlag) {
						jumpFlag = false;
				}

				//落下
				if (rigidbody2D.velocity.y < 0) {
						jumpFlag = true;
				}

				//ジャンプボタン押下
				if (Input.GetKeyDown ("space") && !jumpFlag) {
						jumpFlag = true;
						v.y = jumpForce;
				}
	
				//横移動-------------------------------------------------
				float h = Input.GetAxis("Horizontal");
				if (h == 0)
						walkFlag = false;
				else
						walkFlag = true;

				//Debug.Log("walk" + ":" + walkFlag);
				
				v.x = h * speed;
				rigidbody2D.velocity = v;

				//右を向いていて、左の入力があったとき、もしくは左を向いていて、右の入力があったとき
				if((h > 0 && !facingRight) || (h < 0 && facingRight))
				{
						//右を向いているかどうかを、入力方向をみて決める
						facingRight = (h > 0);
						//localScale.xを、右を向いているかどうかで更新する
						transform.localScale = new Vector3((facingRight ? 1 : -1), 1, 1);
				}

				//ショット制御---------------------------------------------------------------
				if (Input.GetKeyDown ("return")) {
						Vector3 v3 = transform.position;
						v3.x += 38 * getFacingRight();
						Instantiate (bullet, v3, transform.rotation);
						shotFlag = true;
						counter = 0;
				}
	
				if (shotFlag) {
						counter++;
						if (counter > 30) {
								shotFlag = false;
						}
				}

				//アニメーション用フラグを設定
				GetComponent<Animator> ().SetBool ("walkFlag", walkFlag);
				GetComponent<Animator> ().SetBool ("shotFlag", shotFlag);
				Debug.Log(shotFlag);

		}
		
		
		void FixedUpdate ()
		{}


		public int getFacingRight(){
				return (facingRight ? 1 : -1);
		}

}
