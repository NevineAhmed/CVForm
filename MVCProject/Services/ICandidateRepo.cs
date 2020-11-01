using CVSubTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVSubTask.Services
{
   public interface ICandidateRepo
    {
        IEnumerable<Candidate> GetAll();

        Candidate GetbyId(int id);

        IEnumerable<Candidate> Delete(int id);

        Candidate Update(Candidate model);

        Candidate Add(Candidate model);
    }
}
