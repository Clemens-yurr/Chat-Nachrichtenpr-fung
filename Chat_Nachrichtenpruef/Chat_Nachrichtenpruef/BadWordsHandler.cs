using Chat_Nachrichtenpruef;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Chat_Nachrichtenpruef
{
    public class BadWordsHandler : ChatHandler
    {
        private List<string> badWords = new List<string> { "blöd", "dumm", "idiot" };

        public override (bool isValid, string reason) Handle(string message, List<string> trace)
        {
            trace.Add("BadWordsHandler");

            string lowerMsg = message.ToLower();
            foreach (var word in badWords)
            {
                if (lowerMsg.Contains(word))
                {
                    return (false, "Unangemessene Sprache");
                }
            }

            if (next != null) return next.Handle(message, trace);
            return (true, "OK");
        }
    }
}