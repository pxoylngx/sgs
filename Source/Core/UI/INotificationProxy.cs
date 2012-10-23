﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanguosha.Core.Cards;
using Sanguosha.Core.Players;
using Sanguosha.Core.Skills;
using Sanguosha.Core.Games;

namespace Sanguosha.Core.UI
{
    public interface INotificationProxy
    {
        void NotifyCardMovement(List<CardsMovement> m, List<UI.IGameLog> notes);
    }
}