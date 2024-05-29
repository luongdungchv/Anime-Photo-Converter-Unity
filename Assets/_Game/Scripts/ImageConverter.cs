using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ImageConverter : MonoBehaviour
{
    [SerializeField] private Texture2D tex, resultTex;
    [SerializeField] private string hostName;
    [SerializeField] private string interrogatorAPI;
    [SerializeField] private string convertAPI;
    [SerializeField] private string predefinedPrompt;
    [SerializeField] private string negativePrompt;

    [SerializeField] private ConvertPostBody defaultConvertBody;
    [SerializeField] private InterrogatorPostBody defaultInterrogatorBody;

    public string HostName => this.hostName;
    private UnityAction<Texture2D, string> onComplete;
    private void Start() {
        var data = tex.EncodeToPNG();
        // tex.Reinitialize((int)(512f * (float)tex.width / (float)tex.height), 512);
        // tex.Apply();
        StartCoroutine(Upload(data));
    }

    public void SetTargetTexture(WebCamTexture tex){
        var texture = new Texture2D(tex.width, tex.height);
        texture.SetPixels(tex.GetPixels());
        texture.Apply(); 
        this.tex = texture;
    }
    public void SetTargetTexture(Texture2D tex){
        this.tex = tex;
    }
    public void Convert(UnityAction<Texture2D, string> onComplete){
        var data = tex.EncodeToPNG();
        this.onComplete = onComplete;
        StartCoroutine(Upload(data));
    }

    IEnumerator Upload(byte[] data)
    {
        // Create a UnityWebRequest instance
        var formData = new WWWForm();
        formData.AddBinaryData("file", data, "image.png", "image/png");
        UnityWebRequest request = UnityWebRequest.Post($"{hostName}/convert", formData);

        DownloadHandler imgIdHandler = new DownloadHandlerBuffer();
        request.downloadHandler = imgIdHandler;

        // Send the request
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Image upload successful!");

            // Texture2D downloadedTexture = imgIdHandler.;
            // this.resultTex = downloadedTexture;
            // onComplete?.Invoke(downloadedTexture);
            // onComplete = null;
            var textResult = imgIdHandler.text;
            Debug.Log(textResult);
            StartCoroutine(Retrieve(textResult));
        }
        else
        {
            Debug.LogError("Image upload failed: " + request.error);
        }
    }

    IEnumerator Retrieve(string text){
        UnityWebRequest request = UnityWebRequest.Get($"{hostName}/image?id=" + text);
        var downloadHandler = new DownloadHandlerTexture();
        request.downloadHandler = downloadHandler;
        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success){
            Debug.Log("Retrieve Success");
            var texture = downloadHandler.texture;
            this.resultTex = texture;
            onComplete?.Invoke(texture, text);
            onComplete = null;
        }
    }
}

[System.Serializable]
public class ConvertPostBody{
    public string prompt;
    public string negative_prompt;
    public string[] init_images;
    public string refiner_checkpoint;
    public float denoising_strength;
    public string sampler_name;
    public string scheduler;
    public string steps;
}
[System.Serializable]
public class InterrogatorPostBody{
    public string image;
    public string clip_model_name;
    public string mode;
}
