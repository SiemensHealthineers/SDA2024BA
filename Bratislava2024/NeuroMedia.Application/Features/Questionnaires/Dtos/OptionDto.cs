namespace NeuroMedia.Application.Features.Questionnaires.Dtos
{
    public class OptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
