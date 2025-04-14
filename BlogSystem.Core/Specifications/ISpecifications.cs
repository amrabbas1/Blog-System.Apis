using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications
{
    public interface ISpecifications<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>> Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        List<string> ThenIncludes { get; }
    }
}
