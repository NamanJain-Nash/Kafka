using BuisnessLayer.IRepository;
using DatabaseLayer.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace Api_Producer.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaProducerController : ControllerBase
{

    private readonly ILogger<KafkaProducerController> _logger;
    private readonly IProducerLogic _producerLogic;
    public KafkaProducerController(ILogger<KafkaProducerController> logger,IProducerLogic producerLogic)
    {
        _producerLogic = producerLogic;
        _logger = logger;
    }

    [HttpPost(Name ="KafkaProducer")]
    public async Task<string> Post([FromBody] ProducerInput input)
    {
        try
        {
            Producer data=input.Data;
            string response= await _producerLogic.AddProducerAsync(input.Data,input.Topic);
            return "Success";
        }
        catch(Exception ex)
        {
            return $"Error :{ex.Message}";
        } 
    }
}
public class ProducerInput
{    
    public Producer Data { get; set; }
    public string Topic { get; set; }
}

