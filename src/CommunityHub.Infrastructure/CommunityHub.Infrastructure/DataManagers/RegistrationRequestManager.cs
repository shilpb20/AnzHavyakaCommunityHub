using CommunityHub.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CommunityHub.Infrastructure.DataManagers
{
    public class RegistrationRequestManager : IRegistrationRequestManager
    {
        private readonly ILogger<RegistrationRequest> _logger;

        public RegistrationRequestManager(ILogger<RegistrationRequest> logger)
        {
            _logger = logger;
        }

        public RegistrationRequest CreateRegistrationRequest(RegistrationInfo registrationInfo)
        {
            var registrationData = JsonConvert.DeserializeObject<RegistrationInfo>(registrationInfo);
            return CreateRegistrationRequest(registrationData);
        }

        public RegistrationRequest CreateRegistrationRequest(string registrationInfo)
        {
            var registrationRequest = new RegistrationRequest(_logger)
            {
                RegistrationInfo = registrationInfo
            };

            return registrationRequest;
        }

        public void ApproveRequest(RegistrationRequest request)
        {
            request.SetStatusToApproved();
        }

        public void RejectRequest(RegistrationRequest request)
        {
            request.SetStatusToRejected();
        }
    }
}
