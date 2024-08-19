using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using Python.Runtime;
using WpfApp2;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.IO.Compression;
using Encrypter;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath;
        string[] files;
        byte[] buff = new byte[8192];

        
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void scriptInit()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            Runtime.PythonDLL = @"C:\Users\osman\AppData\Local\Programs\Python\Python312\python312.dll";
            Environment.SetEnvironmentVariable("PYTHONPATH", @"C:\Users\osman\AppData\Local\Programs\Python\Python312\Lib\site-packages");
            PythonEngine.Initialize();
            PythonEngine.PythonPath = @"C:\Users\osman\AppData\Local\Programs\Python\Python312\Lib\site-packages";
            start.Arguments = "qiskit2.py";
        }

        private string RunScript() 
        {
            scriptInit();
            using (Py.GIL())
            {
                
                var script = Py.Import("qiskit2");
                
                var quasiDist = script.InvokeMethod("getQuasi");
                var key = script.InvokeMethod("getParameter");
                String quasi = quasiDist.ToString();
                String key2 = key.ToString();
                String quasiParameter = $"{quasi}+{key2}";
                return quasiParameter;
            }
        }
        
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                string filepath = openFileDialog.FileName;
                MessageBox.Show(filepath);
            }
        }
        private void FileDropStackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                filePath = files[0];
                string filename = System.IO.Path.GetFileName(files[0]);
                FileNameLabel.Content = filename;
                filePath = System.IO.Path.GetFullPath(files[0]);

                DirectoryInfo directory = new DirectoryInfo(filePath);
                DirectorySecurity ds = directory.GetAccessControl();
                ds.AddAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, 
                                                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, 
                                                        PropagationFlags.None, AccessControlType.Allow ));

                byte[] bytes = zipBytes(filePath);
                string s = Encoding.UTF8.GetString(bytes);
                File.WriteAllText("C:\\Users\\osman\\source\\repos\\WpfApp2\\Trials.txt", s);
                byteToZip(bytes);
                Array.Clear(bytes);
            }
            
            
        }

        private byte[] zipBytes(string zipPath)
        {
            FileInfo fileInfo = new FileInfo(zipPath);
            long fileSize = fileInfo.Length-1;
            byte[] zipBytes = new byte[fileSize];
            using (MemoryStream stream = new MemoryStream())
            {
                using (FileStream fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(stream);
                }
                zipBytes = stream.ToArray();
            }
            return zipBytes;
        }

        private void byteToZip(byte[] bytes)
        {

            string outPath = "C:\\Users\\osman\\source\\repos\\WpfApp2\\Output\\denemeler.zip";
            DirectoryInfo directory = new DirectoryInfo(outPath);
            DirectorySecurity ds = directory.GetAccessControl();
            ds.AddAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl,
                                                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                                    PropagationFlags.None, AccessControlType.Allow));
            directory.SetAccessControl(ds);
            using (FileStream fs = new FileStream(outPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        
        private void EncryptFileCommand_Click(object sender, RoutedEventArgs e)
        {

            int asd = e.GetHashCode();
            string v = asd.ToString();
            string keyData = RunScript();
            string parameter = getParameter(keyData);
            string key = getKey(keyData);
            KeyElements elements = new KeyElements(key);
            byte[] iv = new byte [16];
            iv = elements.IVGenerator(key);
            byte[] assd = iv;
        }

        private string getParameter(string keyData)
        {
            string parameter = keyData.Substring(286,20);
            return parameter;
        }
        private string getKey(string keyData) 
        {
            string key = keyData.Substring(9, 274);
            return key;
        }

        void Encryptor(string key, string IVKey, string inputFile, string outputFile)
        {

        }
    }
    
}
