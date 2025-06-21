using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace kDriveClient.Models.Exceptions
{
    /// <summary>
    /// KdriveErrorDetail represents detailed information about an error returned by the kDrive API.
    /// </summary>
    public class KDriveErrorDetail : ApiResultBase
    {
        /// <summary>
        /// Code represents the error code returned by the kDrive API.
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Description provides a human-readable description of the error.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ToString provides a string representation of the KDriveErrorDetail.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Code: {Code}, Description: {Description}";
        }
    }
}
