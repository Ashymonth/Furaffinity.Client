// using Furaffinity.Client.Validators;
//
// namespace Furaffinity.Client.Models;
//
// public class SubmissionThumbnail : IFile
// {
//     internal SubmissionThumbnail(string fileName, byte[] data)
//     {
//         if (string.IsNullOrWhiteSpace(fileName))
//         {
//             throw new ArgumentNullException(nameof(fileName));
//         }
//
//         if (data is null || data.Length == 0)
//         {
//             throw new ArgumentNullException(nameof(data));
//         }
//
//         FileName = fileName;
//         Data = data;
//
//         new FileValidator().ValidateThumbnailFile(this);
//     }
//
//     public string FileName { get; }
//     
//     public byte[] Data { get; }
//     
//     public string Extension => Path.GetExtension(FileName);
// }