namespace FileStorage.Domain.Entities
{
    /// <summary>
    /// Представляет файл в системе хранения.
    /// </summary>
    public class FileEntity
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string FileName { get; init; }
        public string StoredPath { get; init; }
        public string ContentType { get; init; }
        public long SizeBytes { get; init; }
        public DateTime UploadedAt { get; init; }
    }
}
