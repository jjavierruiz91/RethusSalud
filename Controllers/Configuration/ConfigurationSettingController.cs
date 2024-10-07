using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rethus_backend.Controllers;
using rethus_backend.Models;
using rethus_backend.Models.Dto.Configuration;

[ApiController]
[Route("[controller]")]
public class ConfigurationSettingController : ApiBaseController
{
    public ConfigurationSettingController(IServiceProvider provider)
        : base(provider) { }

    [HttpGet("/consecutive")]
    [Authorize(Roles = Policies.Inventory)]
    public ApiResponse GetConfigurationConsecutive()
    {
        bool configuration = _unitOfWork.ConfigurationSetting.GetSettingBoolValue(
            "ActiveConsecutive"
        );

        ConsecutiveAutomaticGenerate a = new ConsecutiveAutomaticGenerate
        {
            ActiveConsecutive = false,
            ConsecutiveStart = "0",
            ConsecutiveEnd = "0",
            ConsecutiveDate = ""
        };

        if (configuration)
        {
            string consecutiveStart = _unitOfWork.ConfigurationSetting.GetSettingStringValue(
                "ConsecutiveStart"
            );

            string consecutiveEnd = _unitOfWork.ConfigurationSetting.GetSettingStringValue(
                "ConsecutiveEnd"
            );

            string ConsecutiveDate = _unitOfWork.ConfigurationSetting.GetSettingStringValue(
                "ConsecutiveDate "
            );

            a.ActiveConsecutive = true;
            a.ConsecutiveStart = consecutiveStart ?? "0";
            a.ConsecutiveEnd = consecutiveEnd ?? "0";
            a.ConsecutiveDate = ConsecutiveDate ?? "";
        }

        _response.Result = a;
        return _response;
    }

    [HttpPatch]
    [Authorize(Roles = Policies.Inventory)]
    public ActionResult<ApiResponse> PatchConfigurationKeys([FromBody] List<ConfigDto> configs)
    {
        foreach (var config in configs)
        {
            switch (config.Key)
            {
                case ConfigKeys.ActiveConsecutive:
                    if (bool.TryParse(config.Value, out bool activeConsecutive))
                    {
                        _unitOfWork.ConfigurationSetting.SaveSettingBoolValue(
                            config.Key.ToString(),
                            activeConsecutive
                        );
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Messages.Add("Solo se acepta campos de tipo bool");
                        return BadRequest(_response);
                    }
                    break;

                case ConfigKeys.ConsecutiveStart:
                case ConfigKeys.ConsecutiveEnd:
                    if (!string.IsNullOrWhiteSpace(config.Value))
                    {
                        _unitOfWork.ConfigurationSetting.SaveSettingStringValue(
                            config.Key.ToString(),
                            config.Value
                        );
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Messages.Add("Solo se acepta campos de tipo string");

                        return BadRequest(_response);
                    }
                    break;
                case ConfigKeys.ConsecutiveDate:
                    if (DateTime.TryParse(config.Value, out DateTime consecutiveDate))
                    {
                        // Convertir la fecha al formato dd/MM/yyyy
                        string formattedDate = consecutiveDate.ToString("dd/MM/yyyy");

                        _unitOfWork.ConfigurationSetting.SaveSettingStringValue(
                            config.Key.ToString(),
                            formattedDate
                        );
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Messages.Add("Solo se acepta campo de tipo DateTime");

                        return BadRequest(_response);
                    }
                    break;
                default:
                    _response.IsSuccess = false;
                    _response.Messages.Add("Ocurrio un error desconocido!");

                    return BadRequest(_response);
            }
        }
        _response.Messages.Add("Configurationes actulizadas");
        return _response;
    }
}
