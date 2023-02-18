namespace NN.Core
{
    public enum ActivationType : byte
    {
        // from 0 to 1
        Linear = 0,
        Clamp = 1,
        Sigmoid = 2,
        ReLu = 3,
        // from -1 to 1
        Hyperbolic = 4,
        ClampNegativeInclude = 5,
        ReLuNegativeInclude = 6,
        // binary
        BinaryBegin = 7,
        BinaryMiddle = 8,
        BinaryEnd = 9,
        BinaryBeginNegativeInclude = 10,
        BinaryMiddleNegativeInclude = 11,
        BinaryEndNegativeInclude = 12
    }
}