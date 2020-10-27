using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SWE1_MTCG.Services
{
    public class FileService : IFileService
    {
        //Get the execution dir of the application to store messages in this dir
        private readonly string _basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private readonly string _messageDirPath;
        private readonly string _indexDirPath;
        private readonly string _indexPath;
        private readonly string _bannedIndexPath;

        public FileService()
        {
            _messageDirPath = _basePath + "\\messages";
            _indexDirPath = _basePath + "\\indices";
            _indexPath = _indexDirPath + "\\nextIndex.txt";
            _bannedIndexPath = _indexDirPath + "\\bannedIndices.txt";
        }

        private bool CheckDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return true;
        }

        private bool CheckFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            return true;
        }

        private int GetNextFreeIndex()
        {
            CheckDir(_indexDirPath);
            CheckFile(_indexPath);
            CheckFile(_bannedIndexPath);

            int index;
            string[] bannedIndices = File.ReadAllText(_bannedIndexPath).Split(',');
            do
            {
                index = Convert.ToInt32(File.ReadAllText(_indexPath));
                File.WriteAllText(_indexPath, (index + 1).ToString());
            } while (bannedIndices.Contains(index.ToString()));

            return index;
        }

        public int SaveInFile(string content)
        {
            int index = GetNextFreeIndex();
            string filePath = $"{_messageDirPath}\\{index}.txt";

            File.WriteAllText(filePath, content);
            return index;
        }

        public void SaveInFile(string content, int id)
        {
            string bannedIndices = File.ReadAllText(_bannedIndexPath);
            bannedIndices += $"{id},";
            File.WriteAllText(_bannedIndexPath, bannedIndices);

            string filePath = $"{_messageDirPath}\\{id}.txt";
            File.WriteAllText(filePath, content);
        }

        public string GetFileContent(int id)
        {
            string content;
            content= File.ReadAllText($"{_messageDirPath}\\{id}");

            return content;
        }

        public List<string> GetAllFileContents()
        {
            List<string> contents= new List<string>();
            string[] fileNames = Directory.GetFiles(_messageDirPath);
            foreach (string fileName in fileNames)
            {
                contents.Add(File.ReadAllText($"{_messageDirPath}\\{fileName}"));
            }

            return contents;
        }

        public void DeleteFile(int id)
        {
            File.Delete($"{_messageDirPath}\\{id}");
        }
    }
}
