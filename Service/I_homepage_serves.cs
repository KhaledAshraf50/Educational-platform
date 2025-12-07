using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Service
{
    public interface I_homepage_serves
    {

         List<Subject> GetSubjects { get;  }
    
       
    }
}
