using rethus_backend.Utilities.Templates.dto;

namespace rethus_backend.Utilities.Templates
{
    public class TemplateHelper
    {
        public static string ReplacePlaceholdersRethus(
            string templateContent,
            TemplateRethusDto dto
        )
        {
            templateContent = templateContent.Replace("${CONSECUTIVO}", dto.CONSECUTIVO);
            templateContent = templateContent.Replace(
                "${CONSECUTIVO_FECHA}",
                dto.CONSECUTIVO_FECHA
            );
            templateContent = templateContent.Replace(
                "${NOMBRE_PROFESIONAL}",
                dto.NOMBRE_PROFESIONAL
            );
            templateContent = templateContent.Replace(
                "${CEDULA_PROFESIONAL}",
                dto.CEDULA_PROFESIONAL
            );
            templateContent = templateContent.Replace(
                "${TYPE_IDENTIFICATION}",
                dto.TYPE_IDENTIFICATION
            );
            templateContent = templateContent.Replace(
                "${EXPEDIDA_PROFESIONAL}",
                dto.EXPEDIDA_PROFESIONAL
            );
            templateContent = templateContent.Replace(
                "${PROFESION_PROFESIONAL}",
                dto.PROFESION_PROFESIONAL
            );
            templateContent = templateContent.Replace(
                "${UNIVERSIDAD_PROFESIONAL}",
                dto.UNIVERSIDAD_PROFESIONAL
            );
            templateContent = templateContent.Replace("${FIRMA_PRINCIPAL}", dto.FIRMA_PRINCIPAL);
            templateContent = templateContent.Replace("${NOMBRE_FIRMANTE}", dto.NOMBRE_FIRMANTE);
            templateContent = templateContent.Replace("${TIPO_TRABAJO}", dto.TIPO_TRABAJO);
            templateContent = templateContent.Replace("${FIRMA_PROYECTO}", dto.FIRMA_PROYECTO);
            templateContent = templateContent.Replace("${FIRMA_1}", dto.FIRMA_1);
            templateContent = templateContent.Replace("${FIRMA_2}", dto.FIRMA_2);
            templateContent = templateContent.Replace("${FIRMA_APROBO}", dto.FIRMA_APROBO);
            templateContent = templateContent.Replace("${FIRMA_REVISION}", dto.FIRMA_REVISION);
            templateContent = templateContent.Replace("${FIRMA_3}", dto.FIRMA_3);
            return templateContent;
        }

        public static string ReplacePlaceholdersSSO(string templateContent, TemplateSSODto dto)
        {
            templateContent = templateContent.Replace("${CODIGO_PLAZA}", dto.CODIGO_PLAZA);
            templateContent = templateContent.Replace("${MODALIDAD}", dto.MODALIDAD);
            templateContent = templateContent.Replace(
                "${NOMBRE_INSTITUCION}",
                dto.NOMBRE_INSTITUCION
            );
            templateContent = templateContent.Replace("${UBICACION_PLAZA}", dto.UBICACION_PLAZA);
            templateContent = templateContent.Replace("${FECHA_INICION}", dto.FECHA_INICION);
            templateContent = templateContent.Replace(
                "${FECHA_TERMINACION}",
                dto.FECHA_TERMINACION
            );
            templateContent = templateContent.Replace("${DIAS}", dto.DIAS);
            templateContent = templateContent.Replace("${MES}", dto.MES);
            templateContent = templateContent.Replace("${ANO}", dto.ANO);
            templateContent = templateContent.Replace("${FIRMA_SECRETARIO}", dto.FIRMA_SECRETARIO);
            templateContent = templateContent.Replace("${FIRMA_PROYECTO}", dto.FIRMA_PROYECTO);
            templateContent = templateContent.Replace("${FIRMA_1}", dto.FIRMA_1);
            templateContent = templateContent.Replace("${FIRMA_APROBO}", dto.FIRMA_APROBO);
            templateContent = templateContent.Replace("${FIRMA_3}", dto.FIRMA_3);

            return templateContent;
        }
    }
}
