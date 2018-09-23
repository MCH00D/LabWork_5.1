using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LabWork_5._1
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var userProfilePath = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            var desktopPath = Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            StreamWriter logStream;

            Console.WriteLine($"Path to the {Environment.UserName} folder: {userProfilePath}");
            Console.WriteLine($"Path to the desktop: {desktopPath}");

            using (logStream = new StreamWriter(desktopPath + "\\Old files from user profile.txt"))
            {
                CheckPathForFilesOldestThenNDays(userProfilePath, logStream);
            }

            Console.WriteLine("Сreated a new file on your desktop");
            Console.ReadLine();

        }

        public static void CheckPathForFilesOldestThenNDays(string path, StreamWriter logStream, int days = 15, int level = 0)
        {
            var dir = new DirectoryInfo(path);
            var prefix = new String('-', level);
            var dateForCheck = DateTime.Now.AddDays(-days);

            try
            {
                var filesOldestThenNDays = new List<FileInfo>();
                var files = dir.GetFiles();

                foreach (var file in files)
                {
                    if (file.CreationTime >= dateForCheck)
                    {
                        filesOldestThenNDays.Add(file);
                    }
                }

                if (filesOldestThenNDays.Count != 0)
                {
                    logStream.WriteLine(prefix + files[0].DirectoryName);

                    foreach (var file in files)
                    {
                        logStream.WriteLine(prefix + file.Name);
                    }
                }

                var subDirs = dir.GetDirectories();
                foreach (var subDir in subDirs)
                {
                    CheckPathForFilesOldestThenNDays(subDir.FullName, logStream, days, level++);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }
        }
    }
}
