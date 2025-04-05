using CommunityHub.Core.Dtos;
using CommunityHub.Core.Enums;
using CommunityHub.Core.Extensions;
using CommunityHub.Core.Helpers;
using CommunityHub.Core.Messages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityHub.Infrastructure.Models
{
    public class RegistrationRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string RegistrationInfo { get; set; }
        public string RegistrationStatus { get; private set; }
        public string? Review { get; set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? ReviewedAt { get; private set; }

        public RegistrationRequest(string registrationInfo)
        {
            ValidationHelper.ValidateNullOrEmptyString(registrationInfo, nameof(registrationInfo));
            RegistrationInfo = registrationInfo;
            InitializeData();
        }

        private void InitializeData()
        {
            CreatedAt = DateTime.UtcNow;
            ReviewedAt = null;
            RegistrationStatus = RegistrationStatusHelper.PendingStatus;
        }

        public void Approve()
        {
            ValidateStateTransition();

            RegistrationStatus = RegistrationStatusHelper.ApprovedStatus;
            ReviewedAt = DateTime.UtcNow;
        }

        public void Reject(string comment)
        {
            ValidationHelper.ValidateNullOrEmptyString(comment, "Rejection reason");

            ValidateStateTransition();

            RegistrationStatus = RegistrationStatusHelper.RejectedStatus;
            Review = comment;
            ReviewedAt = DateTime.UtcNow;
        }

        private void ValidateStateTransition()
        {
            if (RegistrationStatus != RegistrationStatusHelper.PendingStatus)
            {
                throw new InvalidOperationException(string.Format(ErrorMessage.InvalidStateTransition, RegistrationStatus));
            }
        }
    }
}