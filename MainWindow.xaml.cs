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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath;
        string[] files;
        byte[] buff = null;


        public MainWindow()
        {
            InitializeComponent();
        }

        private string RunScript() 
        {
            ProcessStartInfo start = new ProcessStartInfo();
            Runtime.PythonDLL = @"C:\Users\osman\AppData\Local\Programs\Python\Python312\python312.dll";
            Environment.SetEnvironmentVariable("PYTHONPATH", @"C:\Users\osman\AppData\Local\Programs\Python\Python312\Lib\site-packages");
            PythonEngine.Initialize();
            PythonEngine.PythonPath = @"C:\Users\osman\AppData\Local\Programs\Python\Python312\Lib\site-packages";
            start.Arguments = "qiskit2.py";
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

                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                long num = new FileInfo(filePath).Length;
                buff = binaryReader.ReadBytes((int)num);

            }
            
            
        }
        private void EncryptFileCommand_Click(object sender, RoutedEventArgs e)
        {

            int asd = e.GetHashCode();
            string v = asd.ToString();
            string keyData = RunScript();
            string parameter = getParameter(keyData);
            string key = getKey(keyData);

        }

        private string getParameter(string keyData)
        {
            string parameter = keyData.Substring(98,20);
            return parameter;
        }
        private string getKey(string keyData) 
        {
            string key = keyData.Substring(9, 85);
            return key;
        }

    }
    
}
