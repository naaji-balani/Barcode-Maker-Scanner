using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;
using System;

public class GoogleSheetReader : MonoBehaviour
{
    public string googleSheetAPIEndpoint = "YOUR_GOOGLE_SHEET_API_ENDPOINT";
    public string apiKey = "YOUR_API_KEY";
    private List<string> mobileNumbers = new List<string>();

    void Start()
    {
        //StartCoroutine(ReadDataFromGoogleSheet());
    }

    public IEnumerator ReadDataFromGoogleSheet(string numberToCheck,Action<bool> callback)
    {
        string url = googleSheetAPIEndpoint + "?key=" + apiKey;
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            string jsonResult = www.downloadHandler.text;
            Debug.Log("JSON Data: " + jsonResult);

            // Parse JSON manually
            JSONNode jsonData = JSON.Parse(jsonResult);

            // Access the "values" array
            JSONArray valuesArray = jsonData["values"].AsArray;

            if (valuesArray.Count > 0)
            {
                // Find the index of the "Number" column (assuming it's in the first row)
                int numberColumnIndex = -1;
                JSONArray headerRow = valuesArray[0].AsArray;
                for (int i = 0; i < headerRow.Count; i++)
                {
                    if (headerRow[i] == "Number")
                    {
                        numberColumnIndex = i;
                        break;
                    }
                }

                if (numberColumnIndex != -1)
                {
                    // Collect all mobile numbers and store in the list
                    for (int i = 1; i < valuesArray.Count; i++)
                    {
                        string mobileNumber = valuesArray[i][numberColumnIndex];
                        mobileNumbers.Add(mobileNumber);
                    }


                    callback(NumberExistsInList(numberToCheck));
                }
                else
                {
                    Debug.Log("Number column not found in the header.");
                }
            }
            else
            {
                Debug.Log("No data found in the values array.");
            }
        }
    }

    bool NumberExistsInList(string numberToCheck)
    {
        return mobileNumbers.Contains(numberToCheck);
    }
}
