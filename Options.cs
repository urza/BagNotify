using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagNotify
{
    class Options
    {
        /// <summary>
        /// Minutes
        /// </summary>
        public int Interval { get; set; }

        public string Symbol { get; set; } = "BTCUSDT";
        public float? LessThan { get; set; } = null;
        public float? MoreThan { get; set; } = null;

        /// <summary>
        /// sending emails on error
        /// </summary>
        public bool SentEmailOnError { get; set; } = false;
        public string EmailTo { get; set; }
        public string Smtp { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpUsername { get; set; }
        public string EmailFrom { get; set; }
        public string EmailFromName { get; set; }
        public string EmailSubject { get; set; } = "bagnotify";
        
        //public string EmailBody { get; set; }
    }
}
