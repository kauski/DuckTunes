using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckTunes.Utility
{
    public static class Utilities
    {
        public static Vector3 GetWorldPositionFromScreenPosition(Vector3 screenPos, Camera camera)
        {
            return camera.ScreenToWorldPoint(screenPos);
        }

        public static Ray CreateScreenPointRay(Vector2 screenPos, Camera camera)
        {
            return camera.ScreenPointToRay(screenPos);
        }

        public static Ray CreateRay(Vector2 position)
        {
            return new Ray(position, Vector3.forward);
        }

        public static T TryGetComponentTFromGameObject<T>(Transform gameObject)
        {
            var component = gameObject.GetComponentInChildren<T>();
            if (component == null)
            {
                component = gameObject.GetComponentInParent<T>();
            }

            return component;
        }

        public static T[] TryGetComponentsTFromGameObjectArray<T>(Transform[] gameObjects)
        {
            List<T> ts = new List<T>();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i].GetComponentInChildren<T>() != null)
                {
                    ts.Add(gameObjects[i].GetComponentInChildren<T>());
                }
            }
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i].GetComponentInParent<T>() != null)
                {
                    ts.Add(gameObjects[i].GetComponentInParent<T>());
                }
            }

            return ts.ToArray();
        }

        public static T[] FindComponentsFromChildWithTag<T>(this GameObject parent, string tag, bool includeInactive = true) where T : MonoBehaviour
        {
            Transform parentTransform = parent.transform;
            List<T> list = new List<T>(parent.GetComponentsInChildren<T>(includeInactive));
            if (list.Count == 0) { return null; }

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (!list[i].CompareTag(tag))
                {
                    list.RemoveAt(i);
                }
            }

            return list.ToArray();
        }

        public static GameObject[] FindChildWithTag(GameObject parent, string tag)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform go in parent.GetComponentInChildren<Transform>())
            {
                if (go.CompareTag(tag))
                {
                    children.Add(go.gameObject);
                }
            }

            return children.ToArray();
        }

        public static Vector2 GetRandomPointInScreen(Camera cam, float dstFromTopInUnits, float dstFromSidesInUnits, float dstFromBottomInUnits)
        {
            Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize * cam.aspect));
            topRight.y = -topRight.y;
            Vector2 botLeft = new Vector2(-topRight.x, -topRight.y);

            float xVal = Random.Range((topRight.x + dstFromSidesInUnits), (botLeft.x - dstFromSidesInUnits));
            float yVal = Random.Range((topRight.y - dstFromTopInUnits), (botLeft.y + dstFromBottomInUnits));

            return new Vector2(xVal, yVal);
        }
    }
}