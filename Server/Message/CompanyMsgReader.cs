using System.Linq;
using LmpCommon.Message.Data.Companies;
using LmpCommon.Message.Interface;
using LmpCommon.Message.Server;
using LmpCommon.Message.Types;
using Server.Client;
using Server.Context;
using Server.Message.Base;
using Server.Server;
using Server.System;

namespace Server.Message
{
    public class CompanyMsgReader : ReaderBase
    {
        public override void HandleMessage(ClientStructure client, IClientMessageBase message)
        {
            var data = (CompanyBaseMsgData)message.Data;
            switch (data.GroupMessageType)
            {
                case GroupMessageType.ListRequest:

                    var msgData = ServerContext.ServerMessageFactory.CreateNewMessageData<CompanyListResponseMsgData>();
                    msgData.Companies = CompanySystem.Companies.Values.ToArray();
                    msgData.CompaniesCount = msgData.Companies.Length;
                    MessageQueuer.SendToClient<CompanySrvMsg>(client, msgData);

                    //We don't use this message anymore so we can recycle it
                    message.Recycle();
                    break;
                case GroupMessageType.CreateGroup:
                    CompanySystem.CreateCompany(client.PlayerName, ((CompanyCreateMsgData)data).CompanyName);
                    break;
                case GroupMessageType.RemoveGroup:
                    CompanySystem.RemoveCompany(client.PlayerName, ((CompanyRemoveMsgData)data).CompanyName);
                    break;
                case GroupMessageType.GroupUpdate:
                    CompanySystem.UpdateCompany(client.PlayerName, ((CompanyUpdateMsgData)data).Company);
                    break;
            }
        }
    }
}
