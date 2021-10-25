﻿namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicMoveCharacterCommand : LogicCommand
    {
        internal int DirectionX, DirectionY;

        internal byte Value;

        internal bool S, F;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMoveCharacterCommand"/> class.
        /// </summary>
        public LogicMoveCharacterCommand(Connection connection) : base(connection)
        {
            this.Type = Command.MoveCharacter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMoveCharacterCommand"/> class.
        /// </summary>
        public LogicMoveCharacterCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicMoveCharacterCommand.
        }

        internal override void Decode()
        {
            this.DirectionX = this.Stream.ReadInt();
            this.DirectionY = this.Stream.ReadInt();

            this.Value      = this.Stream.ReadByte();

            this.S = this.Value == 1 || this.Value == 3;
            this.F = this.Value >= 2;

            this.ReadHeader();
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.DirectionX);
            this.Stream.WriteInt(this.DirectionY);

            this.Stream.WriteByte(this.Value);

            this.WriteHeader();
        }

        internal override void Execute()
        {
            if (this.Connection.Avatar.OngoingQuestData != null)
            {
                if (this.Connection.Avatar.OngoingQuestData.Data.QuestType != "PvP")
                {
                    if (this.DirectionX + 256 >= 512)
                    {
                        Debugger.Error("Invalid Dx");
                        return;
                    }

                    if (this.DirectionY + 256 >= 512)
                    {
                        Debugger.Error("Invalid Dy");
                        return;
                    }

                    var dX = LogicMath.Abs(this.DirectionX) + 256;

                    if (LogicMath.Abs(this.DirectionY) + dX < 10)
                    {
                        return;
                    }

                    this.Connection.Avatar.OngoingQuestData.SublevelMoveCount += 1;

                    var ongoingLevel = this.Connection.Avatar.OngoingQuestData.Levels[this.Connection.Avatar.OngoingQuestData.Level];
                    ongoingLevel.Battles[this.Connection.Avatar.Quests[this.Connection.Avatar.OngoingQuestData.GlobalID].Levels[0].CurrentBattle].CheckCollision(this.DirectionX, this.DirectionY);
                }
            }

            var battle = this.Connection.Avatar.Battle;

            if (battle != null)
            {
                battle.Turn++;

                var opponent = battle.Avatars.Find(avatar => avatar.Identifier != this.Connection.Avatar.Identifier);

                LogicMoveCharacterCommand cmd = new LogicMoveCharacterCommand(opponent.Connection)
                {
                    DirectionX     = this.DirectionX,
                    DirectionY     = this.DirectionY,
                    Value          = this.Value,
                    ExecuteSubTick = this.ExecuteSubTick,
                    ExecutorID     = this.ExecutorID
                };

                battle.GetOwnQueue(this.Connection.Avatar).Enqueue(this);
                battle.GetEnemyQueue(this.Connection.Avatar).Enqueue(cmd);
            }

            Debugger.Debug($"DirectionX : {this.DirectionX}, DirectionY : {this.DirectionY}, S : {this.S}, F : {this.F}");
        }
    }
}