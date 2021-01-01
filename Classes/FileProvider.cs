using System;
using System.Collections.Generic;
using System.Text;
using FolderPreservationCut.Interfaces;
using System.IO;
using System.ComponentModel;
using System.Linq;

namespace FolderPreservationCut.Classes
{
    class FileProvider : IFileProvider
    {
        [Description("Determines if a given directory exists")]
        public bool FindFolderLocation(string folderPath)
        {
            return Directory.Exists(folderPath);
        }

        [Description("Finds All Folders in the given location")]
        public List<string> FindAllFolders(string path)
        {
            IEnumerable<string> rawFolders = Directory.EnumerateDirectories(path);
            List<string> parsedFolders = new List<string>();
            foreach (var rawFolder in rawFolders)
            {
                string replacedString = rawFolder.Replace(path, "");
                parsedFolders.Add(replacedString); 
            }

            return parsedFolders;
        }

        public int CreateFolders(IEnumerable<string> foldersToCreate, string path)
        {
            List<string> errorFolders = new List<string>();
            
            foreach(string folder in foldersToCreate)
            {
                try
                {
                    Directory.CreateDirectory(path + folder);
                }
                catch (Exception)   // generic error catch
                {
                    errorFolders.Add(path + folder);
                }
                
            }

            if (errorFolders.Count >= 1)
            {
                Console.WriteLine("+++ Error creating the following folders: ");
                foreach (var folder in errorFolders)
                {
                    Console.WriteLine(folder);
                }
            }
            
            return 1;
        }

        [Description("Find all files in a given directory listing")]
        public List<string> FindAllFiles(string path)
        {
            IEnumerable<string> rawFiles = Directory.EnumerateFiles(path);// Possibly change this to get inner folders - maybe on a mark II version
            List<string> parsedFiles = new List<string>();
            foreach (var rawFolder in rawFiles)
            {
                string replacedString = rawFolder.Replace(path, "");
                parsedFiles.Add(replacedString);
            }

            return parsedFiles;
        }

        [Description("Copy a file from one location to another")]
        public int CopyFile(string startPath, string endPath)
        {
            try
            {
                File.Copy(startPath, endPath);
            }
            catch (Exception error )
            {
                Console.WriteLine($"+++ Error copying file: ${startPath} : Error: {error.Message} +++");
                return 0;
            }
            
            return 1;
        }

        [Description("Delete a specified file")]
        public int DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception error)
            {
                Console.WriteLine($"+++ Error deleting file: ${path} : Error: {error.Message} +++");
                return 0;
            }

            return 1;
        }
    }
}
