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
    private JSONArray valuesArray; // Declare valuesArray as a class member
    private bool dataLoaded = false;

    void Start()
    {
        StartCoroutine(ReadDataFromGoogleSheet("9890017607", GetRow));
    }


    private void GetRow(bool arg1,int arg2)
    {
        Debug.Log(arg2);
    }

    public IEnumerator ReadDataFromGoogleSheet(string numberToCheck, Action<bool,int> callback)
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

            // Access the "values" array and store it as a class member
            valuesArray = jsonData["values"].AsArray;

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

                    dataLoaded = true; // Data is loaded and ready
                    callback(NumberExistsInList(numberToCheck),GetPersonsCount(numberToCheck));
                    Debug.Log(IsColumnEmptyForMobileNumber("9890017607", "Name of 3rd person"));
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

    public bool NumberExistsInList(string numberToCheck)
    {
        return mobileNumbers.Contains(numberToCheck);
    }

    public bool IsColumnEmptyForMobileNumber(string mobileNumber, string columnName)
    {
        // Find the index of the "Number" column
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

        if (numberColumnIndex == -1)
        {
            Debug.LogError("Number column not found in the header.");
            return false;
        }

        // Find the row that matches the provided mobile number
        int rowIndex = -1;
        for (int i = 1; i < valuesArray.Count; i++)
        {
            if (valuesArray[i][numberColumnIndex] == mobileNumber)
            {
                rowIndex = i;
                break;
            }
        }

        if (rowIndex == -1)
        {
            Debug.LogError("Mobile number not found in the Google Sheet.");
            return false;
        }

        // Find the index of the specified column by name
        int columnIndex = -1;
        for (int i = 0; i < headerRow.Count; i++)
        {
            if (headerRow[i] == columnName)
            {
                columnIndex = i;
                break;
            }
        }

        if (columnIndex == -1)
        {
            Debug.LogError("Column not found in the header.");
            return false;
        }

        string cellValue = valuesArray[rowIndex][columnIndex];
        return string.IsNullOrEmpty(cellValue);
    }




    private int GetPersonsCount(string mobileNumber)
    {
        if (!IsColumnEmptyForMobileNumber(mobileNumber, "Name of 8th person"))
        {
            return 8;
        }
        else if (!IsColumnEmptyForMobileNumber(mobileNumber, "Name of 7th person"))
        {
            return 7;
        }
        else if (!IsColumnEmptyForMobileNumber(mobileNumber, "Name of 6th person"))
        {
            return 6;
        }
        else if (!IsColumnEmptyForMobileNumber(mobileNumber, "Name of 5th person"))
        {
            return 5;
        }
        else if (!IsColumnEmptyForMobileNumber(mobileNumber, "Name of 4th person"))
        {
            return 4;
        }
        else if (!IsColumnEmptyForMobileNumber(mobileNumber, "Name of 3rd person"))
        {
            return 3;
        }
        else if (!IsColumnEmptyForMobileNumber(mobileNumber, "Name of 2nd person"))
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

}
