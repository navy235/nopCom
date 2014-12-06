namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger
{
    using ;
    using Nop.Core;
    using Nop.Core.Infrastructure;
    using Nop.Services.Logging;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web;

    public class FNSLogger
    {
        private readonly ILogger ;
        private readonly bool ;
        private readonly HttpContextBase ;

        public FNSLogger(bool showDebugInfo = false)
        {
            this. = showDebugInfo;
            this. = EngineContext.Current.Resolve<HttpContextBase>();
            this. = EngineContext.Current.Resolve<ILogger>();
        }

        public void ClearLogFile()
        {
            string systemName = PluginLog.SystemName;
            systemName.Replace(..(0x2198), ..(0x219d));
            if (systemName.Length == 0)
            {
                systemName = ..(0x21a2);
            }
            try
            {
                string path = this..Server.MapPath(..(0x21f1) + systemName + ..(0x2202));
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception exception)
            {
                this..Error(exception.Message, exception, null);
            }
        }

        public string GetLogFilePath()
        {
            string systemName = PluginLog.SystemName;
            systemName.Replace(..(0x2198), ..(0x219d));
            if (systemName.Length == 0)
            {
                systemName = ..(0x21a2);
            }
            return this..Server.MapPath(..(0x21f1) + systemName + ..(0x2202));
        }

        public void LogMessage(string message)
        {
            string systemName = PluginLog.SystemName;
            systemName.Replace(..(0x2198), ..(0x219d));
            if (systemName.Length == 0)
            {
                systemName = ..(0x21a2);
            }
            bool flag = true;
            try
            {
                if (this.)
                {
                    if (flag)
                    {
                        message = string.Format(..(0x21b7), DateTime.Now.ToString(..(0x21d0)), Environment.NewLine, message);
                        string path = this..Server.MapPath(..(0x21f1) + systemName + ..(0x2202));
                        try
                        {
                            if (File.Exists(path))
                            {
                                FileInfo info = new FileInfo(path);
                                if (info.Length > 0x3200000L)
                                {
                                    info.Delete();
                                }
                            }
                        }
                        catch
                        {
                        }
                        using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            using (StreamWriter writer = new StreamWriter(stream))
                            {
                                writer.WriteLine(message);
                            }
                            return;
                        }
                    }
                    this..Information(systemName, new NopException(message), null);
                }
            }
            catch (Exception exception)
            {
                this..Error(exception.Message, exception, null);
            }
        }
    }
}

