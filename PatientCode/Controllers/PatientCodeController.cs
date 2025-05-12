using Microsoft.AspNetCore.Mvc;
using PatientCode.DTOs;
using PatientCode.Models;
using Microsoft.Extensions.Logging;


namespace PatientCode.Controllers


{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientCodeController : ControllerBase
    {
        private readonly ILogger<PatientCodeController> _logger;

        public PatientCodeController(ILogger<PatientCodeController> logger)
        {
            _logger = logger;
        }

        [HttpPost("generate")]
        public ActionResult<Patient> GeneratePatientCode([FromBody] PatientRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name) ||
                    string.IsNullOrWhiteSpace(request.LastName) ||
                    string.IsNullOrWhiteSpace(request.CI))
                {
                    _logger.LogWarning("Datos incompletos para generar código de paciente.");
                    return BadRequest("Name, LastName y CI son obligatorios.");
                }

                var code = $"{request.Name[0]}{request.LastName[0]}-{request.CI}";
                var patient = new Patient
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    CI = request.CI,
                    BloodType = request.BloodType,
                    PatientCode = code.ToUpper()
                };

                _logger.LogInformation("Código generado correctamente para el paciente: {Code}", patient.PatientCode);
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar código de paciente.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}

