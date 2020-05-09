﻿using Lidgren.Network;
using LmpCommon.Enums;
using LmpCommon.Message.Client.Base;
using LmpCommon.Message.Data.Companies;
using LmpCommon.Message.Types;
using System;
using System.Collections.Generic;

namespace LmpCommon.Message.Client
{
    public class GroupCliMsg : CliMsgBase<CompanyBaseMsgData>
    {
        /// <inheritdoc />
        internal GroupCliMsg() { }

        /// <inheritdoc />
        public override string ClassName { get; } = nameof(GroupCliMsg);

        /// <inheritdoc />
        protected override Dictionary<ushort, Type> SubTypeDictionary { get; } = new Dictionary<ushort, Type>
        {
            [(ushort)GroupMessageType.ListRequest] = typeof(CompanyListRequestMsgData),
            [(ushort)GroupMessageType.ListResponse] = typeof(CompanyListResponseMsgData),
            [(ushort)GroupMessageType.CreateGroup] = typeof(CompanyCreateMsgData),
            [(ushort)GroupMessageType.RemoveGroup] = typeof(CompanyRemoveMsgData),
            [(ushort)GroupMessageType.GroupUpdate] = typeof(CompanyUpdateMsgData)
        };

        public override ClientMessageType MessageType => ClientMessageType.Company;

        protected override int DefaultChannel => 17;

        public override NetDeliveryMethod NetDeliveryMethod => NetDeliveryMethod.ReliableOrdered;
    }
}