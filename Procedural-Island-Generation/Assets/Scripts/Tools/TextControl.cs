﻿using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour
{
    public Text headerText;

    public void Initialize(string header)
    {
        name = header;
        headerText.text = header;
    }
}
