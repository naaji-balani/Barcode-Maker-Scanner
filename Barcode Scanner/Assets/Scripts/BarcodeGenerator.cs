using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;

public class BarcodeGenerator : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageBackground; // Reference to the UI RawImage where the barcode will be displayed
    [SerializeField]
    private string _textToEncode = "Hello World"; // The text you want to encode in the barcode (EAN-13 format)

    void Start()
    {
        GenerateBarcode();
    }

    private void GenerateBarcode()
    {
        // Create a new barcode writer
        BarcodeWriter writer = new BarcodeWriter();
        writer.Format = BarcodeFormat.EAN_13; // You can change this format as needed

        // Set encoding options (if necessary)
        EncodingOptions encodingOptions = new EncodingOptions
        {
            Width = 256, // Adjust the width of the barcode
            Height = 128 // Adjust the height of the barcode
        };
        writer.Options = encodingOptions;

        // Encode the text into a barcode
        Color32[] pixels = writer.Write(_textToEncode);
        int width = encodingOptions.Width;
        int height = encodingOptions.Height;

        // Create a new Texture2D and set its pixels
        Texture2D barcodeTexture = new Texture2D(width, height);
        barcodeTexture.SetPixels32(pixels);
        barcodeTexture.Apply();

        // Apply the barcode texture to the UI RawImage
        _rawImageBackground.texture = barcodeTexture;
    }
}
