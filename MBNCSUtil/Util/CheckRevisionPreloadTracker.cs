using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BNSharp.MBNCSUtil.Util
{
    internal static class CheckRevisionPreloadTracker
    {
        private static Dictionary<string, Stream> cachedFiles = new Dictionary<string, Stream>();

        internal static void ClearCaches()
        {
            cachedFiles.Clear();
        }

        public static Stream GetFiles(IEnumerable<string> fileNames)
        {
            string completeFileList = JoinWith(fileNames, ' ');
            Stream str = null;
            if (!cachedFiles.TryGetValue(completeFileList, out str))
            {
                int totalLength = 0;
                foreach (string file in fileNames)
                {
                    FileInfo fi = new FileInfo(file);
                    totalLength += CalculatePaddedBufferSize((int)fi.Length);
                }

                byte[] data = new byte[totalLength];
                int currentPosition = 0;
                foreach (string file in fileNames)
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        int amountRead = fs.Read(data, currentPosition, (int)fs.Length);
                        currentPosition += amountRead;
                        int remainder = (int)(fs.Length % 1024);
                        if (remainder > 0)
                        {
                            int difference = 1024 - remainder;

                            byte currentPaddingByte = 0xff;
                            for (int i = 0; i < difference; i++)
                            {
                                unchecked
                                {
                                    data[currentPosition++] = currentPaddingByte--;
                                }
                            }
                        }
                    }
                }

                MemoryStream ms = new MemoryStream(data, false);
                if (CheckRevision.OptimizationStrategy == CheckRevisionOptimizationStrategy.PreloadAndPersistFiles)
                    cachedFiles.Add(completeFileList, ms);

                str = ms;

                File.WriteAllBytes("c:\\projects\\comparison.bin", data);
            }

            return str;
        }

        private static int CalculatePaddedBufferSize(int baseFileSize)
        {
            int remainder = baseFileSize % 1024;
            if (remainder == 0)
                return baseFileSize;
            else
                return baseFileSize - remainder + 1024;
        }

        private static string JoinWith(IEnumerable<string> strings, char joiner)
        {
            List<string> source = new List<string>(strings);
            StringBuilder sb = new StringBuilder();
            if (source.Count > 0)
                sb.Append(source[0]);
            for (int i = 1; i < source.Count; i++)
            {
                sb.Append(joiner);
                sb.Append(source[i]);
            }

            return sb.ToString();
        }
    }
}
