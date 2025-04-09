using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Videos.Helpers
{
    public static class VideoHelper
    {
        #region Constants

        public const string VideoFileExt = ".mp4";
        public const string VideoResultFolder = "Videos";

        #endregion

        #region Public methods

        public static Video AutoUpdateEntries(Video video, ClaimsPrincipal claims)
        {
            BaseAuditableEntityHelper.UpdateAuditableEntries(video, claims);

            return video;
        }

        //Example path: Results/[PatientId]/[VisitId]/[VideoId]/[Type]/[Name] ([Name]=[FileDateTime].json)
        public static string GetVideoFilePath(int patientId, int visitId, int videoId, VideoType videoType, string fileName)
        {
            return Path.Combine(VideoResultFolder, $"{patientId}", $"{visitId}", $"{videoId}", $"{videoType}", fileName)
                .Replace("\\", "/");
        }

        public static VideoType? GetVideoType(string path)
        {
            var pathParts = SplitPath(path);

            var type = pathParts[4];

            if (Enum.TryParse<VideoType>(type, true, out var videoType))
            {
                return videoType;
            }

            return null;
        }

        public static int GetPatientId(string path)
        {
            return GetId(path, 1);
        }

        public static int GetVisitId(string path)
        {
            return GetId(path, 2);
        }

        public static int GetVideoId(string path)
        {
            return GetId(path, 3);
        }

        public static VideoType? GetResultVideoType(string path)
        {
            var type = SplitPath(path)[4];

            if (Enum.TryParse<VideoType>(type, true, out var videoType))
            {
                return videoType;
            }

            return null;
        }

        public static string GetFileName(string path)
        {
            return SplitPath(path)[5];
        }

        public static DateTime GetFileDateTime(string path)
        {
            var fileNameWithoutExtension = GetFileName(path).Replace(VideoFileExt, string.Empty);

            if (!long.TryParse(fileNameWithoutExtension, out var fileTime))
            {
                throw new ArgumentException("Wrong parsing");
            }

            return DateTime.FromFileTime(fileTime);
        }

        #endregion

        #region Private methods

        private static int GetId(string path, int index)
        {
            var array = SplitPath(path);

            var idStr = array[index];

            if (!int.TryParse(idStr, out var id))
            {
                throw new ArgumentException("Wrong parsing");
            }

            return id;
        }

        private static string[] SplitPath(string path, int pathPartsCount = 6)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Empty path");
            }

            var array = path.Split('/');

            if (array.Length != pathPartsCount)
            {
                throw new ArgumentException("Wrong '/' split length");
            }

            return array;
        }

        #endregion
    }
}
