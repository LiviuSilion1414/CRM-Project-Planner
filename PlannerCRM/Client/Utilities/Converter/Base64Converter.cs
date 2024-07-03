namespace PlannerCRM.Client.Utilities.Converter;

public class Base64Converter
{
    const int MAX_FILE_SIZE = 1024 * 5000; //5MB of data

    public static async Task<(string thumbnail, string imageType)> ConvertImageAsync(InputFileChangeEventArgs args) {
        var imgFile = args.File;
        var buffers = new byte[imgFile.Size];

        await imgFile.OpenReadStream(MAX_FILE_SIZE).ReadAsync(buffers);

        var imageType = imgFile.ContentType;
        var thumbnail = $"data:{imageType};base64,{Convert.ToBase64String(buffers)}";

        return (thumbnail, imageType);
    }
}