using System.Collections;
using System.Collections.Generic;
using Unity.IntegerTime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private RawImage resultImage;
    [SerializeField] private GameObject loading;
    [SerializeField] private RawImage qrCodeImage;   
    [SerializeField] private Button captureAgainBtn;

    private void Awake(){
        captureAgainBtn.onClick.AddListener(() => this.Activate(false));
    } 

    public void Activate(bool state){
        this.gameObject.SetActive(state);
    }
    public void SetImage(Texture tex, RectTransform targetTransform, float ratio = 1){
        this.resultImage.texture = tex;
        var resultImgTransform = resultImage.GetComponent<RectTransform>();
        resultImgTransform.localScale = targetTransform.localScale;
        resultImgTransform.rotation = targetTransform.rotation;

        resultImgTransform.sizeDelta = new Vector2(targetTransform.sizeDelta.x * ratio, targetTransform.sizeDelta.y * ratio);

        //this.GenerateQRCodeLink("a", "htt");

    }
    public void SetImage(Texture tex){
        this.resultImage.texture = tex;
    }
    public void SetLoading(bool state){
        this.loading.SetActive(state);
        this.ShowQR(!state);
        this.captureAgainBtn.gameObject.SetActive(!state);
    }
    public void ShowQR(bool state){
        this.qrCodeImage.gameObject.SetActive(state);
    }
    public void GenerateQRCodeLink(string hash, string hostName){
        var link = $"{hostName}/image?id={hash}";

        var width = 256;
        var height = 256;

        BarcodeWriter barcodeWriter = new BarcodeWriter(){
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        var color = barcodeWriter.Write(link);
        var tex = new Texture2D(width, height);
        tex.SetPixels32(color);
        tex.Apply();
        this.qrCodeImage.texture =tex;
    }
    
}

