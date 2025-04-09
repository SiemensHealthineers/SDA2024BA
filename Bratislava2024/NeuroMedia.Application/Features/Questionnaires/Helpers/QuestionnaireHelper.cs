using System.Security.Claims;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Features.Questionnaires.Helpers
{
    public static class QuestionnaireHelper
    {
        #region Constants

        public const string QuestionnaireFileExt = ".json";
        public const string QuestionnaireFolder = "Questionnaires";
        public const string QuestionnaireResultFolder = "Results";

        #endregion

        #region Public methods

        public static Questionnaire AutoUpdateEntries(Questionnaire questionnaire, ClaimsPrincipal claims)
        {
            BaseAuditableEntityHelper.UpdateAuditableEntries(questionnaire, claims);

            return questionnaire;
        }

        //Example path: Questionnaires/[QuestionnaireType].json
        public static string GetQuestionnairePath(QuestionnaireType questionnaireType)
        {
            return Path.Combine(QuestionnaireFolder, $"{questionnaireType}{QuestionnaireFileExt}")
                .Replace("\\", "/");
        }

        public static QuestionnaireType? GetQuestionnaireType(string path)
        {
            var pathParts = SplitPath(path, 2);

            var type = pathParts[1].Replace(QuestionnaireFileExt, string.Empty);

            if (Enum.TryParse<QuestionnaireType>(type, true, out var questionnaireType))
            {
                return questionnaireType;
            }

            return null;
        }

        public static string GetBlobFolder(string path)
        {
            return SplitPath(path)[0];
        }

        public static int GetPatientId(string path)
        {
            return GetId(path, 1);
        }

        public static int GetVisitId(string path)
        {
            return GetId(path, 2);
        }

        public static int GetQuestionnaireId(string path)
        {
            return GetId(path, 3);
        }

        public static QuestionnaireType? GetResultQuestionnaireType(string path)
        {
            var type = SplitPath(path)[4];

            if (Enum.IsDefined(typeof(QuestionnaireType), type))
            {
                if (Enum.TryParse<QuestionnaireType>(type, false, out var questionnaireType))
                {
                    return questionnaireType;
                }
            }

            return null;
        }

        public static string GetFileName(string path)
        {
            return SplitPath(path)[5];
        }

        public static DateTime GetFileDateTime(string path)
        {
            var fileNameWithoutExtension = GetFileName(path).Replace(QuestionnaireFileExt, string.Empty);

            if (!long.TryParse(fileNameWithoutExtension, out var fileTime))
            {
                throw new ArgumentException("Invalid file name");
            }

            return DateTime.FromFileTime(fileTime);
        }

        //Example path: Results/[PatientId]/[VisitId]/[QuestionnaireId]/[Type]/[Name] ([Name]=[FileDateTime].json)
        public static string GetResultPath(int patientId, int visitId, int questionnaireId, QuestionnaireType questionnaireType, string fileName)
        {
            return Path.Combine(QuestionnaireResultFolder, $"{patientId}", $"{visitId}", $"{questionnaireId}", $"{questionnaireType}", fileName)
                .Replace("\\", "/");
        }

        public static bool ResultPathValidation(string path)
        {
            var blobFolder = GetBlobFolder(path);

            if (blobFolder != "Results")
            {
                throw new ArgumentException("Wrong folder");
            }

            var patientId = GetPatientId(path);

            if (patientId <= 0)
            {
                throw new ArgumentException("Invalid patient ID");
            }

            var visitId = GetVisitId(path);

            if (visitId <= 0)
            {
                throw new ArgumentException("Invalid visit ID");
            }

            var questionnaireId = GetQuestionnaireId(path);

            if (questionnaireId <= 0)
            {
                throw new ArgumentException("Invalid questionnaire ID");
            }

            var questionnaireType = GetResultQuestionnaireType(path);

            if (questionnaireType == null)
            {
                throw new ArgumentException("Invalid questionnaire type");
            }

            var fileName = GetFileName(path);

            if (!fileName.Contains(QuestionnaireFileExt))
            {
                throw new ArgumentException("Invalid file extension");
            }

            GetFileDateTime(path);

            return true;
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
