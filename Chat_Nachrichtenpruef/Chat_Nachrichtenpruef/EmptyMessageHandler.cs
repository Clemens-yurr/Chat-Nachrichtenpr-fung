using Chat_Nachrichtenpruef;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Chat_Nachrichtenpruef
{
    public class EmptyMessageHandler : ChatHandler
    {
        public override (bool isValid, string reason) Handle(string message, List<string> trace)
        {
            trace.Add("EmptyMessageHandler");

            bool isEmpty = true;
            foreach (char c in message)
            {
                if (!char.IsWhiteSpace(c)) isEmpty = false;
            }

            if (message.Length == 0 || isEmpty)
            {
                return (false, "Leere Nachricht");
            }

            if (next != null) return next.Handle(message, trace);
            return (true, "OK");
        }
    }
}