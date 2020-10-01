using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
    private GameObject earthSpawner;
    public int cubeType = 0;
    public float blockHPMax = 60f;

    public GameObject player;

	// Use this for initialization
	void Start () {
        earthSpawner = GameObject.Find("EarthSpawner");
	}

    public class CubePosition
    {
        public int type; // 0 = 초원블럭
        public Vector3 position;
        public CubePosition(int a, Vector3 b)
        {
            type = a;
            position = b;
        }
        public CubePosition()
        {
        }
    }


    // Update is called once per frame
    void LateUpdate () {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            earthSpawner.GetComponent<EarthSpawner>().mapInfo.Add(new CubePosition(cubeType,transform.position));
        }
	}
}
