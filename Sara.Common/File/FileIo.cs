using System.IO;

namespace Sara.Common.File
{
    public static class FileIo
    {
        public static bool IsFileLocked(FileInfo file)
        {
            // Source: https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                // the file is unavailable because it is:
                // still being written to
                // or being processed by another thread
                // or does not exist (has already been processed)
                return true;
            }
            finally
            {
                stream?.Close();
            }

            // file is not locked
            return false;
        }
    }
}
