﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanguosha.Core.Triggers;
using Sanguosha.Core.Cards;
using Sanguosha.Core.UI;
using Sanguosha.Core.Skills;
using Sanguosha.Core.Games;
using Sanguosha.Core.Heroes;
using Sanguosha.Expansions.OverKnightFame11.Skills;

namespace Sanguosha.Expansions.OverKnightFame11
{
    public class OverKnightFame11Expansion : Expansion
    {
        public OverKnightFame11Expansion()
        {
            CardSet = new List<Card>();

            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("CaoZhi", true, Allegiance.Wei, 3, new LuoYing(), new JiuShi()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("ZhangChunHua", true, Allegiance.Wei, 3, new JueQing()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("YuJin", true, Allegiance.Wei, 4, new YiZhong()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("XuShu", true, Allegiance.Shu, 3, new WuYan(), new JuJian()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("FaZheng", true, Allegiance.Shu, 3, new XuanHuo(), new EnYuan()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("LingTong", true, Allegiance.Wu, 4, new XuanFeng()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("XuSheng", true, Allegiance.Wu, 4, new PoJun()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("WuGuoTai", true, Allegiance.Wu, 3, new BuYi(), new GanLu()))));
            CardSet.Add(new Card(SuitType.None, -1, new HeroCardHandler(new Hero("MaSu", true, Allegiance.Shu, 3, new XinZhan(), new HuiLei()))));
        }
    }
}
