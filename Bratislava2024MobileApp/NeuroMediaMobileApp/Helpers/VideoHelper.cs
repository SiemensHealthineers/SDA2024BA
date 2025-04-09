using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Helpers
{
    public static class VideoHelper
    {
        #region Constants

        public const string VideoFileExt = ".mp4";
        public const string VideoResultFolder = "Videos";

        #endregion

        #region Public methods

        //Example path: Results/[PatientId]/[VisitId]/[VideoId]/[Type]/[Name] ([Name]=[FileDateTime].json)
        public static string GetVideoFilePath(int patientId, int visitId, int videoId, VideoType videoType, string fileName)
        {
            return Path.Combine(VideoResultFolder, $"{patientId}", $"{visitId}", $"{videoId}", $"{videoType}", fileName)
                .Replace("\\", "/");
        }

        #endregion
    }
}
