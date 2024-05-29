using UnityEngine;
using System;

public class WebCamTexutreToTexture2D : MonoBehaviour {
	private static WebCamTexture wc;
	private Texture2D t;
	private static DateTime lastSendTime = DateTime.Now;

	// Use this for initialization
	void Start () {
		wc = new WebCamTexture ();
		GameObject.Find ("/Local").GetComponent<Renderer>().material.mainTexture = wc;
		wc.Play ();
		t = new Texture2D (wc.width, wc.height);
		GameObject.Find ("/Remote").GetComponent<Renderer>().material.mainTexture = t;
	}
	
	// Update is called once per frame
	void Update () {
		TimeSpan timeInterval = DateTime.Now - lastSendTime;
		if (timeInterval.TotalMilliseconds > 250 && wc.didUpdateThisFrame) {
			t.SetPixels(wc.GetPixels());
			t.Apply();
		}
	}
}
