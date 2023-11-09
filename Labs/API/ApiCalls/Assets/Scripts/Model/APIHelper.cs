using UnityEngine;
using System.Net;
using System.IO;
using System;

public static class APIHelper
{
    /// <summary>
    /// Gets a new joke from the API
    /// </summary>
    /// <returns>A new Joke from the API</returns>
    public static Joke GetNewJoke()
    {
        try
        {
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create("https://api.chucknorris.io/jokes/random");

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using StreamReader reader = new(response.GetResponseStream());
            string json = reader.ReadToEnd();

            return JsonUtility.FromJson<Joke>(json);
        } 
        
        catch (WebException e)
        {
            Debug.LogError("WebExcepton has occured: " + e.Message);
            return null;
        }

        catch (Exception e)
        {
            Debug.LogError("Exception has occured: " + e.Message);
            return null;
        }
    }
}