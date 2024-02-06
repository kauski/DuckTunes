using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DuckTunes.ScriptableObjects;
using UnityEngine;
using UnityEngine.Networking;

namespace DuckTunes.Systems.Music
{
    public class BeatMapFileOperations 
    {
        protected readonly Conductor _conductor;
        protected TrackInfo _track;
        protected TextAsset _midiCSV; 
        protected bool DataReceived = false;
        protected SimpleNote[] _notes;
        
        protected const float MsToSec = 0.01f;
        
        #region Public functions
        
        public BeatMapFileOperations(TrackInfo track,  Conductor conductor)
        {
            _conductor = conductor;
            _track = track;
        }

        #endregion
        
        
        
       #region File operations
        protected void GetMidiFilePath()
        {
            string path1 = Path.Combine(Application.streamingAssetsPath, _track._midiCSVNameWithFileEnding);

            GetRequest(path1);
        }

        private async void GetRequest(string url)
        {
            using var webRequest = UnityWebRequest.Get(url);
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            HandleWebRequestResult(webRequest);
        }

        private void HandleWebRequestResult(UnityWebRequest webRequest)
        {
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                _midiCSV = new TextAsset(webRequest.downloadHandler.text);
                ParseCSV(_midiCSV);
                DataReceived = true;
            }
            else
            {
                Debug.Log("Get failed");
            }
        }

        private void ParseCSV(TextAsset midiFile)
        {
            var textList = GetRows();

            string[] text = textList.ToArray();
            List<SimpleNote> notes = new List<SimpleNote>();

            CreateNotesFromRows(text, notes);

            _notes = notes.ToArray();

            var times = GetNoteTimes();

            ValidateNotes();
            _conductor.SetNoteTimes(times);
        }

        private float[] GetNoteTimes()
        {
            float[] times = new float[_notes.Length];
            for (int i = 0; i < _notes.Length; i++)
            {
                times[i] = _notes[i].StartTime * MsToSec;
            }

            return times;
        }

        private static void CreateNotesFromRows(string[] text, List<SimpleNote> notes)
        {
            for (int i = 0; i < text.Length - 1; i++)
            {
                string[] textValues = text[i].Split(',');

                if (textValues[2].EndsWith("On")) //start time
                {
                    notes.Add(new SimpleNote(int.Parse(textValues[1]), int.Parse(textValues[4])));
                }
            }
        }

        private List<string> GetRows()
        {
            string[] rows = _midiCSV.text.Split('\n');
            List<string> textList = new List<string>();

            for (int i = 5; i < rows.Length; i++) //Skip header
            {
                textList.Add(rows[i]);
            }

            return textList;
        }

        private void ValidateNotes()
        {
            var n1 = 60;
            var n2 = 62;
            for (int i = 0; i < _notes.Length; i++)
            {
                //Debug.LogWarning(notes[i].NoteNumber);
                if (_notes[i].NoteNumber != n1 && _notes[i].NoteNumber != n2) 
                {
                    Debug.LogWarning("Midifile has notes that are not being read: \n Note: " + _notes[i] + "\n" + "Only use C and D notes (q and w in lmms)");
                }
            }
        }

        #endregion
    }  
}

