using System;
using System.Collections.Generic;
using System.Text;

namespace FolderPreservationCut.Interfaces
{
    public interface IFileProvider
    {
        public bool FindFolderLocation(string folderPath);

        public List<string> FindAllFolders(string path);

        public int CreateFolders(IEnumerable<string> folders, string path);

        public List<string> FindAllFiles(string path);

        public int CopyFile(string startPath, string endPath);

        public int DeleteFile(string path);
    }
}
