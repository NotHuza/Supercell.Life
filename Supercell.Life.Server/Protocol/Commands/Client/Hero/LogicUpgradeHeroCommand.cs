﻿namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;

    internal class LogicUpgradeHeroCommand : LogicCommand
    {
        private LogicHeroData Hero;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUpgradeHeroCommand"/> class.
        /// </summary>
        public LogicUpgradeHeroCommand(Connection connection) : base(connection)
        {
            // LogicUpgradeHeroCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.Hero = stream.ReadDataReference<LogicHeroData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (this.Hero != null)
            {
                if (gamemode.Avatar.HeroUpgrade.CanUpgrade(this.Hero))
                {
                    if (gamemode.Avatar.ExpLevel < this.Hero.RequiredXpLevel)
                    {
                        Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) is not at the required level. (Level : {gamemode.Avatar.ExpLevel}, Require : {this.Hero.RequiredXpLevel})");
                        return;
                    }

                    if (!string.IsNullOrEmpty(this.Hero.RequiredQuest))
                    {
                        if (!gamemode.Avatar.NpcProgress.ContainsKey(this.Hero.RequiredQuestData.GlobalID))
                        {
                            Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) has not unlocked the required quest.");
                            return;
                        }
                    }

                    string[] cost = this.Hero.Cost[gamemode.Avatar.HeroLevels.GetCount(this.Hero.GlobalID)].Split(',');

                    if (cost.Length >= 7)
                    {
                        int diamonds = LogicStringUtil.ConvertToInt(cost[0]);
                        int gold     = LogicStringUtil.ConvertToInt(cost[1]);
                        int energy   = LogicStringUtil.ConvertToInt(cost[2]);
                        int orb1     = LogicStringUtil.ConvertToInt(cost[3]);
                        int orb2     = LogicStringUtil.ConvertToInt(cost[4]);
                        int orb3     = LogicStringUtil.ConvertToInt(cost[5]);
                        int orb4     = LogicStringUtil.ConvertToInt(cost[6]);

                        if (diamonds != 0)
                        {
                            if (gamemode.Avatar.Diamonds < diamonds)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough diamonds. (Diamonds : {gamemode.Avatar.Diamonds}, Require : {diamonds})");
                                return;
                            }
                        }

                        if (gold != 0)
                        {
                            if (gamemode.Avatar.Gold < gold)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough gold. (Gold : {gamemode.Avatar.Gold}, Require : {gold})");
                                return;
                            }
                        }

                        if (energy != 0)
                        {
                            if (gamemode.Avatar.Energy < energy)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough energy. (Energy : {gamemode.Avatar.Energy}, Require : {energy}.");
                                return;
                            }
                        }
                        
                        if (orb1 != 0)
                        {
                            if (gamemode.Avatar.Orb1 < orb1)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb1. (Orb1 : {gamemode.Avatar.Orb1}, Require : {orb1})");
                                return;
                            }
                        }

                        if (orb2 != 0)
                        {
                            if (gamemode.Avatar.Orb2 < orb2)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb2. (Orb2 : {gamemode.Avatar.Orb2}, Require : {orb2})");
                                return;
                            }
                        }

                        if (orb3 != 0)
                        {
                            if (gamemode.Avatar.Orb3 < orb3)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb3. (Orb3 : {gamemode.Avatar.Orb3}, Require : {orb3})");
                                return;
                            }
                        }

                        if (orb4 != 0)
                        {
                            if (gamemode.Avatar.Orb4 < orb4)
                            {
                                Debugger.Error($"Unable to upgrade the hero. {gamemode.Avatar.Name} ({gamemode.Avatar}) does not have enough of orb4. (Orb4 : {gamemode.Avatar.Orb4}, Require : {orb4})");
                                return;
                            }
                        }

                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -diamonds);
                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, -gold);
                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Energy, -energy);
                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb1, -orb1);
                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb2, -orb2);
                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb3, -orb3);
                        gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Orb4, -orb4);
                    }

                    gamemode.Avatar.HeroUpgrade.Start(this.Hero);
                }
            }
        }
    }
}