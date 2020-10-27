using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SWE1_MTCG.Services
{
    public interface IFileService
    {

        int SaveInFile(string content);
        void SaveInFile(string content, int id);
        string GetFileContent(int id);
        List<string> GetAllFileContents();
        void DeleteFile(int id);


    }
}
