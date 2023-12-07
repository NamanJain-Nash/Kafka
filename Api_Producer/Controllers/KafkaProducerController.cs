using Microsoft.AspNetCore.Mvc;

namespace Api_Producer.Controllers;

[ApiController]
[Route("[controller]")]
public class KafkaProducerController : ControllerBase
{

    private readonly ILogger<KafkaProducerController> _logger;

    public KafkaProducerController(ILogger<KafkaProducerController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name ="KafkaProducer")]
    public async Task<string> Post()
    {
        try
        {
            
            return "Success";
        }
        catch(Exception ex)
        {
            return $"Error :{ex.Message}";
        } 
    }
}