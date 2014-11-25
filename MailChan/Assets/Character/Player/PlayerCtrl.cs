using UnityEngine;
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
		public GameObject bullet;			//PLayerBulletのプレハブ
		private int maxLifePoint = 28;		//最大ライフポイント
		private int lifePoint = 28;			//ライフポイント
		private float maxChargePoint = 28f;	//最大チャージポイント
		private float chargePoint = 0;		//チャージ量
		public float chargeTime = 2f;		//チャージにかかる秒数


		void Start(){
				lifePoint = maxLifePoint;
		}

		void Update(){
				print ("FPS:" + 1 / Time.deltaTime);
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
				float h = Input.GetAxis("Horizontal");
				if (h == 0)
						walkFlag = false;
				else
						walkFlag = true;
				
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
						//Instantiate (bullet, v3, transform.rotation);
						GameObject bulletCtrl = Instantiate (bullet, v3, transform.rotation) as GameObject;
						Bullet b = bulletCtrl.GetComponent<Bullet> ();
						b.ShotCtrl (facingRight);
						shotFlag = true;

						StartCoroutine ("ShotCheck");

						Damage(1);
				}
				ChargeCheck ();
				if (!Input.GetKey ("return")) {
						chargePoint = 0;
				}


				//アニメーション用フラグを設定
				GetComponent<Animator> ().SetBool ("walkFlag", walkFlag);
				GetComponent<Animator> ().SetBool ("shotFlag", shotFlag);

				//デバッグ---------------------------------------------------
				//MyDebug ();

		}

		//デバッグ用
		void MyDebug(){
				Debug.Log ("jumpFlag:" + jumpFlag);
				Debug.Log ("v:" + rigidbody2D.velocity);
		}

		//向き判定
		public int getFacingRight(){
				return (facingRight ? 1 : -1);
		}

		//ダメージ制御-------------------------------------------------------------
		public void Damage(int damage){
				lifePoint -= damage;
				if (lifePoint <= 0) {
						lifePoint = 0;
						//死亡判定
				}
				GameObject lifeBar = GameObject.Find ("LifeBar1");
				Vector2 vec = lifeBar.transform.localScale;
				vec.y = lifePoint;
				lifeBar.transform.localScale = vec;
		}

		//ショット制御--------------------------------------------------------------

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
