using UnityEngine;

namespace DuckTunes.TargetTool.Data
{
    [System.Serializable]
    public class Circle
    {
        private Vector2 _position;
        public Vector2 Position => _position;


        private bool _isStartingCircle;
        public bool IsStartingCircle => _isStartingCircle;
        

        public Circle(Vector2 pos, bool isStart)
        {
            _position = pos;
            _isStartingCircle = isStart;
        }

        public void MoveCircle(Vector2 pos)
        {
            _position = pos;
        }
    }
}


