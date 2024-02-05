using System.Collections.Concurrent;
using System.Reflection;
using System.Xml.Linq;

namespace EbeninQueue
{
    public class QueuesHelper
    {
        private static ConcurrentDictionary<string, ConcurrentQueue<string>> QueueChannels { get; set; } = new();

        public static MoResponse<object> ChannelInfoHtml()
        {
            string tr = "";

            var q = QueuesHelper.QueueChannels.Select(s => new { s.Key, s.Value.Count });

            foreach (var item in q)
            {
                tr += $"<tr> <td>{item.Key}</td> <td>{item.Count}</td> <td><a href=\"/QueuesApi/QueueInfo/{item.Key}\" target=\"_blank\">details</a></td> </tr>";
            }

            string th = "<tr> <th>Channel Name</th> <th>Queue Count</th> <th></th> </tr>";
            string tabloHtml = $"<table class=\"table w-50 \" >{th}{tr}</table>";

            MoResponse<object> response = new()
            {
                Data = tabloHtml,
                IsSuccess = true
            };

            return response;
        }

        public static MoResponse<object> QueueInfoHtml(string channel)
        {
            string tr = "";
            if (QueuesHelper.QueueChannels.ContainsKey(channel))
            {
                var q = QueuesHelper.QueueChannels[channel].ToList().Select(s => s);

                foreach (var item in q)
                {
                    tr += $"<tr> <td>{item}</td> <td>{item}</td> </tr>";
                }
            }

            string th = "<tr> <th>Element Name</th> <th>Priority</th> </tr>";
            string tablo = $"<table class=\"table w-50 \" >{th}{tr}</table>";

            string link = "";
            link += "<meta charset=\"utf-8\">";
            link += "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">";
            link += "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css\" rel=\"stylesheet\">";
            link += "<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js\"></script>";
            link += "";


            string style = "";
            style += "html {color:black}";

            string script = "";
            script += "setInterval(function () {window.location.reload();}, 5000);";

            string header = $"<p>{channel} Detail</p>";

            string content = $"<html> <title>Lite MQ</title> <head>{link} <style>{style}</style> <script>{script}</script> </head> <body> {header} {tablo}</body></html>";

            MoResponse<object> response = new()
            {
                Data = content,
                IsSuccess = true
            };

            return response;
        }

        public static MoResponse<object> NewChannel(string channel)
        {
            MoResponse<object> response = new();

            if (!QueuesHelper.QueueChannels.ContainsKey(channel))
            {
                QueuesHelper.QueueChannels.TryAdd(channel, new());
                response.IsSuccess = true;
            }
            else
            {
                response.Messages.Add("Element already exists");
            }

            return response;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static MoResponse<object> Enqueue(string channel, string element, int periority = 0)
        {
            MoResponse<object> response = new();
            try
            {
                if (!QueuesHelper.QueueChannels.ContainsKey(channel))
                {
                    QueuesHelper.QueueChannels.TryAdd(channel, new());
                }

                if (!QueuesHelper.QueueChannels[channel].Contains(element))
                {
                    QueuesHelper.QueueChannels[channel].Enqueue(element);
                    response.IsSuccess = true;
                }
                else
                {
                    response.Messages.Add("Element already exists");
                }
            }
            catch (Exception ex)
            {
                response.Messages.Add(ex.Message + "  InnerException:" + ex.InnerException?.Message);
            }

            return response;
        }

        public static MoResponse<MoEnqueue> Dequeue(string channel)
        {
            MoResponse<MoEnqueue> response = new();

            if (QueuesHelper.QueueChannels.ContainsKey(channel))
            {
                QueuesHelper.QueueChannels[channel].TryDequeue(out string? element);

                response.Data = new MoEnqueue()
                {
                    Channel = channel,
                    Element = element ?? "",
                    Periority = 2
                };

                response.IsSuccess = !string.IsNullOrEmpty(element);
            }
            else
            {
                response.Messages.Add($"Channel not found {channel}");
            }

            return response;
        }

        public static MoResponse<MoEnqueue> Peek(string channel)
        {
            MoResponse<MoEnqueue> response = new();

            if (QueuesHelper.QueueChannels.ContainsKey(channel))
            {
                QueuesHelper.QueueChannels[channel].TryPeek(out string? element);

                response.Data = new MoEnqueue()
                {
                    Channel = channel,
                    Element = element ?? "",
                    Periority = 0
                };

                response.IsSuccess = !string.IsNullOrEmpty(element);
            }
            else
            {
                response.Messages.Add($"Channel not found {channel}");
            }

            return response;
        }

    }

}