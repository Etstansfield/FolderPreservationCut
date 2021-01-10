using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Linq;
using FolderPreservationCut.Classes;
using FolderPreservationCut.Interfaces;

namespace FolderPreservationCut
{
    class Fpc
    {
        private IFileProvider _fp;

        public Fpc(IFileProvider fileProvider)
        {
            _fp = fileProvider;
        }
        
        [Description("Cuts all files from one location to another while preserving the folder structure")]
        public int Cut(string startLocation, string endLocation)
        {
            int totalFiles = 0;
            int copiedFiles = 0;

            if (String.IsNullOrEmpty(startLocation))
            {
                throw new ArgumentException("Start Location is Required!");
            }

            if (String.IsNullOrEmpty(endLocation))
            {
                throw new ArgumentException("End Location is Required!");
            }

            Console.WriteLine($"+++ Cutting Files From {startLocation} to {endLocation} +++");
            
            // now that we have both the start and end locations - we want to attempt to read both locations
            bool startLocationExists = _fp.FindFolderLocation(startLocation);
            bool endLocationExists = _fp.FindFolderLocation(endLocation);

            if (!startLocationExists)
            {
                throw new FileNotFoundException("Error accessing start location!");
            }

            if (!endLocationExists)
            {
                throw new FileNotFoundException("Error accessing end location!");
            }
            
            // now that we have determined that both locations exist - enumerate the folders in both - so we can compare the difference and create the necessary folders
            IEnumerable<string> startLocationFolders = _fp.FindAllFolders(startLocation);
            IEnumerable<string> endLocationFolders = _fp.FindAllFolders(endLocation);
            
            // we have all folders - compare those in the end location to those in the start location
            // and create the missing folders
            IEnumerable<string> missingFolders = startLocationFolders.Except(endLocationFolders);

            if (missingFolders.Count() > 0)
            {
                Console.WriteLine("The following folders are in StartLocation but not EndLocation and will be created: ");
                foreach (string s in missingFolders)
                {
                    Console.WriteLine(s);
                }

                // now create the missing folders
                _fp.CreateFolders(missingFolders, endLocation);
            }

            
            
            // now we need to go through all folders and find the files in each and copy them over - then delete the original file
            // TODO: - Add a argument wether we want to override existing files

            foreach (var folder in startLocationFolders)
            {
                List<string> files = _fp.FindAllFiles(startLocation + folder);
                List<string> endFiles = _fp.FindAllFiles(endLocation + folder);
                IEnumerable<string> missingFiles = files.Except(endFiles);
                // list all the files - just for debugging
                totalFiles += missingFiles.Count();

                foreach (string file in missingFiles)
                {
                    PrintCurrentCopyingFile(file);
                    // now copy the missing files over
                    int result = _fp.CopyFile(startLocation + folder + file, endLocation + folder + file);
                    copiedFiles += result;

                }
                
                // now delete the original files - regardless of if they were missing or not
                // TODO - Add an argument to delete only the copied files

                foreach (string file in files)
                {
                     _fp.DeleteFile(startLocation + folder + file);
                }
            }
            Console.Write('\r');
            Console.WriteLine($"+++ {copiedFiles}/{totalFiles} Files Moved Successfully +++");
            return 1;
        }

        private void PrintCurrentCopyingFile(string filename)
        {
            Console.Write('\r');
            Console.Write($"+++ Cutting: {filename} +++");
        }

        
    }
}
