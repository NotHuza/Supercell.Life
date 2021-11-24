﻿namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;

    internal class LogicUpgradeShipCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUpgradeShipCommand"/> class.
        /// </summary>
        public LogicUpgradeShipCommand(Connection connection) : base(connection)
        {
            // LogicSpeedUpShipCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.ShipLevel == 0)
            {
                gamemode.Avatar.ShipLevel = 1;
            }
            else
            {
                LogicDataTable globals = CSV.Tables.Get(Gamefile.Globals);

                int cost  = ((LogicGlobalData)globals.GetDataByName("SHIP_UPGRADE_COST")).NumberArray.Find(value => value == gamemode.Avatar.ShipLevel);
                int xpLvl = ((LogicGlobalData)globals.GetDataByName("SHIP_UPGRADE_REQUIRED_XP_LEVEL")).NumberArray.Find(value => value == gamemode.Avatar.ExpLevel);

                if (gamemode.Avatar.Gold >= cost && gamemode.Avatar.ExpLevel >= xpLvl)
                {
                    gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, -cost);
                    gamemode.Avatar.ShipUpgrade.Start();
                }
            }
        }
    }
}
