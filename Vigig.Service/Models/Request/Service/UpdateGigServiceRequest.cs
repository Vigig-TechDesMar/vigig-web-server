﻿using System.ComponentModel.DataAnnotations;
using Vigig.Service.Attributes;

namespace Vigig.Service.Models.Request.Service;

public class UpdateGigServiceRequest
{
    public Guid Id { get; set; }

    [Required]
    public required string ServiceName { get; set; } 

    public string Description { get; set; } = string.Empty;
    
    [Required]
    [MinValue(0)]
    public double MinPrice { get; set; }
    
    [Required]
    [GreaterThanOrEqual("MinPrice")]
    public double MaxPrice { get; set; }
    
    [Required]
    [MinValue(0)]
    public double Fee { get; set; }
    
    public Guid ServiceCategoryId { get; set; }
}