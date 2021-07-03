using System.IO;

namespace Game.SerializationSaver
{
    public static class SaverStatic
    {
        public const string LOAD_EXCEPTION = " : This file or directory doesn\'t exist";
        public static readonly object EMPTY = new object();

        public static string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}