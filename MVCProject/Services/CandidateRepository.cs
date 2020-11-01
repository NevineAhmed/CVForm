using CVSubTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVSubTask.Services
{
    public class CandidateRepository : ICandidateRepo
    {
        CVFormTaskEntities _context = new CVFormTaskEntities();
        public Candidate Add(Candidate model)
        {
            if (model != null)
            {
                _context.Candidates.Add(model);
                _context.SaveChanges();
                return model;
            }
            return null;
            
        }

        public IEnumerable<Candidate> Delete(int id)
        {
            var can = GetAll().FirstOrDefault(a => a.Id == id);
            if (can != null)
            {
                _context.Candidates.Remove(can);
                _context.SaveChanges();
                return GetAll();
            }

            else
            {
                return null;
            }
            
            
        }

        public IEnumerable<Candidate> GetAll()
        {
            return _context.Candidates.AsEnumerable();
        }

        public Candidate GetbyId(int id)
        {
            var can = GetAll().FirstOrDefault(a => a.Id == id);
            if (can != null)
            {
                return can;
            }
            else
            {
                return null;
            }
            
        }

        public Candidate Update(Candidate model)
        {
            
            var candidate = _context.Candidates.FirstOrDefault(a => a.Id == model.Id);
            if (candidate != null)
            {
                candidate.FullName = model.FullName;
                candidate.Age = model.Age;
                candidate.Gender = model.Gender;
                candidate.City = model.City;
                candidate.Area = model.Area;
                candidate.Address = model.Address;
                candidate.CV = model.CV;
                candidate.Image = model.Image;
                _context.SaveChanges();
                return candidate;
            }

            else
            {
                return null;
            }
            
        }
    }
}