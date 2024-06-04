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
            templateContent = templateContent.Replace("${RED}", dto.Title);
            templateContent = templateContent.Replace("${RED_DATE}", dto.Content);
            return templateContent;
        }

        public static string ReplacePlaceholdersSSO(string templateContent, TemplateSSODto dto)
        {
            templateContent = templateContent.Replace("${RED}", dto.Title);
            templateContent = templateContent.Replace("${RED_DATE}", dto.Content);
            return templateContent;
        }
    }
}
