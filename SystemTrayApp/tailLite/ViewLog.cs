using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ViewTailLogFile
{
    public class ViewLog
    {
        TailForm mdiForm = new TailForm();
        private static TailConfig LoadSessionFile(string filepath)
        {
            TailConfig tailConfig = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TailConfig));
                using (XmlTextReader reader = new XmlTextReader(filepath))
                {
                    //  _currenTailConfig = new Uri(reader.BaseURI).LocalPath;
                    tailConfig = serializer.Deserialize(reader) as TailConfig;
                }
                return tailConfig;
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    errorMsg += "\n" + ex.Message;
                }
                return null;
            }
        }
        TailFileConfig _defaultTailConfig = null;
        public ViewLog(string logfilename)
        {
           
            if (_defaultTailConfig == null)
            {
                TailConfig tailConfig1 = null;
                string defaultPath = "ViewLog.xml";
                if (!string.IsNullOrEmpty(defaultPath))
                    tailConfig1 = LoadSessionFile(defaultPath);
                if (tailConfig1 != null && tailConfig1.TailFiles.Count > 0)
                {
                    _defaultTailConfig = tailConfig1.TailFiles[0];
                    _defaultTailConfig.Title = null;
                }
                else
                {
                    _defaultTailConfig = new TailFileConfig();
                }
            }
            var configPath = Directory.GetCurrentDirectory();
            
            TailFileConfig tailConfig = _defaultTailConfig;
            tailConfig.FilePath = logfilename;
            mdiForm.LoadConfig(tailConfig, configPath);
            // mdiForm.MdiParent = this;   
        }
        public void Show()
        {
            mdiForm.Show();
            
        }
    }
}
