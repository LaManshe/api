using api.Data;
using api.Models;

namespace api.Repository
{
    public class EFGameRepository : IEFGameRepository
    {
        #region Fields
        private readonly apiContext _context;
        #endregion
        #region Constructor
        public EFGameRepository(apiContext context)
        {
            _context = context;
        }
        #endregion
        #region Public methods
        public IEnumerable<vGame> Get()
        {
            List<vGame> vGames = new(); 

            List<Game> gamesJSON = _context.Games.ToList();
            foreach (Game gameJSON in gamesJSON)
            {
                vGames.Add(new vGame()
                {
                    Name = gameJSON.Name == String.Empty ? "Unknown" : gameJSON.Name,
                    Studio = GetStudioNameById(gameJSON.StudId),
                    Genres = gameJSON.Genres == String.Empty ? "Unknown" : gameJSON.Genres
                });
            }

            return vGames;
        }
        public vGame Get(int id)
        {
            vGame vGame = new();

            Game gameJSON = _context.Games.FirstOrDefault(x => x.Id == id); 

            if (gameJSON == null)
            {
                vGame.Name = "Unknown";
                vGame.Studio = "Unknown";
                vGame.Genres = "Unknown";
            }
            else
            {
                vGame.Name = gameJSON.Name == String.Empty ? "Unknown" : gameJSON.Name;
                vGame.Studio = GetStudioNameById(gameJSON.StudId);
                vGame.Genres = gameJSON.Genres == String.Empty ? "Unknown" : gameJSON.Genres;
            }

            return vGame;
        }
        public bool Create(vGame vGame)
        {
            if (GameExist(vGame.Name))
            {
                return false;
            }

            var studId = GetStudioIdByName(vGame.Studio) == -1 ? CreateStudioByName(vGame.Studio) : GetStudioIdByName(vGame.Studio);
            Game gameJSON = new Game() { Id = GetNextGameId(), Name = vGame.Name, StudId = studId, Genres = vGame.Genres };

            if(gameJSON != null)
            {
                _context.Games.Add(gameJSON);
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            Game game = _context.Games.FirstOrDefault(x => x.Id == id);

            if(game != null)
            {
                _context.Remove(game);
                _context.SaveChanges();

                if (!_context.Games.Contains(_context.Games.FirstOrDefault(x => x.StudId == game.StudId)))
                {
                    Studio studToRemove = _context.Studios.FirstOrDefault(x => x.Id == game.StudId);
                    _context.Studios.Remove(studToRemove);
                    _context.SaveChanges();
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Edit(int id, vGame vGame)
        {
            Game game = _context.Games.FirstOrDefault(x => x.Id == id);

            if(game != null)
            {
                game.Name = vGame.Name;
                game.StudId = GetStudioIdByName(vGame.Studio) == -1 ? CreateStudioByName(vGame.Studio) : GetStudioIdByName(vGame.Studio);
                game.Genres = vGame.Genres;

                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        public List<vGame> GetSortedGames(Sort sorts)
        {
            List<vGame> sortedGames = new();
            List<Game> gamesJSON = _context.Games.ToList();

            foreach(Game gameJSON in gamesJSON)
            {
                List<string> gameGenres = gameJSON.Genres.Split(',').ToList();
                for(int i = 0; i < gameGenres.Count; i++)
                {
                    gameGenres[i] = gameGenres[i].Trim().ToLower();
                }
                List<string> sortsNormilized = sorts.Genres;
                for (int i = 0; i < sortsNormilized.Count; i++)
                {
                    sortsNormilized[i] = sortsNormilized[i].Trim().ToLower();
                }

                if (gameGenres.Intersect(sortsNormilized).Any())
                {
                    sortedGames.Add(new vGame()
                    {
                        Name = gameJSON.Name == String.Empty ? "Unknown" : gameJSON.Name,
                        Studio = GetStudioNameById(gameJSON.StudId),
                        Genres = gameJSON.Genres == String.Empty ? "Unknown" : gameJSON.Genres
                    });
                }
            }

            return sortedGames;
        }
        #endregion
        #region Private methods
        private int CreateStudioByName(string? name)
        {
            Studio newStudio = new Studio() { Id = GetNextStudioId(), Name = name };
            _context.Studios.Add(newStudio);
            return newStudio.Id;
        }

        private int GetNextGameId()
        {
            return _context.Games.Count() == 0 ? 1 : _context.Games.Max(x => x.Id) + 1;
        }

        private int GetNextStudioId()
        {
            return _context.Studios.Count() == 0 ? 1 : _context.Studios.Max(x => x.Id) + 1;
        }

        private int GetStudioIdByName(string studioName)
        {
            int id = _context.Studios.FirstOrDefault(x => x.Name == studioName)?.Id ?? - 1;
            return id;
        }

        private string? GetStudioNameById(int? id)
        {
            return _context.Studios.FirstOrDefault(x => x.Id == id)?.Name ?? "Unknown";
        }
        private bool GameExist(string? name)
        {
            if (_context.Games.Any(x => x.Name == name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
