﻿using Vigig.Domain.Entities;

namespace Vigig.Service.Exceptions.NotFound;

public class BookingNotFoundException : EntityNotFoundException<Booking>
{
    public BookingNotFoundException(object id) : base(id)
    {
    }
}