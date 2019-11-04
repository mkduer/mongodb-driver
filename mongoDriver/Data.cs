using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace mongoDriver
{
    public class Data
    {
        private String _dataDir;
        public String DataDir
        {
            get { return this._dataDir; }
            set { this._dataDir = value; }
        }

        /// <summary>Checks is specified data storing directory exists</summary>
        /// <returns>True if directory with data was found, False, otherwise</returns>
        public bool getDataDirectory()
        {
            // if working directory is successfully found, look for the specific sub-directory containing the dataset
            if (this.getWorkingDirectory() == true) {
                this._dataDir += "\\csv_files";

                if (Directory.Exists(this._dataDir))
                    return true;
            }
            throw new DirectoryNotFoundException();
        }

        /// <summary>
        /// Retrieves current working directory based on the OS's current file path
        /// relative to the "mongoDriver" directory
        /// </summary>
        /// <param name="path">Optional value allowing for a provided path or testing path, defaults to an empty string</param>
        /// <returns>True if directory was found, False, otherwise</returns>
        public bool getWorkingDirectory(String path = "")
        {
            return _getWorkingDirectory(path);
        }

        /// <summary>Given a paramterized path, provides all of the subdirectories as a JSON document</summary>
        /// <param name="currentDir">
        /// The current directory that is being searched, which defaults to an empty string. 
        /// (note: if a valid directory is not provided, the top-level data directory is used inside the program)
        /// </param>
        /// <returns>JSON of subdirectory</returns>
        public String getSubDirectories(String currentDir = "")
        {
            if (currentDir == "")
                currentDir = _dataDir;

            var items = _getSubDirectories(currentDir + "\\");
            return JsonConvert.SerializeObject(items);
        }

        /// <summary>
        /// Retrieves current working directory based on the OS's current file path
        /// relative to the "mongoDriver" directory
        /// </summary>
        /// <param name="path">Optional value allowing for a provided path or testing path, defaults to an empty string</param>
        /// <returns>True if directory was found, False, otherwise</returns>
        private bool _getWorkingDirectory(String path = "")
        {
            // Set directory to provided path for unique OS or testing scenarios
            this._dataDir = path;

            // Otherwise, set path to the executing path
            // source for finding executing path: https://stackoverflow.com/questions/15653921/get-current-folder-path
            if (this._dataDir == "") {
                this._dataDir = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                this._dataDir = Path.GetDirectoryName(this._dataDir);
            }

            // if expected directory does not exist, return False
            if (this._dataDir.Contains("mongoDriver") == false) {
                throw new DirectoryNotFoundException();
            }

            // if unnecessary prepend substring exists for Windows, remove the substring
            String windowsSubstring = "file:\\";
            if (this._dataDir.Contains(windowsSubstring) == true) {
                this._dataDir = this._dataDir.Replace(windowsSubstring, "");
            }

            // Find the directory path to `...mongo-driver\monogoDriver\` by removing the tail-end of the directory path
            // Example: 
            // Starting path: `file:\C:\Users\username\ . . . \mongodb-driver\mongoDriver\bin\Debug\netcoreapp3.0`
            // Resulting path: `file:\C:\Users\username\ . . . \mongodb-driver\mongoDriver\`
            String[] workingDir = this._dataDir.Split('\\');
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
                this._dataDir = this._dataDir.Replace(removeStr, "");

            return true;
        }

        /// <summary>Given a paramterized path, provides all of the subdirectories as a list of strings</summary>
        /// <param name="currentDir">The current directory that is being searched</param>
        /// <returns>A list of strings representing each directory or null if invalid</returns>
        private Queue<String> _getSubDirectories(String currentDir)
        {
            Queue<String> itemsQueue = null;

            // if the current directory is an invalid path, return null
            if (string.IsNullOrEmpty(currentDir))
                return itemsQueue;

            // Grab subdirectories and enqueue
            String[] subDirectories = Directory.GetDirectories(currentDir);

            if (subDirectories.Length > 0) {
                itemsQueue = new Queue<string>();
            }

            foreach (String dir in subDirectories) { 
                String newDir = dir.Replace(currentDir, "");
                itemsQueue.Enqueue(newDir);
            }
            return itemsQueue;
        }
    }
}
