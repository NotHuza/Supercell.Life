﻿namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicChangeNameCommand : LogicServerCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChangeNameCommand"/> class.
        /// </summary>
        public LogicChangeNameCommand(Connection connection) : base(connection)
        {
            this.Type = Command.ChangeName;
        }

        internal override void Encode(ByteStream stream)
        {
            stream.WriteString(this.Connection.GameMode.Avatar.Name);
            stream.WriteBoolean(this.Connection.GameMode.Avatar.NameSetByUser);

            base.Encode(stream);
        }
    }
}