using UnityEngine;
using ZXing;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QRCodeScanner : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageBackground;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;
    [SerializeField]
    private TextMeshProUGUI _textOut;
    [SerializeField]
    private RectTransform _scanZone;

    [SerializeField] GoogleSheetReader _sheets;

    private bool _isCamAvaible;
    private WebCamTexture _cameraTexture;
    void Start()
    {
        SetUpCamera();
        StartCoroutine(KeepScanning());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            //_textOut.text = "No Camera";
            _isCamAvaible = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == false)
            {
                //_textOut.text = "Front Camera";
                _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
                break;
            }
        }
        _cameraTexture.Play();
        _rawImageBackground.texture = _cameraTexture;
        _isCamAvaible = true;
    }

    private void UpdateCameraRender()
    {
        if (_isCamAvaible == false)
        {
            return;
        }

        // Correct the orientation of the camera feed.
        int orientation = _cameraTexture.videoRotationAngle;
        orientation = (360 - orientation) % 360; // Correct for orientation issues
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, 180, orientation);

        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;
    }

    public void OnClickScan()
    {
        Scan();
    }
    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                //_sheets.ReadDataFromGoogleSheet(result.Text, GetResult);
                StartCoroutine(_sheets.ReadDataFromGoogleSheet(result.Text, GetResult));
            }
            else
            {
                //_textOut.text = "";
            }
        }
        catch
        {
            //_textOut.text = "FAILED IN TRY";
        }
    }

    private void GetResult(bool isDataExists)
    {
        _textOut.text = isDataExists ? "User Valid" : "User Invalid";
        _textOut.color = isDataExists ? Color.green : Color.red;

        Invoke("RemoveText", 4);
    }

    private void RemoveText()
    {
        _textOut.text = "";
    }

    IEnumerator KeepScanning()
    {
        while (true)
        {
            Scan();
            yield return new WaitForSeconds(1);
        }
    }
}