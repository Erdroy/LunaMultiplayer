using Lidgren.Network;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Companies
{
    public class CompanyUpdateMsgData : CompanyBaseMsgData
    {
        /// <inheritdoc />
        internal CompanyUpdateMsgData() { }
        public override GroupMessageType GroupMessageType => GroupMessageType.GroupUpdate;

        public Company Company = new Company();

        public override string ClassName { get; } = nameof(CompanyUpdateMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);

            Company.Serialize(lidgrenMsg);
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);

            Company.Deserialize(lidgrenMsg);
        }

        internal override int InternalGetMessageSize()
        {
            return base.InternalGetMessageSize() + Company.GetByteCount();
        }
    }
}
