using DatabaseLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.IRepository
{
    public interface IProducerLogic
    {
        public Task<string> AddProducerAsync(ProducerDTO producerDTO, string Topic);
    }
}
