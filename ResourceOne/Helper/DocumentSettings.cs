namespace ResourceOne.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            if (file != null)
            {
                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", FolderName);
                var fileName = $"{Guid.NewGuid()} {Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(FolderPath, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);
                return fileName;
            }
            else
            {
                return null;
            }
        }
        public static string DeleteFile(string fileName, string FolderName)
        {
            if (fileName != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", FolderName, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    return null;
                }
            }

            return null;
        }
    }
}
