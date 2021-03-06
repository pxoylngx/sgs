﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Sanguosha.Core.Cards;
using Sanguosha.Core.Players;
using Sanguosha.Core.Skills;
using Sanguosha.Core.Games;
using System.Diagnostics;
using System.IO;

namespace Sanguosha.Core.Network
{
    public class Client
    {
        ItemReceiver receiver;
        ItemSender sender;
        int commId;
        
        public int SelfId { get; set; }

        private string ipString;

        public string IpString
        {
            get { return ipString; }
            set { ipString = value; }
        }
        private int portNumber;

        public int PortNumber
        {
            get { return portNumber; }
            set { portNumber = value; }
        }

        public Client()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isReplay">Set true if this is client is connected to a replayFile</param>
        /// <param name="replayStream"></param>
        /// <exception cref="System.ArgumentOutOfRangeException" />
        /// <exception cref="System.Net.Sockets.SocketException" />
        public void Start(Stream recordStream = null)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IpString), PortNumber);
            TcpClient client = new TcpClient();
            client.Connect(ep);
            NetworkStream stream = client.GetStream();
            receiver = new ItemReceiver(stream, recordStream);
            sender = new ItemSender(stream);
            commId = 0;
        }

        public void StartReplay(Stream replayStream)
        {
            receiver = new ItemReceiver(replayStream);
            sender = new ItemSender(Stream.Null);
            commId = 0;
        }

        public Stream RecordStream
        {
            get
            {
                if (receiver == null) return receiver.RecordStream;
                else return null;
            }
            set
            {
                Trace.Assert(receiver != null);
                receiver.RecordStream = value;
            }
        }

        public void MoveHandCard(int from, int to)
        {
            HandCardMovement move = new HandCardMovement();
            move.playerId = SelfId;
            move.from = from;
            move.to = to;
            CommandItem item = new CommandItem();
            item.command = Command.Interrupt;
            item.obj = move;
            sender.Send(item);
        }

        public object Receive()
        {
            object o = receiver.Receive();
            
            if (o is CommandItem)
            {
                CommandItem item = (CommandItem)o;
                if (item.command == Command.Interrupt)
                {
                    if (item.obj is CardChoiceCallback)
                    {
                        CardChoiceCallback cbi = (CardChoiceCallback)item.obj;
                        Game.CurrentGame.NotificationProxy.NotifyCardChoiceCallback(cbi.o);
                    }
                    if (item.obj is CardUsageResponded)
                    {
                        var cbi = (CardUsageResponded)item.obj;
                        Game.CurrentGame.NotificationProxy.NotifyMultipleCardUsageResponded(Game.CurrentGame.Players[cbi.playerId]);
                    }
                    return Receive();
                }
            }
            else if (o is PlayerItem)
            {
                PlayerItem i = (PlayerItem)o;
                o = Game.CurrentGame.Players[i.id];
            }
            else if (o is int)
            {
                Trace.TraceInformation("Received a {0}", (int)o);
            }
            else if (o is CardItem)
            {
                CardItem i = (CardItem)o;
                return Translator.DecodeForClient(i, SelfId);
            }
            else if (o is SkillItem)
            {
                return Translator.Translate((SkillItem)o);
            }
            return o;
        }

        public void AnswerNext()
        {
            sender.Send(new CommandItem() { command = Command.QaId, data = commId });
        }

        public void NextComm()
        {
            commId++;
        }

        public void AnswerItem(Card card)
        {            
            sender.Send(Translator.TranslateForClient(card));
        }

        public void AnswerItem(ISkill skill)
        {
            if (skill is CheatSkill)
            {
                sender.Send(skill);
            }
            else
            {
                sender.Send(Translator.Translate(skill));
            }
        }

        public void AnswerItem(int i)
        {            
            sender.Send(i);
        }

        public void AnswerItem(Player p)
        {
            sender.Send(p);
        }

        public void Flush()
        {
            sender.Flush();
        }

        public void CardChoiceCallBack(object obj)
        {
            CommandItem item = new CommandItem();
            item.command = Command.Interrupt;
            item.obj = new CardChoiceCallback() { o = obj };
            sender.Send(item);
        }

        public void Stop()
        {
            if (receiver.RecordStream != null)
            {
                receiver.RecordStream.Flush();
                receiver.RecordStream.Close();
            }
        }
    }
}
