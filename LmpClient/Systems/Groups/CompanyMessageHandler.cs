using System;
using System.Collections.Concurrent;
using LmpClient.Base;
using LmpClient.Base.Interface;
using LmpCommon.Message.Data.Companies;
using LmpCommon.Message.Interface;
using LmpCommon.Message.Types;

namespace LmpClient.Systems.Companies
{
    public class CompanyMessageHandler : SubSystem<CompanySystem>, IMessageHandler
    {
        public ConcurrentQueue<IServerMessageBase> IncomingMessages { get; set; } = new ConcurrentQueue<IServerMessageBase>();

        public void HandleMessage(IServerMessageBase msg)
        {
            if (!(msg.Data is CompanyBaseMsgData msgData)) return;

            switch (msgData.GroupMessageType)
            {
                case GroupMessageType.ListResponse:
                    {
                        var data = (CompanyListResponseMsgData)msgData;
                        for (var i = 0; i < data.CompaniesCount; i++)
                        {
                            System.Companies.TryAdd(data.Companies[i].Name, data.Companies[i]);
                        }
                        break;
                    }
                case GroupMessageType.RemoveGroup:
                    {
                        var data = (CompanyRemoveMsgData)msgData;
                        System.Companies.TryRemove(data.CompanyName, out _);
                        break;
                    }
                case GroupMessageType.GroupUpdate:
                    {
                        var data = (CompanyUpdateMsgData)msgData;
                        System.Companies.AddOrUpdate(data.Company.Name, data.Company, (key, existingVal) => data.Company);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
