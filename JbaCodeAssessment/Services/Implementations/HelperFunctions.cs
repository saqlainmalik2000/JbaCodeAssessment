using JbaCodeAssessment.Models;

namespace JbaCodeAssessment.Services.Implementations
{
    public static class HelperFunctions
    {
        public static FileTypes GetFileType(string filePath)
        {
            var fileExtension = System.IO.Path.GetExtension(filePath);
            switch (fileExtension)
            {
                case ".pre":
                    return FileTypes.Pre;

                default:
                    return FileTypes.NotSupported;
            }
        }
    }
}
