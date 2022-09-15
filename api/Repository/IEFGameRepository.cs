using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Repository
{
    public interface IEFGameRepository
    {
        IEnumerable<vGame> Get();
        vGame Get(int id);

        bool Create(vGame game);
        bool Delete(int id);
        bool Edit(int id, vGame vGame);

        List<vGame> GetSortedGames(Sort sorts);
    }
}
