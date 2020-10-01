using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    private Animator animator;
    private Transform head;
    private float aa = 10.616990f;


	// Use this for initialization
	void Start () {
        animator = transform.GetComponent<Animator>();
        head = animator.GetBoneTransform(HumanBodyBones.Neck);
    }
	
	// Update is called once per frame
	void LateUpdate () {

        head.localRotation = Quaternion.Euler(new Vector3(-180, -1.525879f,aa ));
        aa += 1;
	}
}
