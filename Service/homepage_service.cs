using Luno_platform.Models;
using Luno_platform.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Luno_platform.Service
{
    public class Homepage_Service : I_homepage_serves
    {
        private readonly I_BaseRepository<Subject> _subjectRepo;

        public Homepage_Service(I_BaseRepository<Subject> subjectRepo)
        {
            _subjectRepo = subjectRepo;
        }

        public List<Subject> GetSubjects
        {
            get
            {
                return _subjectRepo.GetAll().ToList();
            }
        }

     
    }
}
