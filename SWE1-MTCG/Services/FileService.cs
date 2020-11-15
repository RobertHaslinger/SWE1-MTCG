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
            _indexDirPath = _messageDirPath + "\\indices";
            _indexPath = _indexDirPath + "\\nextIndex.txt";
            _bannedIndexPath = _indexDirPath + "\\bannedIndices.txt";

            CheckDir(_indexDirPath);
            CheckFile(_indexPath);
            CheckFile(_bannedIndexPath);
        }

        private bool CheckDir(string path)
        {
            if (!Directory.Exists(Path.GetFullPath(path)))
            {
                Directory.CreateDirectory(Path.GetFullPath(path));
            }

            return true;
        }

        private bool CheckFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            return true;
        }

        private int GetNextFreeIndex()
        {
            int index=0;
            string[] bannedIndices = File.ReadAllText(_bannedIndexPath).Split(',');
            do
            {
                string fileContent = File.ReadAllText(_indexPath);
                if (!string.IsNullOrWhiteSpace(fileContent))
                {
                    index = Convert.ToInt32(fileContent);
                }
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
            content= File.ReadAllText($"{_messageDirPath}\\{id}.txt");

            return content;
        }

        public List<string> GetAllFileContents()
        {
            List<string> contents= new List<string>();
            string[] fileNames = Directory.GetFiles(_messageDirPath);
            foreach (string fileName in fileNames)
            {
                contents.Add(File.ReadAllText(Path.GetFullPath(fileName)));
            }

            return contents;
        }

        public void DeleteFile(int id)
        {
            if (!File.Exists($"{_messageDirPath}\\{id}.txt"))
                throw new IOException("No such file");
            File.Delete($"{_messageDirPath}\\{id}.txt");
        }
    }
}
