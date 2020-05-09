using Lidgren.Network;
using LmpCommon.Enums;
using LmpCommon.Message.Data.Companies;
using LmpCommon.Message.Server.Base;
using LmpCommon.Message.Types;
using System;
using System.Collections.Generic;

namespace LmpCommon.Message.Server
{
    public class CompanySrvMsg : SrvMsgBase<CompanyBaseMsgData>
    {
        /// <inheritdoc />
        internal CompanySrvMsg() { }

        /// <inheritdoc />
        public override string ClassName { get; } = nameof(CompanySrvMsg);

        /// <inheritdoc />
        protected override Dictionary<ushort, Type> SubTypeDictionary { get; } = new Dictionary<ushort, Type>
        {
            [(ushort)GroupMessageType.ListRequest] = typeof(CompanyListRequestMsgData),
            [(ushort)GroupMessageType.ListResponse] = typeof(CompanyListResponseMsgData),
            [(ushort)GroupMessageType.CreateGroup] = typeof(CompanyCreateMsgData),
            [(ushort)GroupMessageType.RemoveGroup] = typeof(CompanyRemoveMsgData),
            [(ushort)GroupMessageType.GroupUpdate] = typeof(CompanyUpdateMsgData)
        };

        public override ServerMessageType MessageType => ServerMessageType.Company;

        protected override int DefaultChannel => 18;

        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;
    }
}