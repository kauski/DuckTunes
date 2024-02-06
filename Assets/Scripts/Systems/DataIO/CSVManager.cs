using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace DuckTunes.Systems.IO
{
    public static class CSVManager
    {
        static string _directoryName = "Resources/TargetDataCSV";
        static string _fileName = "targetdata.csv";
        static string _separator = ";";
        static string[] _defaultHeaders = new string[3]
        {
        "id",
        "points",
        "anchors"
        };

        #region public_functions
        public static void AppendToFile(string[] strings)
        {
            VerifyDirectory();
            VerifyFile();

            using (StreamWriter sw = File.AppendText(GetFilePath()))
            {
                string finalString = "";
                for (int i = 0; i < strings.Length; i++)
                {
                    if (finalString != "")
                    {
                        finalString += _separator;
                    }
                    finalString += strings[i];
                }

                sw.WriteLine(finalString);
                Debug.Log("Text appended to: " + _fileName);
            }
        }

        public static void CreateFile(string[] headers)
        {
            VerifyDirectory();
            _defaultHeaders = headers;

            using (StreamWriter sw = File.CreateText(GetFilePath()))
            {
                string finalString = "";
                for (int i = 0; i < _defaultHeaders.Length; i++)
                {
                    if (finalString != "")
                    {
                        finalString += _separator;
                    }
                    finalString += _defaultHeaders[i];
                }
                sw.WriteLine(finalString);
            }
        }

        /// <summary>
        /// Load a text file from the resources folder
        /// </summary>
        /// <param name="fileName">path of the file without file extension</param>
        public static string[] ReadFile(string fileName, bool hasHeader = true)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(fileName);
            string[] text = textAsset.text.Split('\n');
            List<string> strings = new List<string>();
            
            for (int i = hasHeader? 1 : 0; i < text.Length - 1; i++)
            {
                strings.Add(text[i]);
            }

            return strings.ToArray();
        }

        public static void SetFileName(string fileName)
        {
            _fileName = fileName;
        }

        public static void SetDirectory(string dirName)
        {
            _directoryName = dirName;
        }
        
        #endregion


        #region operations
        static void VerifyDirectory()
        {
            string dir = GetDirectoryPath();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Debug.Log("Creating new directory: " + dir);
            }
        }

        static void VerifyFile()
        {
            string file = GetFilePath();
            if (!File.Exists(file))
            {
                CreateFile(_defaultHeaders);
                Debug.Log("Creating a new file: " + file);
            }
        }
        #endregion


        #region Queries
        static string GetDirectoryPath()
        {
            return Application.dataPath + "/" + _directoryName + "/";
        }

        static string GetFilePath()
        {
            return GetDirectoryPath() + _fileName;
        }
        #endregion
    }
}