using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using BuisnessLayer.IRepository;
using DatabaseLayer.Data;
using System.Text.RegularExpressions;
using Services.IServices;

namespace BuisnessLayer.Repository
{
    public class ConsumerLogic: IConsumerLogic
    {
        private readonly ISendMail _sendMailService;
        public ConsumerLogic(ISendMail sendMailService) {
            _sendMailService = sendMailService;
        
        }

        public async Task<string> SendMail(string message)
        {
            Producer sendRequest = JsonConvert.DeserializeObject<Producer>(message)??new Producer();
            var result = await _sendMailService.SendGraphAPI(sendRequest.to, sendRequest.subject, sendRequest.body);
            return result;
        }
    }
}
