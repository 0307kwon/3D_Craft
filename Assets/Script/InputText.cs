using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    InputField inputField;
    private bool writing = false;

    // Use this for initialization
    public bool GetWriting()
    {
        return writing;
    }

    public void WritingBool (bool b)
    {
        writing = b;
    }
}
