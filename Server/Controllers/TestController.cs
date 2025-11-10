using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController:ControllerBase
    {
        /// <summary>
        /// Простой тестовый эндпоинт для проверки работы API
        /// </summary>
        /// <param name="request">Данные от клиента</param>
        /// <returns>Ответ с приветствием</returns>
        [HttpPost("echo")]
        public IActionResult Echo([FromBody] TestRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Name))
                return BadRequest("Имя обязательно");

            var response = new
            {
                Greeting = $"Привет, {request.Name}!",
                YourMessage = request.Message ?? "(сообщение не указано)",
                Timestamp = DateTime.UtcNow
            };

            return Ok(response);
        }
    }
}
