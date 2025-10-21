namespace CMCS.Services
{
    public interface IFileValidator
    {
        void EnsureValid(IEnumerable<IFormFile> files, int existingCount = 0);
    
}
}
