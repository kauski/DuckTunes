using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Melanchall.DryWetMidi.Core;

namespace DuckTunes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "New Track")]
    public class TrackInfo : ScriptableObject
    {
        [Header("Insert sound file to Track field and fill the BPM, offset and Midi fields")]
        public AudioClip Track;
        public float BPM;
        public float FirstBeatOffsetInSeconds;
        public string _midiCSVNameWithFileEnding;
    }
}