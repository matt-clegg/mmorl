using Lidgren.Network;

namespace MMORL.Shared.Net.Messages
{
    public class LoginMessage : IMessage
    {
        public MessageType Type => MessageType.Login;

        public string Token { get; set; }

        public LoginMessage() { }

        public LoginMessage(string token)
        {
            Token = token;
        }

        public void Read(NetIncomingMessage message)
        {
            Token = message.ReadString();
        }

        public void Write(NetOutgoingMessage message)
        {
            message.Write(Token);
        }
    }
}
