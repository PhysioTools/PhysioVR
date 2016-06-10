using UnityEngine;
using System;

public class DataManager : MonoBehaviour {


    public static void ParseData(string message)
    {
        string[] separators = { ",", ";" };

        string[] words = message.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < words.Length; i++)
        {
            if (words[i].StartsWith("S_"))
            {
                //instantiates/updates sensor variable
            }
            else if (words[i].StartsWith("UB_"))
            {
                //updates updatable boolean variable
            }
            else if (words[i].StartsWith("UR_"))
            {
                //updates range variable
            }
        }
    }

    public static void CreateStringMessage()
    {

    }
}
