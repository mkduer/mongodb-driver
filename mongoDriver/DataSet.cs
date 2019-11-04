using System;
using System.IO;

namespace mongoDriver
{
    public class DataSet
    {
        private String _workingDir;
        public String WorkingDir
        {
            get { return _workingDir; }
            set { _workingDir = value; }
        }

        /// <summary>
        /// Retrieves current working directory based on the OS's current file path
        /// relative to the "mongoDriver" directory
        /// </summary>
        /// <param name="path">Optional value allowing for a provided path or testing path, defaults to an empty string</param>
        /// <returns>True if directory with data was found, False, otherwise</returns>
        public bool getWorkingDir(String path="")
        {
            return _getWorkingDir(path);
        }

        /// <summary>
        /// Retrieves current working directory based on the OS's current file path
        /// relative to the "mongoDriver" directory
        /// </summary>
        /// <param name="path">Optional value allowing for a provided path or testing path, defaults to an empty string</param>
        /// <returns>True if directory with data was found, False, otherwise</returns>
        private bool _getWorkingDir(String path="")
        {
            // Set directory to provided path for unique OS or testing scenarios
            _workingDir = path;

            // Otherwise, set path to the executing path
            // source for finding executing path: https://stackoverflow.com/questions/15653921/get-current-folder-path
            if (_workingDir == "") { 
                _workingDir = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                _workingDir = Path.GetDirectoryName(_workingDir);
            }

            // if expected directory does not exist, return False
            if (_workingDir.Contains("mongoDriver") == false) {
                throw new DirectoryNotFoundException();
            }

            // Find the directory path to `...mongo-driver\monogoDriver\` by removing the tail-end of the directory path
            // Example: 
            // Starting path: `file:\C:\Users\username\ . . . \mongodb-driver\mongoDriver\bin\Debug\netcoreapp3.0`
            // Resulting path: `file:\C:\Users\username\ . . . \mongodb-driver\mongoDriver\`
            String[] workingDir = _workingDir.Split('\\');
            String removeStr = "";
            bool startStorage = false;

            foreach (String part in workingDir) {
                if (startStorage == true) { 
                    removeStr = removeStr + "\\" + part;
                }
                if (part == "mongoDriver") {
                    startStorage = true;
                }
            }

            if (removeStr != "")
                _workingDir = _workingDir.Replace(removeStr, "");

            return true;
        }

    }
}
