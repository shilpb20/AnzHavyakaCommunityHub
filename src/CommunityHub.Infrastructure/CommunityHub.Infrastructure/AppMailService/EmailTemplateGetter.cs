using System;
using System.IO;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.AppMailService
{
    public static class EmailTemplateGetter
    {
        public static async Task<string> GetTemplateAsync(string dirPath, string templateFileName)
        {
            try
            {
                var headerTemplate = await File.ReadAllTextAsync(Path.Combine(dirPath, TemplateNames.Header));
                var footerTemplate = await File.ReadAllTextAsync(Path.Combine(dirPath, TemplateNames.Footer));
                var emailTemplate = await File.ReadAllTextAsync(Path.Combine(dirPath, templateFileName));

                //emailTemplate = emailTemplate.Replace("{{header}}", headerTemplate);
                emailTemplate = emailTemplate.Replace("{{footer}}", footerTemplate);
                emailTemplate = emailTemplate.Replace("{{logo}}", "cid:logo");
                return emailTemplate;
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException("One or more template files were not found.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while processing the email template.", ex);
            }
        }

        private static async Task<string> GetBase64Image(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }
    }
}
