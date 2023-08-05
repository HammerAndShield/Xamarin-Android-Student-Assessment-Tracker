using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;
using Atown10_CMobile.Models;

namespace Atown10_CMobile.Services
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database()
        {
            _database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
        }

        //Term methods
        public Task<int> SaveTermAsync(Term term)
        {
            if (term.Id != 0)
            {
                return _database.UpdateAsync(term);
            }
            else
            {
                return _database.InsertAsync(term);
            }
        }

        public Task<Term> GetTermAsync(int id)
        {
            return _database.Table<Term>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateTermAsync(Term term)
        {
            return _database.UpdateAsync(term);
        }

        public Task<int> DeleteTermAsync(Term term)
        {
            return _database.DeleteAsync(term);
        }

        public Task<List<Term>> GetTermsAsync()
        {
            return _database.Table<Term>().ToListAsync();
        }

        public Task<List<Course>> GetCoursesForTermAsync(int termId)
        {
            return _database.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        //Course methods
        
        public Task<int> SaveCourseAsync(Course course)
        {
            if (course.Id != 0)
            {
                return _database.UpdateAsync(course);
            }
            else
            {
                return _database.InsertAsync(course);
            }
        }

        public Task<Course> GetCourseAsync(int id)
        {
            return _database.Table<Course>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateCourseAsync(Course course)
        {
            return _database.UpdateAsync(course);
        }

        public Task<int> DeleteCourseAsync(Course course)
        {
            return _database.DeleteAsync(course);
        }

        public Task<List<Course>> GetCoursesAsync()
        {
            return _database.Table<Course>().ToListAsync();
        }
        
        public Task<List<Assessment>> GetAssessmentsForCourseAsync(int courseId)
        {
            return _database.Table<Assessment>().Where(a => a.CourseId == courseId).ToListAsync();
        }

        //Assessment operations
        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.Id != 0)
            {
                return _database.UpdateAsync(assessment);
            }
            else
            {
                return _database.InsertAsync(assessment);
            }
        }

        public Task<Assessment> GetAssessmentAsync(int id)
        {
            return _database.Table<Assessment>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateAssessmentAsync(Assessment assessment)
        {
            return _database.UpdateAsync(assessment);
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }

        public Task<List<Assessment>> GetAssessmentsAsync()
        {
            return _database.Table<Assessment>().ToListAsync();
        }

    }
}
