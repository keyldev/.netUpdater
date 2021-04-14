using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace updaterTest.Core.Classes
{
    class U_Checker
    {
        private string _taskText;
        private string _fileURL;
        private string _filename = AppDomain.CurrentDomain.BaseDirectory + "version.xml";
        private static Assembly assembly = Assembly.GetEntryAssembly();
        private string _currentVersion = $"{assembly.GetName().Version}";
        private string _version;


        /// <summary>
        /// Проверка наличия файла на сервере
        /// </summary>
        /// <param name="url">ссылка на файл</param>
        /// <returns></returns>
        private bool _isFileExistOnServer(string url)
        {
            _taskText = "Проверка на наличие файла на сервере..";
            Uri uri = new Uri(url);
            WebClient web = new WebClient();
            web.DownloadFileCompleted += new AsyncCompletedEventHandler(_completedDownloading);
            try
            {
                web.DownloadFileAsync(uri, _filename);
            }
            catch (WebException)
            {
                return false;
            }
            return true;
        }

        private void _completedDownloading(object sender, AsyncCompletedEventArgs e)
        {
            _version = _getVersion();
            if (!_isVersionEquals()) _downloadUpdate();
            else _taskText = "Обновление не требуется.";
        }

        /// <summary>
        /// Текст нынешней задачи.
        /// </summary>
        public string TaskText
        {
            get { return _taskText; }
        }
        /// <summary>
        /// Получения версии для вывода на загрузочный экран
        /// </summary>
        public string Version
        {
            get { return _currentVersion; }
        }

        public async void startUpdate(string url)
        {
            _fileURL = url;
            _taskText = "Test is work?";
            await Task.Run(() => _isFileExistOnServer(url));
        }
        private bool _isVersionEquals()
        {
            if (!_currentVersion.Equals(_version)) return false;
            else return true;
        }
        private string _getVersion()
        {
            string temp_version;
            if (!File.Exists(_filename)) _taskText = "Ошибка получения доступа к файлу";
            XmlDocument xml = new XmlDocument();
            xml.Load(_filename);
            foreach (XmlNode node in xml.DocumentElement)
            {
                if (node.LocalName == "version")
                {
                    temp_version = node.FirstChild.Value;
                    return temp_version;
                }
            }
            return null;
        }
        private void _downloadUpdate()
        {
            _taskText = "Загрузка обновления.";
        }
    }
}
