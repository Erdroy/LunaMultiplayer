using Lidgren.Network;
using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Companies
{
    public class CompanyListResponseMsgData : CompanyBaseMsgData
    {
        /// <inheritdoc />
        internal CompanyListResponseMsgData() { }
        public override GroupMessageType GroupMessageType => GroupMessageType.ListResponse;

        public int CompaniesCount;
        public Company[] Companies = new Company[0];

        public override string ClassName { get; } = nameof(CompanyListResponseMsgData);

        internal override void InternalSerialize(NetOutgoingMessage lidgrenMsg)
        {
            base.InternalSerialize(lidgrenMsg);

            lidgrenMsg.Write(CompaniesCount);
            for (var i = 0; i < CompaniesCount; i++)
            {
                Companies[i].Serialize(lidgrenMsg);
            }
        }

        internal override void InternalDeserialize(NetIncomingMessage lidgrenMsg)
        {
            base.InternalDeserialize(lidgrenMsg);

            CompaniesCount = lidgrenMsg.ReadInt32();
            if (Companies.Length < CompaniesCount)
                Companies = new Company[CompaniesCount];

            for (var i = 0; i < CompaniesCount; i++)
            {
                if (Companies[i] == null)
                    Companies[i] = new Company();

                Companies[i].Deserialize(lidgrenMsg);
            }
        }

        internal override int InternalGetMessageSize()
        {
            var arraySize = 0;
            for (var i = 0; i < CompaniesCount; i++)
            {
                arraySize += Companies[i].GetByteCount();
            }

            return base.InternalGetMessageSize() + sizeof(int) + arraySize;
        }
    }
}
