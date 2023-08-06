using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using Xamarin.Forms;
using Atown10_CMobile.Models;
using System.Diagnostics;

namespace Atown10_CMobile.Services
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database()
        {
            _database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            _database.ExecuteAsync("PRAGMA foreign_keys = ON;");
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();

            //Code to generate demonstration information in the database
            var terms = _database.Table<Term>().ToListAsync().Result;
            if (terms.Count == 0)
            {
                var term = new Term
                {
                    Title = "Test Term",
                    StartDate = new DateTime(2023, 1, 1),
                    EndDate = new DateTime(2023, 12, 31)
                };
                _database.InsertAsync(term).Wait();

                var insertedTerm = _database.Table<Term>().FirstOrDefaultAsync(t => t.Title == "Test Term").Result;

                if (insertedTerm != null)
                {
                    var course = new Course
                    {
                        Name = "Test Course",
                        StartDate = new DateTime(2023, 1, 1),
                        EndDate = new DateTime(2023, 12, 31),
                        Status = "In Progress",
                        InstructorName = "Test Instructor",
                        InstructorPhone = "123-456-7890",
                        InstructorEmail = "test@test.com",
                        Notes = "Test Notes",
                        TermId = insertedTerm.Id
                    };
                    _database.InsertAsync(course).Wait();

                    var insertedCourse = _database.Table<Course>().FirstOrDefaultAsync(c => c.Name == "Test Course").Result;
                }
            }
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

        public async Task<List<Course>> GetCoursesForTermAsync(int termId)
        {
            List<Course> courses = await _database.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
            return courses;
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
