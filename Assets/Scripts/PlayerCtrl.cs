﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
	/*
	Idle - 0
	Jump - 1
	Run - 2
	Falling -3
	Shooting -4
	Hurt - 5
    */

	public class PlayerCtrl : MonoBehaviour {
	
		public float horizontalSpeed = 10f;
        public float jumpSpeed = 600f;

		Rigidbody2D rb;
		SpriteRenderer sr;
		Animator anim;
		
        bool isJumping = false;

		// Use this for initialization
		void Start () {
			rb = GetComponent<Rigidbody2D>();
			sr = GetComponent<SpriteRenderer>();
			anim = GetComponent<Animator>();
		}
	
		// Update is called once per frame
		void Update () {
	
			float horizontalInput=Input.GetAxisRaw ("Horizontal"); // -1: esquerda / 1: direta //
			float horizontalPlayerSpeed=horizontalSpeed*horizontalInput;
			
			if (horizontalPlayerSpeed !=0) {
				MoveHorizontal(horizontalPlayerSpeed);
			}
			else {
				StopMovingHorizontal();
			}

			if (Input.GetButtonDown("Jump")){
				Jump();				
			}
		}
	
		void MoveHorizontal(float speed) {
			rb.velocity=new Vector2(speed, rb.velocity.y);

			if (speed < 0f){
				sr.flipX = true;
			}
			else if (speed > 0){
				sr.flipX = false;
			}
			
			if (!isJumping){
			    anim.SetInteger("State", 2);
			}
		}
	
		void StopMovingHorizontal () {
			rb.velocity=new Vector2(0f, rb.velocity.y);

			if (!isJumping){
		        anim.SetInteger("State", 0);
			}
		}

		void ShowFalling(){
			if (rb.velocity.y < 0){
				anim.SetInteger("State", 3);
			}
		}

        void Jump(){
			isJumping = true;
			rb.AddForce(new Vector2(0f, jumpSpeed));
			anim.SetInteger("State", 1);
		}

		void OnCollisionEnter2D(Collision2D other){
			if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
				isJumping = false;
			}
		}
	}