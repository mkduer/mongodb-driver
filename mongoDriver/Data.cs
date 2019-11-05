using System;
using System.IO;
using MongoDB.Bson;

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

        /// <summary>Given a paramterized path, provides all of the subdirectories as a BSON document</summary>
        /// <param name="currentDir">The current directory to search for subdirectories</param>
        /// <returns>A BSON document representing each directory or null if invalid/empty</returns>
        public BsonDocument getSubDirectories(String currentDir = "")
        {
            if (currentDir == "")
                currentDir = _dataDir;

            return _getSubDirectories(currentDir + "\\");
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

        /// <summary>Given a paramterized path, provides all of the subdirectories as a BSON document</summary>
        /// <param name="currentDir">The current directory to search for subdirectories</param>
        /// <returns>A BSON document representing each directory or null if invalid/empty</returns>
        private BsonDocument _getSubDirectories(String currentDir)
        {
            String[] subDirectories = null;
            BsonDocument items = null;
            int count = 1;

            // if the current directory is an invalid path, return null
            if (string.IsNullOrEmpty(currentDir))
                return items;

            // Grab subdirectories (full-path)
            subDirectories = Directory.GetDirectories(currentDir);

            if (subDirectories.Length > 0) {
                items = new BsonDocument();
            }

            // Create a BSON document with the key/values
            foreach (String dir in subDirectories) { 
                BsonValue itemValue = dir.Replace(currentDir, "");
                items.Add(count.ToString(), itemValue);
                count++;
            }
            return items;
        }
    }
}
