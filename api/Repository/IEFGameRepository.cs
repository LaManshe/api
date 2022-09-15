using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Repository
{
    public interface IEFGameRepository
    {
        /// <summary>
        /// Get all games
        /// </summary>
        /// <returns>Returns IEnumerable games (visual style).</returns>
        IEnumerable<vGame> Get();
        /// <summary>
        /// Get game by id
        /// </summary>
        /// <param name="id">Game id.</param>
        /// <returns>Returns game (visual style).</returns>
        vGame Get(int id);
        /// <summary>
        /// Create game in DB with your options
        /// </summary>
        /// <param name="game">Game as visual style object.</param>
        /// <returns>Returns bool succses.</returns>
        bool Create(vGame game);
        /// <summary>
        /// Delete game from DB by id
        /// </summary>
        /// <param name="id">Game id.</param>
        /// <returns>Returns bool succses.</returns>
        bool Delete(int id);
        /// <summary>
        /// Edit a game by id with your options
        /// </summary>
        /// <param name="id">Game id.</param>
        /// <param name="id">Options.</param>
        /// <returns>Returns bool succses.</returns>
        bool Edit(int id, vGame vGame);
        /// <summary>
        /// Filtering by genres
        /// </summary>
        /// <param name="sorts">Sort parameters.</param>
        /// <returns>Returns list of find games by genres.</returns>
        List<vGame> GetSortedGames(Sort sorts);
    }
}
