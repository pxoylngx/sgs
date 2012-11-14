﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Sanguosha.Core.Triggers;
using Sanguosha.Core.Cards;
using Sanguosha.Core.UI;
using Sanguosha.Core.Skills;
using Sanguosha.Expansions.Basic.Cards;
using Sanguosha.Core.Games;
using Sanguosha.Core.Players;
using Sanguosha.Core.Exceptions;

namespace Sanguosha.Expansions.Wind.Skills
{
    /// <summary>
    /// 天香-每当你受到伤害时，你可以弃置一张红桃手牌来转移此伤害给一名其他角色，然后该角色摸X张牌(X为该角色当前已损失的体力值)。
    /// </summary>
    public class TianXiang : TriggerSkill
    {
        public class TianXiangVerifier : CardsAndTargetsVerifier
        {

            public TianXiangVerifier()
            {
                minPlayers = 1;
                maxPlayers = 1;
                minCards = 1;
                maxCards = 1;
                discarding = true;
            }

            protected override bool VerifyPlayer(Player source, Player player)
            {
                return player != source;
            }

            protected override bool VerifyCard(Player source, Card card)
            {
                return card.Suit == SuitType.Heart;
            }
       }

        void Run(Player Owner, GameEvent gameEvent, GameEventArgs eventArgs)
        {
            ISkill skill;
            List<Card> cards;
            List<Player> players;
            if (Game.CurrentGame.UiProxies[Owner].AskForCardUsage(new CardUsagePrompt("TianXiang"), new TianXiangVerifier(),
                out skill, out cards, out players))
            {
                Game.CurrentGame.HandleCardDiscard(Owner, cards);
                Game.CurrentGame.DoDamage(eventArgs.Source, players[0], -eventArgs.IntArg - eventArgs.IntArg3, (DamageElement)eventArgs.IntArg2, eventArgs.Card, eventArgs.ReadonlyCard);
                Game.CurrentGame.DrawCards(players[0], players[0].MaxHealth - players[0].Health);
                throw new TriggerResultException(TriggerResult.End);
            }
        }

        public TianXiang()
        {
            var trigger = new AutoNotifyPassiveSkillTrigger(
                this,
                Run,
                TriggerCondition.OwnerIsTarget
            );

            Triggers.Add(GameEvent.DamageInflicted, trigger);
        }

    }
}