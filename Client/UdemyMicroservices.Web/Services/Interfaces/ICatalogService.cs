using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models.Catalog;

namespace UdemyMicroservices.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CategoryViewModel>> GetAllCategoryAsync();
        Task<List<CourseViewModel>> GetAllCourseAsync();
        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);
        Task<CourseViewModel> GetCourseByCourseId(string courseId);
        Task<bool> CreateCourseAsync(CourseCreateInput course);
        Task<bool> UpdateCourseAsync(CourseUpdateInput course);
        Task<bool> DeleteCourseAsync(string courseId);
    }
}
