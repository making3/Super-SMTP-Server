using System;
using System.ServiceProcess;
using System.Threading;
using log4net;
using SuperSmtpServer;

namespace SuperSmtpService
{
    public partial class SmtpService : ServiceBase
    {
        private ILog _logger = LogManager.GetLogger("root");
        private SmtpServer _smtpServer;

        public SmtpService()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _logger.Debug("Starting dev smtp server.");
                runEmailServer(args);
            }
            catch (Exception ex)
            {
                _logger.Error("OnStart: " + ex.ToString());
            }
        }

        protected override void OnStop()
        {
            try
            {
                _logger.Debug("Stopping dev smtp server.");

                if (_smtpServer != null)
                    _smtpServer.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error("OnStop: " + ex.ToString());
            }
        }

        private void runEmailServer(string[] args)
        {
            _smtpServer = new SmtpServer();
            _smtpServer.MessageRecieved += smtp_MessageRecieved;
            _logger.Debug("Dev smtp server started.");
        }

        void smtp_MessageRecieved(System.Net.Mail.MailMessage m)
        {
            _logger.InfoFormat("Message Received\r\nTo: {0}\r\nFrom: {1}\r\nBody: {2}", m.To, m.From, m.Body);
        }
    }
}