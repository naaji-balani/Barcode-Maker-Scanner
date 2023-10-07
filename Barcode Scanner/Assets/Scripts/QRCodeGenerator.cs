using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageBackground; // Reference to the UI RawImage where the QR code will be displayed
    [SerializeField]
    private string _textToEncode = "Hello, World!"; // The text you want to encode in the QR code

    void Start()
    {
        GenerateQRCode();
    }

    private void GenerateQRCode()
    {
        // Create a new QR code writer
        BarcodeWriter writer = new BarcodeWriter();
        writer.Format = BarcodeFormat.QR_CODE;

        // Set encoding options for QR code size
        QrCodeEncodingOptions options = new QrCodeEncodingOptions
        {
            Width = 256,
            Height = 256
        };
        writer.Options = options;

        // Encode the text into a QR code
        Color32[] pixels = writer.Write(_textToEncode);
        int width = options.Width;
        int height = options.Height;

        // Create a new Texture2D and set its pixels
        Texture2D qrCodeTexture = new Texture2D(width, height);
        qrCodeTexture.SetPixels32(pixels);
        qrCodeTexture.Apply();

        // Apply the QR code texture to the UI RawImage
        _rawImageBackground.texture = qrCodeTexture;
    }
}
