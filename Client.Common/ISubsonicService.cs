using System;
using Client.Common.Results;

namespace Client.Common
{
    public interface ISubsonicService
    {
        SubsonicServiceConfiguration Configuration { get; set; }

        Func<IGetRootResult> GetRootIndex { get; set; }

        Func<int, IGetMusicDirectoryResult> GetMusicDirectory { get; set; }

        Func<string, ISearchResult> Search { get; set; }

        Uri GetUriForFileWithId(int id);
    }
}