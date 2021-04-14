using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using System.Xml;
using updaterTest.Core.Classes;

namespace updaterTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// доработать загрузчик. переделать в свой собственный класс. поддержка английского и русского языков. мейби опенсорс. залить на гит 
    public partial class MainWindow : Window
    {
        private string _version;
        private string _actual_version = "1.1.1";

        U_Checker checkVersion = new U_Checker();
        public MainWindow()
        {
            InitializeComponent();
            checkVersion.startUpdate("https://www.dropbox.com/s/dgnmsj3cu8l0x4n/version.xml?dl=1");
            taskLabel.Content = checkVersion.TaskText;
            //UpdateChecker.checkUpdate("https://www.dropbox.com/s/dgnmsj3cu8l0x4n/version.xml?dl=1", (AppDomain.CurrentDomain.BaseDirectory + "/version.xml").ToString());
        }
        private void _downloadXML(string url, string filename)
        {
            taskLabel.Content = "Взлом базы данных архива...";
            WebClient webClient = new WebClient();
            Uri uri = new Uri(url);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(_completedLoading);
            webClient.DownloadFileAsync(uri, AppDomain.CurrentDomain.BaseDirectory + "/" + filename);
            
        }
        private void _completedLoading(object sender, AsyncCompletedEventArgs e)
        {
            _checkVersion(AppDomain.CurrentDomain.BaseDirectory + "/version.xml");
            
        }
        
        private void _checkVersion(string filename)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/version.xml")) taskLabel.Content = "Ошибка проверки версии...";
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            foreach (XmlNode node in xml.DocumentElement)
            {
                if (node.LocalName == "version")
                {
                    _version = node.FirstChild.Value;
                    //taskLabel.Content = "Найдена новая версия.. обновляем";
                }
            }
            
            if (!_actual_version.Equals(_version))
            {
                taskLabel.Content = "Найдена новая версия.. обновляем";
                _startUpdate();
            }
            else taskLabel.Content = "Обновление не требуется...";
        }

        private void _startUpdate()
        {
            WebClient web = new WebClient();
            web.DownloadProgressChanged += new DownloadProgressChangedEventHandler(_updateProgressChanged);
            Uri uri = new Uri("https://www.dropbox.com/s/zxroapyuuqy7bjm/archive.zip?dl=1");
            web.DownloadFileAsync(uri, AppDomain.CurrentDomain.BaseDirectory + "/" + "file.zip");
            //web.DownloadFileAsync();
        }
        private void _updateProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            downloadProgress.Value += e.ProgressPercentage;
        }
    }
}
