﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : Ispecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }=null;
        public List<Expression<Func<T, object>>> Includes { get ; set ; }= new List<Expression<Func<T, object>>> ();
        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
        public int Skip { get ; set; } = 0 ;
        public int Take { get ; set ; } = 0 ;
        public bool IsPaginationEnabled { get; set; } = false ;

        public BaseSpecification()
        {
            
        }

        public BaseSpecification( Expression<Func<T,bool>> CriteriaExpression)
        {
            
            Criteria = CriteriaExpression;

        }



        public void AddOrderBy(Expression<Func<T, object>> OrderByExpression)
        {

            OrderBy = OrderByExpression;

        }


        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExpression)
        {

            OrderByDesc = OrderByDescExpression;

        }

        public void ApplyPagination( int skip , int take)
        {
            Skip = skip;
            Take = take;

        }

    }


    
}