using Chat_Nachrichtenpruef;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Chat_Nachrichtenpruef
{
    public class ShoutingHandler : ChatHandler
    {
        public override (bool isValid, string reason) Handle(string message, List<string> trace)
        {
            trace.Add("ShoutingHandler");

            int letters = 0;
            int uppers = 0;

            foreach (char c in message)
            {
                if (char.IsLetter(c))
                {
                    letters++;
                    if (char.IsUpper(c)) uppers++;
                }
            }

            if (letters > 0 && (double)uppers / letters >= 0.8)
            {
                return (false, "Bitte nicht schreien");
            }

            if (next != null) return next.Handle(message, trace);
            return (true, "OK");
        }
    }
}