﻿using UnityEngine;
using System.Collections;
using System;

public class PlayerCtrl : MonoBehaviour {
		public float speed = 196f;			//横移動速度
		public float jumpForce = 320f;		//ジャンプ力
		public int bulletMaxNum = 3; 		//画面内の弾の最大数
		private bool facingRight = true;	//向いてる方向
		bool jumpFlag = false;				//ジャンプフラグ
		bool walkFlag = false;				//横移動フラグ
		bool shotFlag = false;				//ショットフラグ
		public GameObject nBullet;			//通常ショットのプレハブ
		public GameObject cBullet;			//チャージショットのプレハブ
		private int maxLifePoint = 28;		//最大ライフポイント
		private int lifePoint = 28;			//ライフポイント
		private float maxChargePoint = 28f;	//最大チャージポイント
		private float chargePoint = 0;		//チャージ量
		public float chargeTime = 2f;		//チャージにかかる秒数
		private bool hitFlag = false;		//ダメージ判定フラグ
		public GameObject hitRenderer;		//ダメージスプライト
		private Vector3 pos;				//座標固定用

		void Start(){
				lifePoint = maxLifePoint;
				this.transform.rigidbody2D.gravityScale = 64;
				hitRenderer = Instantiate (hitRenderer, this.transform.position, this.transform.rotation) as GameObject;
				hitRenderer.transform.renderer.enabled = false;
		}

		void Update(){
				if (lifePoint > 0) {
						Vector2 v = rigidbody2D.velocity;

						//ジャンプ制御------------------------------------

						//着地
						if (rigidbody2D.velocity.y == 0 && jumpFlag) {
								jumpFlag = false;

						}

						//落下
						if (rigidbody2D.velocity.y < -0.1f) {	//誤差修正のため0ではなく-0.1とする
								jumpFlag = true;
						}

						//ジャンプボタン押下
						if (Input.GetKeyDown ("space") && !jumpFlag) {
								jumpFlag = true;
								v.y = jumpForce;
						}
		
						//横移動-------------------------------------------------
						float h = Input.GetAxis ("Horizontal");
						if (h == 0)
								walkFlag = false;
						else
								walkFlag = true;
					
						v.x = h * speed;
						rigidbody2D.velocity = v;

						//右を向いていて、左の入力があったとき、もしくは左を向いていて、右の入力があったとき
						if ((h > 0 && !facingRight) || (h < 0 && facingRight)) {
								//右を向いているかどうかを、入力方向をみて決める
								facingRight = (h > 0);
								//localScale.xを、右を向いているかどうかで更新する
								transform.localScale = new Vector3 ((facingRight ? 1 : -1), 1, 1);
						}

						//ショット制御---------------------------------------------------------------
						if (Input.GetKeyDown ("return")) {
								ShotCtrl ();
						}
						ChargeCheck ();
						if (Input.GetKeyUp ("return") && chargePoint > maxChargePoint / 3) {
								ShotCtrl ();
						}
						if (!Input.GetKey ("return")) {
								chargePoint = 0;
						}


						//アニメーション用フラグを設定
						GetComponent<Animator> ().SetBool ("walkFlag", walkFlag);
						GetComponent<Animator> ().SetBool ("shotFlag", shotFlag);
				}

				//デバッグ---------------------------------------------------
				//MyDebug ();

		}

		//デバッグ用
		void MyDebug(){
				print ("FPS:" + 1 / Time.deltaTime);
				Debug.Log ("jumpFlag:" + jumpFlag);
				Debug.Log ("v:" + rigidbody2D.velocity);
		}

		//向き判定
		public int getFacingRight(){
				return (facingRight ? 1 : -1);
		}

		//ダメージ制御-------------------------------------------------------------
		public void Damage(int damage){
				if (!hitFlag) {
						hitFlag = true;
						lifePoint -= damage;
						if (lifePoint <= 0) {
								lifePoint = 0;
								//キャラクター座標を固定
								this.transform.rigidbody2D.velocity = new Vector2 (0, 0);
								this.transform.rigidbody2D.gravityScale = 0;
								//スプライト非表示
								this.transform.renderer.enabled = false;
						} else {
								//無敵時間発生
								StartCoroutine ("InvincibleTime");
						}
						GameObject lifeBar = GameObject.Find ("LifeBar1");
						Vector2 vec = lifeBar.transform.localScale;
						vec.y = lifePoint;
						lifeBar.transform.localScale = vec;
				}
		}

		//無敵時間発生
		IEnumerator InvincibleTime(){
				int count = 0;
				hitRenderer.transform.position = this.transform.position; 
				//ダメージエフェクト表示
				for (count = 0; count < 10; count++) {
						hitRenderer.renderer.enabled = (count % 2 == 0 ? true : false);
						this.transform.renderer.enabled = (count % 2 == 0 ? false : true);
						yield return new WaitForSeconds (1 / 60f);
				}
				//ダメージエフェクトを終了し、無敵継続
				for (count = 0; count < 50; count++) {
						this.transform.renderer.enabled = (count % 2 == 0 ? false : true);
						yield return new WaitForSeconds (1 / 60f);
				}

				//ダメージを受けるようにする
				hitFlag = false;
		}

		//ショット制御--------------------------------------------------------------

		//ショットコントロール
		void ShotCtrl(){
				Vector3 v3 = transform.position;
				v3.x += 38 * getFacingRight ();
				GameObject bulletCtrl;

				if (chargePoint != maxChargePoint) {
						bulletCtrl = Instantiate (nBullet, v3, transform.rotation) as GameObject;
				}else {
						bulletCtrl = Instantiate (cBullet, v3, transform.rotation) as GameObject;
				}
				Bullet b = bulletCtrl.GetComponent<Bullet> ();
				b.BulletCtrl (0, facingRight);
				shotFlag = true;

				StartCoroutine ("ShotCheck");
		}

		//ショットモーション制御
		IEnumerator ShotCheck(){
				yield return new WaitForSeconds (0.4f);
				shotFlag = false;
		}

		//チャージ時間管理
		bool ChargeCheck(){
				chargePoint += maxChargePoint / (chargeTime * 50);
				if (chargePoint > maxChargePoint) {
						chargePoint = maxChargePoint;
				}

				GameObject chargeBar = GameObject.Find ("ChargeBar1");
				Vector2 vec = chargeBar.transform.localScale;
				vec.y = chargePoint;
				chargeBar.transform.localScale = vec;

				return (chargePoint == maxChargePoint ? true : false);
		}


		//接触制御--------------------------------------------------------------------

		//接触時の処理
		void OnTriggerEnter2D(Collider2D col){
				//移動床に乗る
				if (transform.parent == null && col.gameObject.tag == "MoveBlock") {
						transform.parent = col.gameObject.transform;
				}

		}

		//離れた時の処理
		void OnTriggerExit2D(Collider2D col){
				//移動床から離れる
				if(transform.parent != null && col.gameObject.tag == "MoveBlock"){
						transform.parent = null;
				}
		}

		//接触中の処理
		void OnTriggerStay2D(Collider2D col){

		}

		//ゲッタ・セッタ---------------------------------------------------------------
	
}
