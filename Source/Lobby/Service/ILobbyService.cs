﻿using Sanguosha.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Sanguosha.Lobby.Core
{
    public enum LoginStatus
    {
        Success = 0,
        OutdatedVersion = 1,
        InvalidUsernameAndPassword,
    }
    public struct LoginToken
    {
        public Guid token;
    }

    [DataContract(Name = "RoomOperationResult")]
    public enum RoomOperationResult
    {
        [EnumMember]
        Success = 0,
        [EnumMember]
        Auth = -1,
        [EnumMember]
        Full = -2,
        [EnumMember]
        Password = -3,
        [EnumMember]
        Locked = -4,
        [EnumMember]
        Invalid = -5,
    }

    [ServiceKnownType("GetKnownTypes", typeof(Helper))]
    [ServiceContract(Namespace = "", CallbackContract = typeof(IGameClient), SessionMode = SessionMode.Required)]
    public interface ILobbyService
    {
        [OperationContract(IsInitiating = true)]
        LoginStatus Login(int version, string username, out LoginToken token, out Account retAccount);

        [OperationContract(IsInitiating = false)]
        void Logout(LoginToken token);

        [OperationContract]
        IEnumerable<Room> GetRooms(LoginToken token, bool notReadyRoomsOnly);

        [OperationContract]
        Room CreateRoom(LoginToken token, string password = null);

        [OperationContract]
        RoomOperationResult EnterRoom(LoginToken token, int roomId, bool spectate, string password, out Room room);

        [OperationContract]
        RoomOperationResult ExitRoom(LoginToken token, int roomId);

        [OperationContract]
        RoomOperationResult ChangeSeat(LoginToken token, int newSeat);

        [OperationContract]
        RoomOperationResult StartGame(LoginToken token);

        [OperationContract]
        RoomOperationResult Ready(LoginToken token);

        [OperationContract]
        RoomOperationResult Kick(LoginToken token, int seatNo);
    }

    public interface IGameClient
    {
        [OperationContract(IsOneWay = true)]
        void NotifyRoomUpdate(int id, Room room);

        [OperationContract(IsOneWay = true)]
        void NotifyKicked();

        [OperationContract(IsOneWay = true)]
        void NotifyGameStart(string connectionString);
    }

    static class Helper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            System.Collections.Generic.List<System.Type> knownTypes =
                new System.Collections.Generic.List<System.Type>();
            // Add any types to include here.
            knownTypes.Add(typeof(Room));
            knownTypes.Add(typeof(Seat));
            knownTypes.Add(typeof(Account));
            return knownTypes;
        }
    }

}
