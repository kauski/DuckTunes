using System;
using System.Collections;
using System.Collections.Generic;
using DuckTunes.Utility;
using UnityEditor;
using UnityEngine;

namespace DuckTunes.EditorScripts
{
    [CustomEditor(typeof(MidiToCSV))]
    public class MidiToCSVEditor : Editor
    {
        #if UNITY_EDITOR
        private MidiToCSV _midiToCsv;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SaveToCSV();
        }

        private void SaveToCSV()
        {
            if (GUILayout.Button("Save"))
            {
                _midiToCsv.ConvertToCsv();
            }
        }

        private void OnEnable()
        {
            _midiToCsv = (MidiToCSV)target;
        }
        #endif
    } 
}

