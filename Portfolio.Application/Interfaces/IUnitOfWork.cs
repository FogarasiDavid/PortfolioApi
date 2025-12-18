using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Application.Interfaces
{
    public interface IUnitOfWork
    {
        //projectrepository összekötése
        IProjectRepository ProjectRepository { get; }

        //mentés adatbázisba 
        Task<int> CommitAsync();
        //eroforrás felszabaditás
        public void Dispose();
    }
}
