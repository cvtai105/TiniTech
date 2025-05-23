using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedKernel.Models;
using WebSharedModels.Dtos.Rating;

namespace WebMVC.Services.Abstractions;

public interface IRatingService
{
    Task<ProductRatingDto> GetByProduct(ProductRatingQuery query);
    Task<int> SubmitRatingAsync(SubmitRatingRequest ratingDto);
}
