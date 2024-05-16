﻿using Vigig.DAL.Interfaces;
using Vigig.Domain.Entities;

namespace Vigig.DAL.Implementations;

public class EventImageRepository : GenericRepository<EventImage>
{
    public EventImageRepository(IAppDbContext dbContext) : base(dbContext)
    {
    }
}