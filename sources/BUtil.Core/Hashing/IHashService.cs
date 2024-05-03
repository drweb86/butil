using System;

namespace BUtil.Core.Hashing;

public interface IHashService : IDisposable
{
    string GetSha512(string file, bool trySpeedupNextTime);
    string GetSha512(string content);
}
