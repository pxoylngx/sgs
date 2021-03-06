﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sanguosha.Core.UI;
using Sanguosha.Core.Skills;
using Sanguosha.Core.Players;
using Sanguosha.Core.Games;
using Sanguosha.Core.Triggers;
using Sanguosha.Core.Exceptions;
using Sanguosha.Core.Cards;

namespace Sanguosha.Expansions.Basic.Cards
{
    
    public class CardWrapper : CardTransformSkill
    {
        public override VerifierResult TryTransform(List<Card> cards, object arg, out CompositeCard card)
        {
            card = new CompositeCard();
            card.Type = handler;
            card.Subcards = new List<Card>(cards);
            return VerifierResult.Success;
        }

        CardHandler handler;

        public CardWrapper(Player p, CardHandler h)
        {
            Owner = p;
            handler = h;
        }

        protected override void NotifyAction(Player source, List<Player> targets, CompositeCard card)
        {
        }
    }
}
