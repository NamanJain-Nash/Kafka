using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{

        public interface IKafkaConsumer : IDisposable
        {
            event EventHandler<string> MessageReceived;
            event EventHandler<string> ErrorOccurred;

            void StartConsumer(string topicName);
            void StopConsumer();
        }
    
}
