using System.Collections.Generic;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Html;
using SudokuBrain.Models;

namespace SudokuBrain.Services
{
    public class MessageLog : IMessageLogger
    {
        private List<LogItem> _messages;

        public MessageLog()
        {
            _messages = new List<LogItem>();
        }

        public void Log(string owner, string message, LogItemLevel level = LogItemLevel.Normal)
        {
            _messages.Add(new LogItem(owner, message, level));
        }

        public HtmlString RenderLog(bool includeDebug = false)
        {
            var sb = new StringBuilder();
            foreach (var item in _messages)
            {
                switch (item.Level)
                {
                    case LogItemLevel.Debug:
                        if (includeDebug)
                        {
                            sb.AppendLine(HttpUtility.HtmlEncode(item.ToString()) + "<br>");
                        }
                        break;
                    case LogItemLevel.Problem:
                        sb.AppendLine("<span class=\"problem\">" + HttpUtility.HtmlEncode(item.ToString()) + "</span><br>");
                        break;
                    default:
                        sb.AppendLine(HttpUtility.HtmlEncode(item.ToString()) + "<br>");
                        break;

                }
            }
            return new HtmlString(sb.ToString());
        }
    }
}