using Lidgren.Network;
using LmpCommon.Message.Base;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Companies
{
    public class CompanyCreateMsgData : CompanyBaseMsgData
    {
        /// <inheritdoc />
        internal CompanyCreateMsgData() { }
        public override GroupMessageType GroupMessageType => GroupMessageType.CreateGroup;

        public string CompanyName;

        public override string ClassName { get; } = nameof(CompanyCreateMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);

            lidgrenMsg.Write(CompanyName);
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);

            CompanyName = lidgrenMsg.ReadString();
        }

        internal override int InternalGetMessageSize()
        {
            return base.InternalGetMessageSize() + CompanyName.GetByteCount();
        }
    }
}
