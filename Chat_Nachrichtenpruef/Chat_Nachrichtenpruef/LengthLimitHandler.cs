using Chat_Nachrichtenpruef;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Chat_Nachrichtenpruef
{
    public class LengthLimitHandler : ChatHandler
    {
        public override (bool isValid, string reason) Handle(string message, List<string> trace)
        {
            trace.Add("LengthLimitHandler");

            if (message.Length > 120)
            {
                return (false, "Nachricht zu lang");
            }

            if (next != null) return next.Handle(message, trace);
            return (true, "OK");
        }
    }
}