using Microsoft.AspNetCore.SignalR;

namespace BlazorChatSignalR.Server.Hubs
{
    public class ChatHub :Hub
    {
        private static Dictionary<string, string> Usuarios = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            string usuarioNome = Context.GetHttpContext().Request.Query["usuarioNome"];

            Usuarios.Add(Context.ConnectionId, usuarioNome);

            await AdicionarMensagemAoBatePapoAsync(string.Empty, $"{usuarioNome} Entrou no Chat!");
            await base.OnConnectedAsync();
        }

        public async Task AdicionarMensagemAoBatePapoAsync(string usuario, string mensagem)
        {
            await Clients.All.SendAsync("ReceberMensagem", usuario, mensagem);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string usuarioNome = Usuarios.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
            await AdicionarMensagemAoBatePapoAsync(string.Empty, $"{usuarioNome} left!");
        }
    }
}
