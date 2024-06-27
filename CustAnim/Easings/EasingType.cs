using UnityEngine;

namespace CustAnim.Easings
{
    /// <summary>
    /// Types of smoothness functions
    /// </summary>
    public enum EasingType
    {
        Linear = 0, Constant = 1,// Usual
        InSine = 2, OutSine = 3, InOutSine = 4,// Sin functions
        InQuad = 5, OutQuad = 6, InOutQuad = 7,// 2-power functions (square)
        InCubic = 8, OutCubic = 9, InOutCubic = 10,// 3-power functions (cube)
        InQuart = 11, OutQuart = 12, InOutQuart = 13,// 4-power functions (tesseract)
        InQuint = 14, OutQuint = 15, InOutQuint = 16,// 5-power functions
        InExpo = 17, OutExpo = 18, InOutExpo = 19,// Exponentionally functions
        InCirc = 20, OutCirc = 21, InOutCirc = 22,// Circle functions
        InBack = 23, OutBack = 24, InOutBack = 25,// Inertial functions
        InElastic = 26, OutElastic = 27, InOutElastic = 28// Elastic functions
    }
}
