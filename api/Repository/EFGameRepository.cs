using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Repository
{
    public class EFGameRepository : IEFGameRepository
    {
        private readonly apiContext _context;

        public EFGameRepository(apiContext context)
        {
            _context = context;

            //CreateSomeGames();
        }
        private void CreateSomeGames()
        {
            if (!_context.Games.Any() && !_context.Studios.Any())
            {
                _context.Games.Add(new Game { Id = 0, Name = "Dota 2", StudId = 1 });
                _context.Games.Add(new Game { Id = 1, Name = "Cs GO", StudId = 1 });
                _context.Games.Add(new Game { Id = 2, Name = "Soviet Republic", StudId = 2 });

                _context.Studios.Add(new Studio { Id = 1, Name = "Valve" });
                _context.Studios.Add(new Studio { Id = 2, Name = "3dVision" });

                _context.SaveChanges();
            }
        }
        public IEnumerable<vGame> Get()
        {
            List<vGame> vGames = new(); 

            List<Game> gamesJSON = _context.Games.ToList();
            foreach (Game gameJSON in gamesJSON)
            {
                vGames.Add(new vGame()
                {
                    Name = gameJSON.Name == String.Empty ? "Unknown" : gameJSON.Name,
                    Studio = GetStudioNameById(gameJSON.StudId)
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
            }
            else
            {
                vGame.Name = gameJSON.Name == String.Empty ? "Unknown" : gameJSON.Name;
                vGame.Studio = GetStudioNameById(gameJSON.StudId);
            }

            return vGame;
        }
        public bool Create(vGame vGame)
        {
            // TODO: check same games
            var studId = GetStudioIdByName(vGame.Studio) == -1 ? CreateStudioByName(vGame.Studio) : GetStudioIdByName(vGame.Studio);
            Game gameJSON = new Game() { Id = GetNextGameId(), Name = vGame.Name, StudId = studId };

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

                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

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
            int id = _context.Studios.FirstOrDefault(x => x.Name == studioName)?.Id ?? -1;
            return id;
        }

        private string? GetStudioNameById(int? id)
        {
            return _context.Studios.FirstOrDefault(x => x.Id == id)?.Name ?? "Unknown";
        }
    }
}
