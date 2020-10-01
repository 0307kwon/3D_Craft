using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private Transform scrollbar;

    // Use this for initialization
    void Start () {
        scrollbar = transform.Find("ScrollArea").Find("TextContainer").Find("Scrollbar");
    }

    // Update is called once per frame
    void Update()
    {
        scrollbar.GetComponent<Scrollbar>().value = 0;
    }
}
