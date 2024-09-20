using System;

namespace BUtil.Core.Services;

public interface ILegacyObsoleteArchiver
{
    bool Extract(
        string archive,
        string password,
        string outputDirectory);
}
