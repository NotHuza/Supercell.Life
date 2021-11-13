﻿namespace Supercell.Life.Server.Helpers
{
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Logic.Game.Objects;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal static class Extensions
    {
        /// <summary>
        /// Sends a chat message.
        /// </summary>
        internal static void SendChatMessage(this Connection connection, string message, bool system = true)
        {
            if (system)
            {
                new GlobalChatLineMessage(connection)
                {
                    Chat = new GlobalChatLine
                    {
                        Message = message,
                        Sender = connection.GameMode.Avatar,
                        System = true
                    }
                }.Send();
            }
            else
            {
                Connections.ForEach(item => new GlobalChatLineMessage(item)
                {
                    Chat = new GlobalChatLine
                    {
                        Message = message,
                        Sender = connection.GameMode.Avatar,
                        WhoSent = true,
                        Regex = true
                    }
                }.Send());
            }
        }
    }
}