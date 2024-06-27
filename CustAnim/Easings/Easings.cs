using UnityEngine;

namespace CustAnim.Easings
{
    public static class Easings
    {
        private const float C1 = 1.70158f;
        private const float C2 = C1 * 1.525f;
        private const float C3 = C1 + 1f;
        private const float C4 = 2 * Mathf.PI / 3;
        private const float C5 = 2 * Mathf.PI / 4.5f;

        /// <summary>
        /// Modify progress (0-1) by easings functions
        /// </summary>
        /// <param name="x">Progress (from 0 to 1)</param>
        /// <param name="easing">Easing function</param>
        /// <returns></returns>
        public static float GetEasing(float x, EasingType easing)
        {
            return easing switch
            {
                EasingType.Linear => x,
                EasingType.Constant => Mathf.Floor(x),
                EasingType.InSine => 1 - Mathf.Cos((x * Mathf.PI) / 2),
                EasingType.OutSine => Mathf.Sin((x * Mathf.PI) / 2),
                EasingType.InOutSine => -(Mathf.Cos(Mathf.PI * x) - 1) / 2,
                EasingType.InQuad => x * x,
                EasingType.OutQuad => 1 - Mathf.Pow(1 - x, 2),
                EasingType.InOutQuad => x < 0.5f ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2,
                EasingType.InCubic => x * x * x,
                EasingType.OutCubic => 1 - Mathf.Pow(1 - x, 3),
                EasingType.InOutCubic => x < 0.5f ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2,
                EasingType.InQuart => x * x * x * x,
                EasingType.OutQuart => 1 - Mathf.Pow(1 - x, 4),
                EasingType.InOutQuart => x < 0.5f ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2,
                EasingType.InQuint => x * x * x * x * x,
                EasingType.OutQuint => 1 - Mathf.Pow(1 - x, 5),
                EasingType.InOutQuint => x < 0.5f ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2,
                EasingType.InExpo => x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10),
                EasingType.OutExpo => x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x),
                EasingType.InOutExpo => x == 0 ? 0 : x == 1 ? 1 :
                    x < 0.5f ? Mathf.Pow(2, 20 * x - 10) / 2 : (2 - Mathf.Pow(2, -20 * x + 10)) / 2,
                EasingType.InCirc => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)),
                EasingType.OutCirc => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)),
                EasingType.InOutCirc => x < 0.5f
                    ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
                    : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2,
                EasingType.InBack => C3 * x * x * x - C1 * x * x,
                EasingType.OutBack => 1 + C3 * Mathf.Pow(x - 1, 3) + C1 * Mathf.Pow(x - 1, 2),
                EasingType.InOutBack => x < 0.5f
                    ? (Mathf.Pow(2 * x, 2) * ((C2 + 1) * 2 * x - C2)) / 2
                    : (Mathf.Pow(2 * x - 2, 2) * ((C2 + 1) * (x * 2 - 2) + C2) + 2) / 2,
                EasingType.InElastic => x == 0 ? 0 :
                    x == 1 ? 1 : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * C4),
                EasingType.OutElastic => x == 0 ? 0 :
                    x == 1 ? 1 : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * C4) + 1,
                EasingType.InOutElastic => x == 0 ? 0 :
                    x == 1 ? 1 :
                    x < 0.5f ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * C5)) / 2 :
                    (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * C5)) / 2 + 1,
                _ => 0
            };
        }
    }
}