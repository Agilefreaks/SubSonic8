using System;
using Client.Common.Results;

namespace Client.Common
{
    public interface ISubsonicService
    {
        SubsonicServiceConfiguration Configuration { get; set; }

        IGetRootResult GetRootIndex();

        IGetMusicDirectoryResult GetMusicDirectory(int id);

        Uri GetUriForFileWithId(int id);
    }
}