using LmpCommon.Message.Types;

namespace LmpCommon.Message.Data.Companies
{
    public class CompanyListRequestMsgData : CompanyBaseMsgData
    {
        /// <inheritdoc />
        internal CompanyListRequestMsgData() { }
        public override GroupMessageType GroupMessageType => GroupMessageType.ListRequest;

        public override string ClassName { get; } = nameof(CompanyListRequestMsgData);
    }
}
