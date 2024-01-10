using Microsoft.AspNetCore.Components.Forms;

namespace PlannerCRM.Client.Utilities.Converter;

public class Base64Converter
{
    public static async Task<(string thumbnail, string imageType, string fileName)> ConvertImageAsync(InputFileChangeEventArgs args, long allowedSize) {
        var imgFile = args.File;
        var buffers = new byte[imgFile.Size];

        await imgFile.OpenReadStream().ReadAsync(buffers);

        var imageType = imgFile.ContentType;
        var fileName = imgFile.Name;        
        var thumbnail = $"data:{imageType};base64,{Convert.ToBase64String(buffers)}";

        return (thumbnail, imageType, fileName);
    }
}