using System.Collections.Generic;

namespace Chat_Nachrichtenpruef
{
    public abstract class ChatHandler
    {
        protected ChatHandler next;

        public ChatHandler SetNext(ChatHandler nextHandler)
        {
            next = nextHandler;
            return nextHandler;
        }

        public abstract (bool isValid, string reason) Handle(string message, List<string> trace);
    }
}