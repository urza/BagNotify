using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BagNotify
{
    class Program
    {
        public static Options options { get; set; } = new Options();
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");

            if (args.Length > 0)
            {
                TryParseOptions(args);
            }
            else
            {
                Console.WriteLine("Usage: --Symbol=XMRBTC --LessThan=0.005 --MoreThan=0.007 --Interval=5");
                return;
            }

            using var httpClient = new HttpClient();

            while (true)
            {
                try
                {
                    var res = await httpClient.GetFromJsonAsync<PriceResponse>("https://api.binance.com/api/v3/ticker/price?symbol=" + options.Symbol);

                    if (res != null)
                    {
                        if (res.price <= options.LessThan)
                            PriceAlert(res);

                        else if (res.price >= options.MoreThan)
                            PriceAlert(res);
                        
                        else
                            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} : {res.symbol} : {res.price}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(options.Interval));
                }
            }
        }

        private static void TryParseOptions(string[] args)
        {
            try
            {
                ParseOptions(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private static void PriceAlert(PriceResponse res)
        {
            PrintAlert(res);

            if (options.SentEmailOnAlert)
                MailAlert(res);
        }

        private static void MailAlert(PriceResponse res)
        {
            SendMail.sendEmail(sentFrom: options.EmailFrom, fromName: options.EmailFromName, emailTo: options.EmailTo,
                                subject: options.EmailSubject, htmlBody: $"PRICE ALERT {res.symbol} {res.price}",
                                smtp: options.Smtp, smtpUserName: options.SmtpUsername, smtpPassword: options.SmtpPassword);
        }

        private static void PrintAlert(PriceResponse res)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ALERT ALERT ALERT !!!! PRICE OF {res.symbol} = {res.price} ALERT ALERT ALERT !!!");
            Console.ResetColor();
        }

        private static void ParseOptions(string[] args)
        {
            foreach (var arg in args.Select(x => x.ToLower()))
            {
                if (arg.Contains("LessThan".ToLower()))
                {
                    var items = arg.Split('=');
                    options.LessThan = items[1].FloatParseCzEn();
                }
                if (arg.Contains("MoreThan".ToLower()))
                {
                    var items = arg.Split('=');
                    options.MoreThan = items[1].FloatParseCzEn();
                }
                if (arg.Contains("Interval".ToLower()))
                {
                    var items = arg.Split('=');
                    options.Interval = int.Parse(items[1]);
                }
                if (arg.Contains("Symbol".ToLower()))
                {
                    var items = arg.Split('=');
                    options.Symbol = items[1].ToUpper();
                }
                if (args.Contains("SentEmailOnAlert".ToLower()))
                {
                    var items = arg.Split('=');
                    options.SentEmailOnAlert = bool.Parse(items[1]);
                }
                if (args.Contains("EmailTo".ToLower()))
                {
                    var items = arg.Split('=');
                    options.EmailTo = items[1];
                }
                if (args.Contains("Smtp".ToLower()))
                {
                    var items = arg.Split('=');
                    options.Smtp = items[1];
                }
                if (args.Contains("SmtpPassword".ToLower()))
                {
                    var items = arg.Split('=');
                    options.SmtpPassword = items[1];
                }
                if (args.Contains("SmtpUsername".ToLower()))
                {
                    var items = arg.Split('=');
                    options.SmtpUsername = items[1];
                }
                if (args.Contains("EmailFrom".ToLower()))
                {
                    var items = arg.Split('=');
                    options.EmailFrom = items[1];
                }
                if (args.Contains("EmailFromName".ToLower()))
                {
                    var items = arg.Split('=');
                    options.EmailFromName = items[1];
                }
                if (args.Contains("EmailSubject".ToLower()))
                {
                    var items = arg.Split('=');
                    options.EmailSubject = items[1];
                }
            }
        }
    }
}
