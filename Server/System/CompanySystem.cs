using LmpCommon;
using LmpCommon.Message.Data.Companies;
using LmpCommon.Message.Server;
using LmpCommon.Xml;
using Server.Context;
using Server.Server;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.System
{
    public static class CompanySystem
    {
        /// <summary>
        /// The serializer is not thread safe so we need a lock
        /// </summary>
        private static readonly object FileLock = new object();

        public static readonly string CompaniesPath = Path.Combine(ServerContext.UniverseDirectory, "Companies");
        private static readonly string CompaniesFilePath = Path.Combine(CompaniesPath, "Companies.xml");

        public static ConcurrentDictionary<string, Company> Companies { get; } = new ConcurrentDictionary<string, Company>();

        public static void CreateCompany(string clientPlayerName, string companyName)
        {
            if (!Companies.ContainsKey(companyName))
            {
                var newCompany = new Company
                {
                    Members = new[] { clientPlayerName },
                    MembersCount = 1,
                    Owner = clientPlayerName,
                    Name = companyName
                };
                if (Companies.TryAdd(companyName, newCompany))
                {
                    var msgData = ServerContext.ServerMessageFactory.CreateNewMessageData<CompanyUpdateMsgData>();
                    msgData.Company = newCompany;

                    MessageQueuer.SendToAllClients<CompanySrvMsg>(msgData);
                    Task.Run(() => SaveCompanies());
                }
            }
        }

        public static void RemoveCompany(string clientPlayerName, string companyName)
        {
            if (Companies.TryGetValue(companyName, out var exitingCompany) && exitingCompany.Owner == clientPlayerName
                && Companies.TryRemove(companyName, out _))
            {
                var msgData = ServerContext.ServerMessageFactory.CreateNewMessageData<CompanyRemoveMsgData>();
                msgData.CompanyName = companyName;

                MessageQueuer.SendToAllClients<CompanySrvMsg>(msgData);
                Task.Run(() => SaveCompanies());
            }
        }

        public static void UpdateCompany(string clientPlayerName, Company company)
        {
            if (Companies.TryGetValue(company.Name, out var existingCompany))
            {
                if (existingCompany.Owner == clientPlayerName)
                {
                    //We are the owner of the company so we can do whatever we want
                    if (Companies.TryUpdate(company.Name, company, existingCompany))
                    {
                        var msgData = ServerContext.ServerMessageFactory.CreateNewMessageData<CompanyUpdateMsgData>();
                        msgData.Company = company;

                        MessageQueuer.SendToAllClients<CompanySrvMsg>(msgData);
                        Task.Run(() => SaveCompanies());
                    }
                }
                else
                {
                    //We are not the owner of the company so the only thing we can do is to add ourself to the "invited" hashset
                    if (company.Owner == existingCompany.Owner && Common.ScrambledEquals(company.Members, existingCompany.Members))
                    {
                        var invited = company.Invited.Except(existingCompany.Invited).ToArray();
                        if (invited.Length == 1 && invited[0] == clientPlayerName && Companies.TryUpdate(company.Name, company, existingCompany))
                        {
                            var msgData = ServerContext.ServerMessageFactory.CreateNewMessageData<CompanyUpdateMsgData>();
                            msgData.Company = company;

                            MessageQueuer.SendToAllClients<CompanySrvMsg>(msgData);
                            Task.Run(() => SaveCompanies());
                        }
                    }
                }
            }
        }

        public static void SaveCompanies()
        {
            lock (FileLock)
            {
                if (FileHandler.FolderExists(CompaniesPath))
                    LunaXmlSerializer.WriteToXmlFile(Companies.Values.ToList(), CompaniesFilePath);
            }
        }

        public static void LoadCompanies()
        {
            lock (FileLock)
            {
                if (File.Exists(CompaniesFilePath))
                {
                    var values = LunaXmlSerializer.ReadXmlFromPath<List<Company>>(CompaniesFilePath);
                    foreach (var value in values)
                    {
                        Companies.TryAdd(value.Name, value);
                    }
                }
            }
        }
    }
}
