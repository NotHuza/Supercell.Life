﻿namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicChangeAllianceRoleCommand : LogicServerCommand
    {
        internal Alliance Alliance;
        internal Alliance.Role Role;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChangeAllianceRoleCommand"/> class.
        /// </summary>
        public LogicChangeAllianceRoleCommand(Connection connection) : base(connection)
        {
            this.Type     = Command.ChangeAllianceRole;
            this.Alliance = this.Connection.GameMode.Avatar.Alliance;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLogicLong(this.Alliance.Identifier);
            encoder.WriteInt((int)this.Role);
        }
    }
}
