﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class AnimationController : NetworkBehaviour {

    private Animator animator;
    private FirstPersonController fpc;
    private bool isRunning;
    private bool isJumping;

	// Use this for initializationooooo
	void Start () {
        animator = GetComponentInChildren<Animator>();
        fpc = GetComponent<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
        isRunning = fpc.m_isRunning;
        isJumping = fpc.m_Jumping;

        animator.SetBool("IsJumping", isJumping);
        animator.SetBool("IsRunning", isRunning);
	}
}
