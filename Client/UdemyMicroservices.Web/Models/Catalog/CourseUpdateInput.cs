﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyMicroservices.Web.Models.Catalog
{
    public class CourseUpdateInput
    {
        public string Id { get; set; }

        [Display(Name = "Kurs ismi")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Kurs açıklaması")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Kurs resmi")]
        public decimal Price { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Kurs resmi")]
        public string Picture { get; set; }

        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs kategorisi")]
        [Required]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
