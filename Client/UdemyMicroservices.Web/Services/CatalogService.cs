using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UdemyMicroservices.Shared.Dtos;
using UdemyMicroservices.Web.Helpers;
using UdemyMicroservices.Web.Models;
using UdemyMicroservices.Web.Models.Catalog;
using UdemyMicroservices.Web.Services.Interfaces;

namespace UdemyMicroservices.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient client, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _photoStockService = photoStockService ?? throw new ArgumentNullException(nameof(photoStockService));
            _photoHelper = photoHelper ?? throw new ArgumentNullException(nameof(photoHelper));
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _client.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response = await _client.GetAsync("courses");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSuccess.Data.ForEach(x =>
            {
                x.Picture = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _client.GetAsync($"courses/GetAllByUserId/{ userId }");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSuccess.Data.ForEach(x =>
            {
                x.Picture = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

            return responseSuccess.Data;
        }

        public async Task<CourseViewModel> GetCourseByCourseId(string courseId)
        {
            var response = await _client.GetAsync($"courses/{ courseId }");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            return responseSuccess.Data;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput course)
        {
            var resultPhoto = await _photoStockService.UploadPhoto(course.PhotoFormFile);

            if (resultPhoto != null)
            {
                course.Picture = resultPhoto.Url;
            }

            var response = await _client.PostAsJsonAsync<CourseCreateInput>("courses", course);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput course)
        {
            var resultPhoto = await _photoStockService.UploadPhoto(course.PhotoFormFile);

            if (resultPhoto != null)
            {
                await _photoStockService.DeletePhoto(course.Picture);

                course.Picture = resultPhoto.Url;
            }

            var response = await _client.PutAsJsonAsync<CourseUpdateInput>("courses", course);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _client.DeleteAsync($"courses/{ courseId }");

            return response.IsSuccessStatusCode;
        }
    }
}