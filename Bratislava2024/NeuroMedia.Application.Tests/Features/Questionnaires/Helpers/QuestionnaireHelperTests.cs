using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Tests.Features.Questionnaires.Helpers
{
    public class QuestionnaireHelperTests
    {
        [Fact]
        public void ResultPathValidation_ValidPath_ReturnsTrue()
        {
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = DateTime.Now.ToFileTime();

            var validPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var result = QuestionnaireHelper.ResultPathValidation(validPath);

            Assert.True(result);
        }

        [Fact]
        public void ResultPathValidation_InvalidBlobFolder_ThrowsArgumentException()
        {
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = DateTime.Now.ToFileTime();

            var invalidPath = $"InvalidFolder/1/1/1/{questionnaireType}/{dateTime}.json";

            var exception = Assert.Throws<ArgumentException>(() => QuestionnaireHelper.ResultPathValidation(invalidPath));
            Assert.Equal("Wrong folder", exception.Message);
        }

        [Fact]
        public void ResultPathValidation_InvalidPatientId_ThrowsArgumentException()
        {
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = DateTime.Now.ToFileTime();

            var invalidPath = $"Results/0/1/1/{questionnaireType}/{dateTime}.json";

            var exception = Assert.Throws<ArgumentException>(() => QuestionnaireHelper.ResultPathValidation(invalidPath));
            Assert.Equal("Invalid patient ID", exception.Message);
        }

        [Fact]
        public void ResultPathValidation_InvalidVisitId_ThrowsArgumentException()
        {
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = DateTime.Now.ToFileTime();

            var invalidPath = $"Results/1/0/1/{questionnaireType}/{dateTime}.json";

            var exception = Assert.Throws<ArgumentException>(() => QuestionnaireHelper.ResultPathValidation(invalidPath));
            Assert.Equal("Invalid visit ID", exception.Message);
        }

        [Fact]
        public void ResultPathValidation_InvalidQuestionnaireId_ThrowsArgumentException()
        {
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = DateTime.Now.ToFileTime();

            var invalidPath = $"Results/1/1/0/{questionnaireType}/{dateTime}.json";

            var exception = Assert.Throws<ArgumentException>(() => QuestionnaireHelper.ResultPathValidation(invalidPath));
            Assert.Equal("Invalid questionnaire ID", exception.Message);
        }

        [Fact]
        public void ResultPathValidation_InvalidQuestionnaireType_ThrowsArgumentException()
        {
            var questionnaireType = "CGIAdmin";
            var dateTime = DateTime.Now.ToFileTime();

            var invalidPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var exception = Assert.Throws<ArgumentException>(() => QuestionnaireHelper.ResultPathValidation(invalidPath));
            Assert.Equal("Invalid questionnaire type", exception.Message);
        }

        [Fact]
        public void ResultPathValidation_InvalidFileName_ThrowsArgumentException()
        {
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = "File Name";

            var invalidPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.json";

            var exception = Assert.Throws<ArgumentException>(() => QuestionnaireHelper.ResultPathValidation(invalidPath));
            Assert.Equal("Invalid file name", exception.Message);
        }

        [Fact]
        public void ResultPathValidation_InvalidFileExtension_ThrowsArgumentException()
        {
            var questionnaireType = QuestionnaireType.CGIDoctor;
            var dateTime = DateTime.Now.ToFileTime();

            var invalidPath = $"Results/1/1/1/{questionnaireType}/{dateTime}.txt";

            var exception = Assert.Throws<ArgumentException>(() => QuestionnaireHelper.ResultPathValidation(invalidPath));
            Assert.Equal("Invalid file extension", exception.Message);
        }
    }
}
