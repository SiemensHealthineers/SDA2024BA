namespace NeuroMedia.Application.Features.Questionnaires.Dtos
{
    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string ResultValue { get; set; } = default!;
    }
}
