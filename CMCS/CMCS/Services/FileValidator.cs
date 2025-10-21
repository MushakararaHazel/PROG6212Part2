namespace CMCS.Services
{
    public class FileValidator : IFileValidator
    {
        private readonly long _maxSize;
        private readonly string[] _allowedExts;
        private readonly int _maxFiles;

        public FileValidator(IConfiguration cfg)
        {
            _maxSize = cfg.GetValue<long>("Upload:MaxFileSizeBytes");
            _allowedExts = cfg.GetSection("Upload:AllowedExtensions").Get<string[]>() ?? Array.Empty<string>();
            _maxFiles = cfg.GetValue<int>("Upload:MaxFilesPerClaim");
        }

        public void EnsureValid(IEnumerable<IFormFile> files, int existingCount = 0)
        {
            var list = files?.ToList() ?? new();
            if (existingCount + list.Count > _maxFiles)
                throw new InvalidOperationException($"Max {_maxFiles} files per claim.");

            foreach (var f in list)
            {
                if (f.Length == 0) continue;
                if (f.Length > _maxSize)
                    throw new InvalidOperationException($"File {f.FileName} exceeds size limit.");

                var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                if (!_allowedExts.Contains(ext))
                    throw new InvalidOperationException($"Extension {ext} not allowed.");
            }
        }
    }
}