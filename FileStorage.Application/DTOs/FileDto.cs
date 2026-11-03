namespace FileStorage.Application.DTOs
{
    /// <summary>
    /// DTO для представления информации о файле.
    /// </summary>
    public class FileDto
    {
        public Guid Id { get; init; }
        public string FileName { get; init; }
        public string ContentType { get; init; }
        public long SizeBytes { get; init; }
        public DateTime UploadedAt { get; init; }
    }
}
