using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private WebCamCapturer capturer;
    [SerializeField] private ImageConverter converter;
    [SerializeField] private ResultPanel resultPanel;
    public void Capture(){
        //resultPanel.SetImage(capturer.Texture, capturer.TargetTransform, 0.7f);
        //capturer.StopCamera();
        resultPanel.Activate(true);
        resultPanel.SetLoading(true);    

        var texture = new Texture2D(capturer.Texture.width, capturer.Texture.height);
        texture.SetPixels32(capturer.Texture.GetPixels32());
        texture.Apply();
        var tex = capturer.GetShrinkedTexture();
        resultPanel.SetImage(tex, capturer.TargetTransform, 0.7f);
        capturer.StopCamera();

        converter.SetTargetTexture(tex);
        converter.Convert((tex, hash) => {
            resultPanel.GenerateQRCodeLink(hash, converter.HostName);
            resultPanel.SetLoading(false);
            capturer.RestartCamera();
            this.resultPanel.SetImage(tex);
            Debug.Log((tex.width, tex.height));
        });
    }
}
