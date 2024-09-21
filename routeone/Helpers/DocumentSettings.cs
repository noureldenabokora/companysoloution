namespace routeone.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1 get located folder path 
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            // 2 get fiel name and make it UNIQ 
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // get file path 
            string filePath = Path.Combine(folderPath, fileName);

            // save file as streams = [data per time] 
            using var fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);

            //return file name as i want to store it in db
            return fileName;
        }

        public static void DeleteFile(string FileName, string FolderName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName, FileName);

            if (File.Exists(filepath))
                File.Delete(filepath);
        }
    }
}
