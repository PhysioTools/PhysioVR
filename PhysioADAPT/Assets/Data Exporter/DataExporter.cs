using UnityEngine;
using System.IO;
using System;
using System.Globalization;
using System.Collections.Generic;
using Assets.Adapter;
using Assets.Manager;

public class DataExporter : MonoBehaviour {

    private List<string> _header = new List<string>();
    private List<string> _variables = new List<string>();

    private TextWriter _file;
    private string _filepath = string.Empty;
    private string _path;
    public static bool Islogging;
    public static bool StartLog;
    
    private bool _setFirstLine;

    
	void Update ()
    {
        if (StartLog && !Islogging)
        {
            LogInit();
        }

	    if (Islogging)
	    {
            SetValues();
            CSVWrite();
	    }
    }

    private void LogInit()
    {
        Islogging = true;

        _path = Application.persistentDataPath;
        //_path = Application.dataPath + "/PhysioVR_Log/";

        if (!Directory.Exists(_path))
        {
            System.IO.Directory.CreateDirectory(_path);
        }

        _filepath = _path + "/PhysioVR_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        _file = new StreamWriter(_filepath, true); 

        _header.Add("TimeStamp"); 

        SetHeaderList();

        //builds the string that will be the header of the csv file
        var fillHeader = _header[0];

        for (var i = 1; i < _header.Count; i++)
        {
            fillHeader = fillHeader + "," + _header[i];
        }

        //writes the first line of the file (header)
        _file.WriteLine(fillHeader);

        StartLog = false;
    }

    private void SetHeaderList()
    {
        if (DataManager.EnvironmentName == "Demo")
        {
            var uv = Demo.SetEnvironment().UpdatableVariables;
            var rv = Demo.SetEnvironment().ReadableVariables;
            var sv = PhysioAdapter.Sensors;
            
            for (var i = 0; i < uv.Count; i++)
            {
                _header.Add(uv[i].Name);
            }

            for (var i = 0; i < rv.Count; i++)
            {
                _header.Add(rv[i].Name);
            }

            for (var i = 0; i < sv.Count; i++)
            {
                _header.Add(sv[i].Name);
            }
        }
    }

    private void SetValues()
    {
        if (DataManager.EnvironmentName == "Demo")
        {
            var uv = Demo.SetEnvironment().UpdatableVariables;
            var rv = Demo.SetEnvironment().ReadableVariables;
            var sv = PhysioAdapter.Sensors;

            if (!_setFirstLine)
            {
                _variables.Add(DateTime.Now.ToString("HHmmss") + DateTime.Now.Millisecond);
                
                for (var i = 0; i < uv.Count; i++)
                {
                    _variables.Add(uv[i].Value);
                }
                
                
                for (var i = 0; i < rv.Count; i++)
                {
                    _variables.Add(rv[i].Value);
                }

                for (var i = 0; i < sv.Count; i++)
                {
                    _variables.Add(sv[i].Value);
                }

                _setFirstLine = true;
            }
            else
            {
                _variables[0] = DateTime.Now.ToString("HHmmss") + DateTime.Now.Millisecond;

                for (var i = 0; i < uv.Count; i++)
                {
                    _variables[i + 1] = uv[i].Value;
                }

                for (var i = 0; i < rv.Count; i++)
                {
                    _variables[uv.Count + 1 + i] = rv[i].Value;
                }

                for (var i = 0; i < sv.Count; i++)
                {
                    _variables.Add(sv[i].Value);
                }
            }
        }
    }

    private void CSVWrite()
    {
        var newLine = _variables[0];

        for (var i = 1; i < _variables.Count; i++)
        {
            newLine = newLine + "," + _variables[i];
        }
        _file.Write(newLine);
        _file.WriteLine("");
    }

    private void OnApplicationQuit()
    {
        Islogging = false;
        _file.Close();
    }
}
