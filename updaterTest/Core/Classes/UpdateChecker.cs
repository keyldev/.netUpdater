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
    class UpdateChecker
    {
        private static Assembly assembly = Assembly.GetEntryAssembly();
        private static string _actual_version = $"{assembly.GetName().Version}";
        private static string _version;
        private static string _filename;

        /// <summary>
        /// A function that checks for updates from an XML file
        /// </summary>
        /// <param name="url">Link to the file</param>
        /// <param name="filename">Полное имя файла. (AppDomain)</param>
        public static void checkUpdate(string url, string filename)
        {
            WebClient web = new WebClient();
            Uri uri = new Uri(url);
            
            web.DownloadFileAsync(uri, filename);
            _filename = filename;
        }
        private static void _completedDownloading(object sender, AsyncCompletedEventArgs e)
        {
            _checkUpdate(_filename);
        }
        private static void _checkUpdate(string filename)
        {
            if (!File.Exists(filename)) return;
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
               
            }
            else return;
        }
    }
}
