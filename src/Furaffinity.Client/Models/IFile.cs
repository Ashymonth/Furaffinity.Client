namespace Furaffinity.Client.Models;

internal interface IFile
{
    string FileName { get; }
    
    byte[] Data { get; }
    
    string Extension { get; }
}