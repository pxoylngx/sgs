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

namespace Sanguosha.Expansions.Fire.Skills
{
    /// <summary>
    /// 双雄-摸牌阶段，你可以放弃摸牌，改为进行一次判定，你获得此判定牌，且此回合你的每张与该判定牌不同颜色的手牌均可当【决斗】使用。
    /// </summary>
    public class ShuangXiong : TriggerSkill
    {
        public class GetJudgeCardTrigger : Trigger
        {
            public override void Run(GameEvent gameEvent, GameEventArgs eventArgs)
            {
                if (eventArgs.Source != Owner)
                {
                    return;
                }
                Game.CurrentGame.UnregisterTrigger(GameEvent.PlayerJudgeDone, this);
                //someone already took it...
                if (Game.CurrentGame.Decks[eventArgs.Source, DeckType.JudgeResult].Count == 0)
                {
                    return;
                }
                Game.CurrentGame.HandleCardTransferToHand(Owner, Owner, new List<Card>(Game.CurrentGame.Decks[eventArgs.Source, DeckType.JudgeResult]));
                return;
            }
            public GetJudgeCardTrigger(Player p)
            {
                Owner = p;
            }
        }

        void Run(Player Owner, GameEvent gameEvent, GameEventArgs eventArgs)
        {
            Game.CurrentGame.RegisterTrigger(GameEvent.PlayerJudgeDone, new GetJudgeCardTrigger(Owner) { Priority = int.MinValue });
            var result = Game.CurrentGame.Judge(Owner, this);
            ISkill skill = new ShuangXiongCardTransformSkill(result.SuitColor);
            Game.CurrentGame.PlayerAcquireSkill(Owner, skill, true);
            Game.CurrentGame.RegisterTrigger(GameEvent.PhasePostEnd, new TriggerRemoval(Owner, skill));
            Game.CurrentGame.CurrentPhaseEventIndex++;
            throw new TriggerResultException(TriggerResult.End);
        }


        class TriggerRemoval : Trigger
        {
            private ISkill skill;

            public TriggerRemoval(Player Owner, ISkill skill)
            {
                this.Owner = Owner;
                this.skill = skill;
            }

            public override void Run(GameEvent gameEvent, GameEventArgs eventArgs)
            {
                if (eventArgs.Source == Owner)
                {
                    Game.CurrentGame.PlayerLoseSkill(Owner, skill, true);
                    Game.CurrentGame.UnregisterTrigger(GameEvent.PhasePostEnd, this);
                }
            }
        }

        public ShuangXiong()
        {
            var trigger = new AutoNotifyPassiveSkillTrigger(
                this,
                Run,
                TriggerCondition.OwnerIsSource
            );
            Triggers.Add(GameEvent.PhaseBeginEvents[TurnPhase.Draw], trigger);
            IsAutoInvoked = false;
        }

        class ShuangXiongCardTransformSkill : OneToOneCardTransformSkill
        {
            public ShuangXiongCardTransformSkill()
            {
                HandCardOnly = true;
            }

            SuitColorType color;
            public ShuangXiongCardTransformSkill(SuitColorType color)
            {
                this.color = color;
            }

            public override bool VerifyInput(Card card, object arg)
            {
                return card.SuitColor != color;
            }

            public override CardHandler PossibleResult
            {
                get { return new JueDou(); }
            }
        }
    }
}
