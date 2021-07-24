﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models.Catalog;

namespace UdemyMicroservices.Web.Validators
{
    public class CourseUpdateInputValidator : AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama alanı boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Süre alanı boş olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("Hatalı fiyat formatı");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori alanı seçiniz");
        }
    }
}
