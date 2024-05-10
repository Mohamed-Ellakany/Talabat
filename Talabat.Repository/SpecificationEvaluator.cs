﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery ,ISpecifications<T> spec)
        {
            var query = inputQuery;

            if(spec.Creatira is not null) 
            {
                query=query.Where(spec.Creatira);
            }

             
            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPaginationEnable )
            {
                query=query.Skip(spec.Skip ) .Take(spec.Take);
            }


            query = spec.Includes.Aggregate(query,(CurrentQuery , newExpression)=>CurrentQuery.Include(newExpression));


            return query;
        }

    }
}
