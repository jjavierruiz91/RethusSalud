using RethusSalud.Application.Dtos;
using RethusSalud.Domain.Entities;

namespace RethusSalud.Application.Interfaces;

public interface ICertificadoPdfService
{
    byte[] Generar(Solicitud solicitud, FirmantesDocumentoDto firmantes);
}
