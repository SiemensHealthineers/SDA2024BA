namespace NeuroMedia.Application.Features.Questionnaires.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public string Type { get; set; } = default!; // Possible values: Radio,DropDown
        public IEnumerable<OptionDto> Options { get; set; } = [];
    }
}
