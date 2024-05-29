using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Converter
{
    public static string TextToDataURI(string text, string mimeType)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        string base64String = Convert.ToBase64String(bytes);

        string dataUri = "data:" + mimeType + ";base64," + base64String;

        return dataUri;
    }
}
