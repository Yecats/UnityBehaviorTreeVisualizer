using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WUG.BehaviorTreeVisualizer
{
    public static class GeneralExtensions
    {
        #region Color
        /// <summary>
        /// Get color with new alpha
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static Color FromRGB(this Color color, int r, int g, int b, int a = 255)
        {
            color.r = Mathf.Clamp((float)r, 0, 255) / 255;
            color.g = Mathf.Clamp((float)g, 0, 255) / 255;
            color.b = Mathf.Clamp((float)b, 0, 255) / 255;
            color.a = Mathf.Clamp((float)a, 0, 255) / 255;

            return color;
        }
        #endregion

        public static string BTDebugLog(this string message)
        {
            Debug.Log($"<b><color=\"#00DCFF\">[Behavior Tree Debugger]</color></b> {message}");

            return message;
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            System.Random rng = new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }

    }
}