using Microsoft.AspNetCore.Mvc;
using Sage50c.WebAPI.Models;
using Sage50c.WebAPI.Services;

namespace Sage50c.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly Sage50cApiService _sage50cService;
        private readonly ILogger<ApiController> _logger;

        public ApiController(Sage50cApiService sage50cService, ILogger<ApiController> logger)
        {
            _sage50cService = sage50cService;
            _logger = logger;
        }

        /// <summary>
        /// Lista códigos de produto disponíveis
        /// </summary>
        /// <returns>Lista de códigos válidos</returns>
        [HttpGet("products")]
        public ActionResult<ApiResponseDto<object>> GetAvailableProducts()
        {
            try
            {
                var products = new[]
                {
                    "Sage 50c",
                    "CRTL",
                    "CGCO",
                    "Sage50c",
                    "50c"
                };

                return Ok(new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Códigos de produto disponíveis",
                    Data = products
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = $"Erro: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Inicializa a API Sage50c
        /// </summary>
        /// <param name="request">Dados de inicialização</param>
        /// <returns>Status da inicialização</returns>
        [HttpPost("initialize")]
        public ActionResult<ApiResponseDto<object>> InitializeApi([FromBody] InitializeApiRequest request)
        {
            try
            {
                if (_sage50cService.IsInitialized)
                {
                    return Ok(new ApiResponseDto<object>
                    {
                        Success = true,
                        Message = "API já foi inicializada",
                        Data = new { Status = "Already initialized" }
                    });
                }

                var success = _sage50cService.Initialize(request.Api, request.CompanyId, request.DebugMode);
                
                if (success)
                {
                    return Ok(new ApiResponseDto<object>
                    {
                        Success = true,
                        Message = "API inicializada com sucesso",
                        Data = new 
                        { 
                            Status = "Initialized",
                            CompanyId = request.CompanyId,
                            Api = request.Api,
                            DebugMode = request.DebugMode
                        }
                    });
                }
                else
                {
                    return StatusCode(500, new ApiResponseDto<object>
                    {
                        Success = false,
                        Message = "Falha ao inicializar API"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inicializar API Sage50c");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = $"Erro ao inicializar API: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Verifica o status da API Sage50c
        /// </summary>
        /// <returns>Status atual da API</returns>
        [HttpGet("status")]
        public ActionResult<ApiResponseDto<object>> GetApiStatus()
        {
            try
            {
                var status = new
                {
                    IsInitialized = _sage50cService.IsInitialized,
                    Timestamp = DateTime.Now,
                    CompanyName = _sage50cService.IsInitialized ? _sage50cService.SystemSettings?.Company?.Name ?? "N/A" : "N/A",
                    CompanyId = _sage50cService.IsInitialized ? _sage50cService.SystemSettings?.Company?.CompanyID ?? "N/A" : "N/A",
                    UserName = _sage50cService.IsInitialized ? _sage50cService.SystemSettings?.User?.Name ?? "N/A" : "N/A"
                };

                return Ok(new ApiResponseDto<object>
                {
                    Success = true,
                    Message = _sage50cService.IsInitialized ? "API ativa" : "API não inicializada",
                    Data = status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar status da API");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = $"Erro ao verificar status: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Termina a API Sage50c
        /// </summary>
        /// <returns>Status da terminação</returns>
        [HttpPost("terminate")]
        public ActionResult<ApiResponseDto<object>> TerminateApi()
        {
            try
            {
                if (!_sage50cService.IsInitialized)
                {
                    return Ok(new ApiResponseDto<object>
                    {
                        Success = true,
                        Message = "API já estava terminada",
                        Data = new { Status = "Already terminated" }
                    });
                }

                _sage50cService.Terminate();

                return Ok(new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "API terminada com sucesso",
                    Data = new { Status = "Terminated", Timestamp = DateTime.Now }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao terminar API Sage50c");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = $"Erro ao terminar API: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Obtém informações da empresa
        /// </summary>
        /// <returns>Informações da empresa</returns>
        [HttpGet("company")]
        public ActionResult<ApiResponseDto<object>> GetCompanyInfo()
        {
            try
            {
                if (!_sage50cService.IsInitialized)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Success = false,
                        Message = "API não inicializada"
                    });
                }

                var company = _sage50cService.SystemSettings!.Company;
                var companyInfo = new
                {
                    CompanyID = company.CompanyID,
                    CompanyName = company.Name,
                    Address = company.Address,
                    Telephone = company.Telephone,
                    Fax = company.Fax,
                    Email = company.Email
                };

                return Ok(new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Informações da empresa obtidas com sucesso",
                    Data = companyInfo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter informações da empresa");
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Success = false,
                    Message = $"Erro ao obter informações da empresa: {ex.Message}"
                });
            }
        }
    }

    public class InitializeApiRequest
    {
        public string Api { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public bool DebugMode { get; set; } = false;
    }
}