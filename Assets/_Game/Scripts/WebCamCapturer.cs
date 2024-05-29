using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WebCamCapturer : MonoBehaviour
{
    [SerializeField] private WebCamTexture webCamTexture;
    [SerializeField] private RawImage image;
    [SerializeField] private RectTransform refCanvas;

    public WebCamTexture Texture => this.webCamTexture;
    public RectTransform TargetTransform => this.image.GetComponent<RectTransform>();

    private void Start()
    {
        
    }

    private void Update()
    {
        if (webCamTexture == null)
        {
            var devices = WebCamTexture.devices;
            var chosenDevice = devices[0];
            foreach(var device in devices){
                if(device.isFrontFacing) chosenDevice = device;
            }
            webCamTexture = new WebCamTexture(chosenDevice.name, (int)refCanvas.sizeDelta.x, (int)refCanvas.sizeDelta.x);
            webCamTexture.Play();
        }
        var rotation = webCamTexture.videoRotationAngle;
        image.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -rotation);
        var absRot = Mathf.Abs(rotation);
        if(absRot == 90 || absRot == 270){
            image.GetComponent<RectTransform>().sizeDelta = new Vector2(refCanvas.sizeDelta.x, refCanvas.sizeDelta.x);
        }
        else image.GetComponent<RectTransform>().sizeDelta = new Vector2(refCanvas.sizeDelta.x, refCanvas.sizeDelta.x);
        
        image.texture = webCamTexture;
    }

    public void StopCamera(){
        this.webCamTexture.Pause();
    }
    public void RestartCamera(){
        this.webCamTexture.Play();
    }

    public Texture2D GetShrinkedTexture(){
        var width = this.webCamTexture.width;
        var height = this.webCamTexture.height;
        var renderTex = new RenderTexture(width * 512 / height, 512, 24);
        var oldActiveRT = RenderTexture.active;
        RenderTexture.active = renderTex;
        Graphics.Blit(this.webCamTexture, renderTex);
        var result = new Texture2D(renderTex.width, renderTex.height);
        result.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        result.Apply();
        RenderTexture.active = oldActiveRT;
        return result;
    }

}
