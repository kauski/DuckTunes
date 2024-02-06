using System.Collections;
using System.Collections.Generic;
using System.IO;
using Melanchall.DryWetMidi.Core;
using UnityEditor;
using UnityEngine;
using Melanchall.DryWetMidi.Tools;

namespace DuckTunes.Utility
{
    public class MidiToCSV : MonoBehaviour
    {
        #if UNITY_EDITOR

        [SerializeField] private string _midiFilePathInResourcesMidiFolderWithFileExtension;
        [SerializeField] private string _fileNameWithoutExtension;
        private string _savePath = "Assets/StreamingAssets/";

        public void ConvertToCsv()
        {
            CsvConverter converter = new CsvConverter();
            MidiFile midi = MidiFile.Read("Assets/Resources/MidiFiles/" + _midiFilePathInResourcesMidiFolderWithFileExtension);
            FileStream stream = new FileStream(_savePath + "/" + _fileNameWithoutExtension + ".csv", FileMode.Create, FileAccess.Write);
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            converter.ConvertMidiFileToCsv(midi, stream);
            stream.Close();
        }

#endif
    }   
}

