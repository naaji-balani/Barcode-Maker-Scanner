using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GoogleSheetReader : MonoBehaviour
{
    public string googleSheetAPIEndpoint = "YOUR_GOOGLE_SHEET_API_ENDPOINT";
    public string apiKey = "YOUR_API_KEY";

    void Start()
    {
        StartCoroutine(ReadDataFromGoogleSheet());
    }

    IEnumerator ReadDataFromGoogleSheet()
    {
        string url = googleSheetAPIEndpoint + "?key=" + apiKey;
        Debug.Log(url);
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            string jsonResult = www.downloadHandler.text;

            // Parse the JSON data and process it as needed.
            // You'll need to extract the data you want from the JSON response.
            Debug.Log(jsonResult);
        }
    }
}
