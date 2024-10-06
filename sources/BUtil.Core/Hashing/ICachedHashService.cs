using System;

namespace BUtil.Core.Hashing;

public interface ICachedHashService : IDisposable
{
    string GetSha512(string file, bool trySpeedupNextTime);
}
