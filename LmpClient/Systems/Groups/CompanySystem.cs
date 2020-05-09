using LmpClient.Base;
using LmpClient.Network;
using LmpClient.Systems.SettingsSys;
using LmpCommon.Message.Data.Companies;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LmpClient.Systems.Companies
{
    public class CompanySystem : MessageSystem<CompanySystem, CompanyMessageSender, CompanyMessageHandler>
    {
        public ConcurrentDictionary<string, Company> Companies { get; } = new ConcurrentDictionary<string, Company>();

        public override string SystemName { get; } = nameof(CompanySystem);

        protected override bool ProcessMessagesInUnityThread => false;

        protected override void OnDisabled()
        {
            base.OnDisabled();
            Companies.Clear();
        }

        public void JoinCompany(string companyName)
        {
            if (Companies.TryGetValue(companyName, out var existingVal))
            {
                if (existingVal.Members.All(m => m != SettingsSystem.CurrentSettings.PlayerName) &&
                    existingVal.Invited.All(m => m != SettingsSystem.CurrentSettings.PlayerName))
                {
                    var expectedCompany = existingVal.Clone();

                    var newInvited = new List<string>(expectedCompany.Invited) { SettingsSystem.CurrentSettings.PlayerName };
                    expectedCompany.Invited = newInvited.ToArray();

                    var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<CompanyUpdateMsgData>();
                    msgData.Company = expectedCompany;

                    MessageSender.SendMessage(msgData);
                }
            }
        }

        public void CreateCompany(string companyName)
        {
            if (!Companies.ContainsKey(companyName))
            {
                var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<CompanyCreateMsgData>();
                msgData.CompanyName = companyName;

                MessageSender.SendMessage(msgData);
            }
        }

        public void RemoveCompany(string companyName)
        {
            if (Companies.TryGetValue(companyName, out var existingVal) && existingVal.Owner == SettingsSystem.CurrentSettings.PlayerName)
            {
                var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<CompanyRemoveMsgData>();
                msgData.CompanyName = companyName;

                MessageSender.SendMessage(msgData);
            }
        }

        public void AddMember(string companyName, string username)
        {
            if (Companies.TryGetValue(companyName, out var existingVal)
                && existingVal.Owner == SettingsSystem.CurrentSettings.PlayerName)
            {
                //TODO: remove this clone and do as with flags to avoid garbage
                var expectedCompany = existingVal.Clone();

                var newMembers = new List<string>(expectedCompany.Members) { username };
                expectedCompany.Members = newMembers.ToArray();

                var newInvited = new List<string>(expectedCompany.Invited.Except(new[] { username }));
                expectedCompany.Invited = newInvited.ToArray();

                var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<CompanyUpdateMsgData>();
                msgData.Company = expectedCompany;

                MessageSender.SendMessage(msgData);
            }
        }

        public void RemoveMember(string companyName, string username)
        {
            if (Companies.TryGetValue(companyName, out var existingVal)
                && existingVal.Owner == SettingsSystem.CurrentSettings.PlayerName)
            {
                //TODO: remove this clone and do as with flags to avoid garbage
                var expectedCompany = existingVal.Clone();

                var newMembers = new List<string>(expectedCompany.Members.Except(new[] { username })) { username };
                expectedCompany.Members = newMembers.ToArray();

                var newInvited = new List<string>(expectedCompany.Invited.Except(new[] { username }));
                expectedCompany.Invited = newInvited.ToArray();

                var msgData = NetworkMain.CliMsgFactory.CreateNewMessageData<CompanyUpdateMsgData>();
                msgData.Company = expectedCompany;

                MessageSender.SendMessage(msgData);
            }
        }
    }
}
