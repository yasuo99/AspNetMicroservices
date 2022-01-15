﻿using MediatR;
using Ordering.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Constracts.Persistence
{
    public interface IRequestWrapper<T> : IRequest<Response<T>>
    {

    }
    public interface IHandlerWrapper<TIn, TOut> : IRequestHandler<TIn, Response<TOut>> where TIn : IRequestWrapper<TOut>
    {

    }
}
